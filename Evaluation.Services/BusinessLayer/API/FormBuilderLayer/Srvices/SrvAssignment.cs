using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scholarship.DAL.Framework;
using Scholarship.DAL.Models.ActionEntities;
using Scholarship.DAL.Models.Base;
using Scholarship.DAL.Models.ScholarshipEntity;
using Scholarship.DAL.Models.ServiceRequestEntities;
using Scholarship.Services.AdminBusinessLayer;
using Scholarship.Services.Extensions;
using Scholarship.Services.Models.API;
using Scholarship.Services.Special;
using Scholarship.SharedHelper.Enums;
using Scholarship.SharedHelper.Exceptions;
using Scholarship.SharedHelper.Models;
using Scholarship.SharedHelper.Models.Api.ActionEntitiesDTOs;
using System.Linq;
using Xceed.Document.NET;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvAssignment (SrvServiceRequest SrvServiceRequest, IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvUser SrvUser, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)

    {

        public async Task PerformAssignAction(Guid requestId, List<AssignUserDTO?> users)
        {
            var _Uow = serviceScopeFactory.CreateScopedUow();

            var userEmails = users.Select(c => c!.Email).Distinct().ToList();

            var userDetails = _Uow.GetRepository<UserProfile>().GetAllQueryFiltered()
                         .Where(c => userEmails.Contains(c.Email))
                         .Select(c => new { c.Id, c.Email })
                         .ToList();

            var userDetailsWithPartyType = userDetails.Select(u => new
            {
                u.Id,
                u.Email,
                PartyTypeId = users.FirstOrDefault(userDto => userDto?.Email == u.Email)?.PartyTypeId,
                IsDefault = users.FirstOrDefault(userDto => userDto?.Email == u.Email)?.IsDefault ?? false,
            }).ToList();

            var userPartyTypes = users.Select(x => x!.PartyTypeId).Distinct().ToList();

            var currentAssignments = _Uow.GetRepository<RequestAssignment>().GetAllQueryFiltered()
                                           .Where(c => c.ServiceRequestId == requestId && userPartyTypes.Contains(c.PartyTypeId))
                                           .ToList();

            var toBeDeleted = currentAssignments
                .Where(c => !userDetailsWithPartyType.Any(u => u.Id == c.MinistryUserId && u.PartyTypeId == c.PartyTypeId))
                .ToList();

            foreach (var assignment in toBeDeleted)
            {
                uow.GetRepository<RequestAssignment>().Delete(assignment);
            }

            foreach (var assignment in currentAssignments)
            {
                var matchedUser = userDetailsWithPartyType
                    .FirstOrDefault(u => u.Id == assignment.MinistryUserId && u.PartyTypeId == assignment.PartyTypeId);

                if (matchedUser != null)
                {
                    assignment.IsSchAssigner = matchedUser.IsDefault;
                    uow.GetRepository<RequestAssignment>().Update(assignment);
                }
            }

            var newAssignments = userDetailsWithPartyType
                .Where(u => !currentAssignments.Any(c => c.MinistryUserId == u.Id && c.PartyTypeId == u.PartyTypeId))
                .Select(u => new RequestAssignment
                {
                    MinistryUserId = u.Id,
                    ServiceRequestId = requestId,
                    PartyTypeId = u.PartyTypeId!.Value,
                    IsSchAssigner = u.IsDefault
                })
                .ToList();

            await uow.GetRepository<RequestAssignment>().InsertRange(newAssignments);

        }

        public async Task<bool> PerformAutoAssign(ServiceRequest request, Guid actionId, Guid? country, Guid? university)
        {
            var allowedPartyTypeIdsTask = GetAllowedPartyTypeIds(actionId, request.Id, request.Scholarship!, country, university);
            var creatorUserTask = SrvUser.GetByIDActiveNonDeleted(userInfo.UserId!.Value);
            Task<List<SchAssignment>> assignedUsersTask = request.ScholarshipId != null
                ? GetSchAssignetEmployee(request.ScholarshipId.Value)
                : Task.FromResult<List<SchAssignment>>(null!);

         
            var allowedPartyTypeIds = await allowedPartyTypeIdsTask;
            var creatorUser = await creatorUserTask;
            var assignedUsers = await assignedUsersTask;

            if (creatorUser == null || !allowedPartyTypeIds.Any())
                return false;

            var isMinistry = creatorUser is MinistryUser;
            Guid? finalUserId = null;

            // Step 1: If creator has allowed party type
            if (creatorUser.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId)))
            {
                finalUserId = creatorUser.Id;
            }

            // Step 2: If previous assigned user exists and still valid
            else if (assignedUsers?.Any() == true)
            {
                var validAssignedIds = assignedUsers
                    .Where(x => x.MinistryUserId.HasValue)
                    .Select(x => x.MinistryUserId!.Value)
                    .ToList();

                var validAssignedUser = await uow.GetRepository<UserProfile>()
                    .GetAllQueryFiltered()
                    .Include(u => u.UserPartTypes)
                    .Where(u => validAssignedIds.Contains(u.Id) &&
                                u.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId)))
                    .FirstOrDefaultAsync();

                finalUserId = validAssignedUser?.Id;
                if(finalUserId == null)
                {
                    finalUserId = await FindUserIdWithMinimumAssignmentsAndAllowedPartyType(allowedPartyTypeIds);
                }
            }
            // Step 3: If user is not ministry, assign to user with least workload
            else 
            {
                finalUserId = await FindUserIdWithMinimumAssignmentsAndAllowedPartyType(allowedPartyTypeIds);
            }

            if (finalUserId.HasValue)
            {
                var assign = new RequestAssignment
                {
                    Id = Guid.NewGuid(),
                    ServiceRequestId = request.Id,
                    MinistryUserId = finalUserId.Value,
                    IsSchAssigner = false,
                    PartyTypeId = allowedPartyTypeIds.FirstOrDefault()

                };

                await uow.GetRepository<RequestAssignment>().InsertAsync(assign);
                return true;
            }
            return false;
        }

        private async Task<Guid?> FindUserIdWithMinimumAssignmentsAndAllowedPartyType(List<Guid> allowedPartyTypeIds)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();

            // Step 1: Get all users with allowed and active party types
            var eligibleUsers = await uow.GetRepository<UserProfile>()
                .GetAllQueryFiltered()
                .Include(u => u.UserPartTypes)
                .Where(u => u.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId) && pt.IsActive==true))
                .Select(u => u.Id)
                .ToListAsync();

            if (!eligibleUsers.Any())
                return null;

            // Step 2: Get count of closed (non-open) assignments per user
            var assignmentCounts = await uow.GetRepository<RequestAssignment>()  
                .GetAllQueryFiltered()
                .Include(x => x.ServiceRequest)
                .ThenInclude(sr => sr.Status)
                .Where(a => a.IsActive == true &&
                            eligibleUsers.Contains(a.MinistryUserId!.Value) &&
                            a.ServiceRequest.Status!.IsOpen == false)
                .GroupBy(a => a.MinistryUserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            // Step 3: Find user with minimum assignment count
            var userWithLeastAssignments = eligibleUsers
                .Select(userId => new
                {
                    UserId = userId,
                    Count = assignmentCounts.FirstOrDefault(x => x.UserId == userId)?.Count ?? 0
                })
                .OrderBy(x => x.Count)
                .FirstOrDefault();

            return userWithLeastAssignments?.UserId;
        }


        private async Task<List<Guid>> GetAllowedPartyTypeIds(Guid actionId, Guid requestId, ScholarshipData scholarship, Guid? requestCountry, Guid? university)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();

            // Fetch assignable party types based on actionId
            var assignablePartyTypes = await uow.GetRepository<ActionAssignPartyType>()
                .GetAllQueryFiltered(c => c.ScholarshipActionId == actionId)
                .Include(x => x.PartyType)
                .ThenInclude(x => x!.PartyTypeCountyUniversity)
                .AsNoTracking()
                .ToListAsync();

            // If no assignable party types found, return an empty list
            if (!assignablePartyTypes.Any()) return new List<Guid>();

            // If requestCountry and university are not provided, get them from the request or scholarship
            if (!requestCountry.HasValue || !university.HasValue)
            {
                if (scholarship != null)
                {
                    // If scholarship is not null, use its CountryId and UniversityId
                    requestCountry = scholarship.CountryId;
                    university = scholarship.UniversityId;
                }
                else
                {
                    // If scholarship is null, fetch from the request
                    var countryUniversity = await SrvServiceRequest.GetRequestCountryAndUniversityAsync(requestId);
                    requestCountry = countryUniversity.Item1;
                    university = countryUniversity.Item2;
                }
            }

            // Filter assignable party types based on the country and university conditions
            return assignablePartyTypes
                .Where(pt => pt.PartyType?.PartyTypeCountyUniversity == null ||
                             !pt.PartyType.PartyTypeCountyUniversity.Any() ||
                             pt.PartyType.PartyTypeCountyUniversity.Any(cu =>
                                 cu.CountryId == requestCountry &&
                                 (cu.UniversityId == null || cu.UniversityId == university)))
                .Select(pt => pt.PartyTypeId)
                .ToList();
        }

        public async Task<List<AssignUserDTO>> GetAssignedUsers(ServiceRequest request, Guid? actionId, bool showIsDefault)
        {
            string lang = _requestInfo.Lang;

            if (actionId is null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.lblActionNotFound);
            }

            var allowedPartyTypeIdsTask =  GetAllowedPartyTypeIds( actionId.Value,request.Id, request.Scholarship!, null, null);
            var assignedUsersTask =  GetRequestAssignet(request.Id);

            var allowedPartyTypeIds = await allowedPartyTypeIdsTask;
            if (!allowedPartyTypeIds.Any())
            {
                return new List<AssignUserDTO>();
            }

            var users = await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<UserProfile>()
                            .GetAllQueryFiltered()
                            .Include(u => u.UserPartTypes!)
                                .ThenInclude(pt => pt.PartyType)
                            .Where(u => u.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId)))
                            .AsNoTracking()
                            .ToListAsync();

            var assignedUsers = await assignedUsersTask;

            var result = users
                .SelectMany(user => user.UserPartTypes!
                    .Where(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId))
                    .Select(pt =>
                    {
                        var assignedUser = assignedUsers
                            .FirstOrDefault(x => x.MinistryUserId == user.Id && x.PartyTypeId == pt.PartyTypeId);

                        return new AssignUserDTO
                        {
                            Email = user.Email,
                            NameAr = user.FullNameAr,
                            NameEn = user.FullNameEn,
                            PartyTypeId = pt.PartyTypeId,
                            PartyTypeTitle = lang == "ar" ? pt.PartyType?.NameAr : pt.PartyType?.NameEn,
                            IsSelected = assignedUser != null,
                            ShowIsDefaultAssigner = showIsDefault,
                            IsDefault = assignedUser?.IsSchAssigner ?? false
                        };
                    }))
                .Distinct()
                .ToList();

            return result;
        }

        public async Task<List<RequestAssignment>> GetAssignetPermanentEmployee(Guid requestId)
        {

            var assignmentUserIds = await serviceScopeFactory.CreateScopedUow()
                                                 .GetRepository<RequestAssignment>()
                                                 .GetAllQueryFiltered()
                                                 .Where(c => c.ServiceRequestId == requestId && c.IsSchAssigner == true)
                                                 .ToListAsync();


            return assignmentUserIds;
        }

        public async Task<List<SchAssignment>> GetSchAssignetEmployee(Guid schId)
        {

            var assignmentUsers = await serviceScopeFactory.CreateScopedUow()
                                                 .GetRepository<SchAssignment>()
                                                 .GetAllQueryFiltered()
                                                 .Where(c => c.ScholarshipId == schId )
                                                 .ToListAsync();


            return assignmentUsers;
        }
        public async Task<List<RequestAssignment>> GetRequestAssignet(Guid requestId)
        {

            var assignmentUserIds = await serviceScopeFactory.CreateScopedUow()
                                                 .GetRepository<RequestAssignment>()
                                                 .GetAllQueryFiltered()
                                                 .Where(c => c.ServiceRequestId == requestId)
                                                 .ToListAsync();


            return assignmentUserIds;
        }

    }
}
