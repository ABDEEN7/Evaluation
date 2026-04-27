
using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities;
using Evaluation.SharedHelper.Models.Api.ServiceDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using System.Threading.Tasks;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvEvaluationRequestAssignment(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvUser SrvUser, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)

    {

        public async Task PerformAssignAction(Guid requestId, List<SharedHelper.Models.Api.ActionEntitiesDTOs.AssignUserDTO?> users)
        {
           using  var _Uow = serviceScopeFactory.CreateScopedUow();

            var userEmails = users.Select(c => c!.Email).Distinct().ToList();

            var userDetails = _Uow.GetRepository<MinistryUser>().GetAllQueryFiltered()
                         .Where(c => userEmails.Contains(c.Email))
                         .Select(c => new { c.Id, c.Email })
                         .ToList();

            var userDetailsWithPartyType = userDetails.Select(u => new
            {
                u.Id,
                u.Email,
                PartyTypeId = users.FirstOrDefault(userDto => userDto?.Email == u.Email)?.PartyTypeId,
                IsDefault = users.FirstOrDefault(userDto => userDto?.Email == u.Email)?.IsLeader ?? false,
            }).ToList();

            var userPartyTypes = users.Select(x => x!.PartyTypeId).Distinct().ToList();

            var currentAssignments = _Uow.GetRepository<EvaluationRequestAssignment>().GetAllQueryFiltered()
                                           .Where(c => c.EvaluationRequestId == requestId && userPartyTypes.Contains(c.PartyTypeId))
                                           .ToList();

            var toBeDeleted = currentAssignments
                .Where(c => !userDetailsWithPartyType.Any(u => u.Id == c.MinistryUserId && u.PartyTypeId == c.PartyTypeId))
                .ToList();

            foreach (var assignment in toBeDeleted)
            {
                uow.GetRepository<EvaluationRequestAssignment>().Delete(assignment);
            }

            foreach (var assignment in currentAssignments)
            {
                var matchedUser = userDetailsWithPartyType
                    .FirstOrDefault(u => u.Id == assignment.MinistryUserId && u.PartyTypeId == assignment.PartyTypeId);

                if (matchedUser != null)
                {
                    assignment.IsLeader = matchedUser.IsDefault;
                    uow.GetRepository<EvaluationRequestAssignment>().Update(assignment);
                }
            }

            var newAssignments = userDetailsWithPartyType
                .Where(u => !currentAssignments.Any(c => c.MinistryUserId == u.Id && c.PartyTypeId == u.PartyTypeId))
                .Select(u => new EvaluationRequestAssignment
                {
                    MinistryUserId = u.Id,
                    EvaluationRequestId = requestId,
                    PartyTypeId = u.PartyTypeId!.Value,
                    IsLeader = u.IsDefault
                })
                .ToList();

            await uow.GetRepository<EvaluationRequestAssignment>().InsertRange(newAssignments);

        }

        //public async Task<bool> PerformAutoAssign(EvaluationRequest request, Guid actionId, Guid? country, Guid? university)
        //{
        //    var allowedPartyTypeIdsTask = GetAllowedPartyTypeIds(actionId, request.Id, request.Scholarship!, country, university);
        //    var creatorUserTask = SrvUser.GetByIDActiveNonDeleted(userInfo.UserId!.Value);
        //    Task<List<SchAssignment>> assignedUsersTask = request.ScholarshipId != null
        //        ? GetSchAssignetEmployee(request.ScholarshipId.Value)
        //        : Task.FromResult<List<SchAssignment>>(null!);


        //    var allowedPartyTypeIds = await allowedPartyTypeIdsTask;
        //    var creatorUser = await creatorUserTask;
        //    var assignedUsers = await assignedUsersTask;

        //    if (creatorUser == null || !allowedPartyTypeIds.Any())
        //        return false;

        //    var isMinistry = creatorUser is MinistryUser;
        //    Guid? finalUserId = null;

        //    // Step 1: If creator has allowed party type
        //    if (creatorUser.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId)))
        //    {
        //        finalUserId = creatorUser.Id;
        //    }

        //    // Step 2: If previous assigned user exists and still valid
        //    else if (assignedUsers?.Any() == true)
        //    {
        //        var validAssignedIds = assignedUsers
        //            .Where(x => x.MinistryUserId.HasValue)
        //            .Select(x => x.MinistryUserId!.Value)
        //            .ToList();

        //        var validAssignedUser = await uow.GetRepository<MinistryUser>()
        //            .GetAllQueryFiltered()
        //            .Include(u => u.UserPartTypes)
        //            .Where(u => validAssignedIds.Contains(u.Id) &&
        //                        u.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId)))
        //            .FirstOrDefaultAsync();

        //        finalUserId = validAssignedUser?.Id;
        //        if(finalUserId == null)
        //        {
        //            finalUserId = await FindUserIdWithMinimumAssignmentsAndAllowedPartyType(allowedPartyTypeIds);
        //        }
        //    }
        //    // Step 3: If user is not ministry, assign to user with least workload
        //    else 
        //    {
        //        finalUserId = await FindUserIdWithMinimumAssignmentsAndAllowedPartyType(allowedPartyTypeIds);
        //    }

        //    if (finalUserId.HasValue)
        //    {
        //        var assign = new EvaluationRequestAssignment
        //        {
        //            Id = Guid.NewGuid(),
        //            EvaluationRequestId = request.Id,
        //            MinistryUserId = finalUserId.Value,
        //            IsSchAssigner = false,
        //            PartyTypeId = allowedPartyTypeIds.FirstOrDefault()

        //        };

        //        await uow.GetRepository<EvaluationRequestAssignment>().InsertAsync(assign);
        //        return true;
        //    }
        //    return false;
        //}

        private async Task<Guid?> FindUserIdWithMinimumAssignmentsAndAllowedPartyType(List<Guid> allowedPartyTypeIds)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();

            // Step 1: Get all users with allowed and active party types
            var eligibleUsers = await uow.GetRepository<MinistryUser>()
                .GetAllQueryFiltered()
                .Include(u => u.UserPartTypes)
                .Where(u => u.UserPartTypes!.Any(pt => allowedPartyTypeIds.Contains(pt.PartyTypeId) && pt.IsActive == true))
                .Select(u => u.Id)
                .ToListAsync();

            if (!eligibleUsers.Any())
                return null;

            // Step 2: Get count of closed (non-open) assignments per user
            var assignmentCounts = await uow.GetRepository<EvaluationRequestAssignment>()
                .GetAllQueryFiltered()
                .Include(x => x.EvaluationRequest)
                .ThenInclude(sr => sr.ServiceStatus)
                .Where(a => a.IsActive == true &&
                            eligibleUsers.Contains(a.MinistryUserId) &&
                            a.EvaluationRequest.ServiceStatus!.ServiceStatusType!.IsOpen == false)
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


        private async Task<List<Guid>> GetAllowedPartyTypeIds(Guid actionId)
        {
            using var scope = serviceScopeFactory.CreateScopedUow();

            var assignablePartyTypes = await scope.GetRepository<ActionAssignPartyType>()
                .GetAllQueryFiltered(c => c.EvaluationActionId == actionId)
                .Include(x => x.PartyType)
                .AsNoTracking()
                .ToListAsync();

            if (!assignablePartyTypes.Any()) return new List<Guid>();

            return assignablePartyTypes.Select(x => x.PartyTypeId).ToList();
        }

        public async Task<List<AssignUserDTO>> GetAssignedUsers(EvaluationRequest request, Guid? actionId, bool showIsDefault)
        {
            string lang = _requestInfo.Lang;

            if (actionId is null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.lblActionNotFound);
            }

            var allowedPartyTypeIdsTask = GetAllowedPartyTypeIds(actionId.Value);
            var assignedUsersTask = GetRequestAssignet(request.Id);

            var allowedPartyTypeIds = await allowedPartyTypeIdsTask;
            if (!allowedPartyTypeIds.Any())
            {
                return new List<AssignUserDTO>();
            }
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var users = await scopedUow
							.GetRepository<MinistryUser>()
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
                            NameAr = user.NameAr,
                            NameEn = user.NameEn,
                            PartyTypeId = pt.PartyTypeId,
                            PartyTypeTitle = lang == "ar" ? pt.PartyType?.NameAr : pt.PartyType?.NameEn,
                            IsSelected = assignedUser != null,
                            ShowIsDefaultAssigner = showIsDefault,
                            IsLeader = assignedUser?.IsLeader ?? false
                        };
                    }))
                .Distinct()
                .ToList();

            return result;
        }



        //public async Task<List<SchAssignment>> GetSchAssignetEmployee(Guid schId)
        //{

        //    var assignmentUsers = await serviceScopeFactory.CreateScopedUow()
        //                                         .GetRepository<SchAssignment>()
        //                                         .GetAllQueryFiltered()
        //                                         .Where(c => c.ScholarshipId == schId )
        //                                         .ToListAsync();


        //    return assignmentUsers;
        //}
        public async Task<List<EvaluationRequestAssignment>> GetRequestAssignet(Guid requestId)
        {
			using var scope = serviceScopeFactory.CreateScopedUow();

			var assignmentUserIds = await scope
												 .GetRepository<EvaluationRequestAssignment>()
                                                 .GetAllQueryFiltered()
                                                 .Where(c => c.EvaluationRequestId == requestId)
                                                 .ToListAsync();


            return assignmentUserIds;
        }
        public async Task<NdaApproveResponse> ApproveNda(NdaApproveRequest dto)
        {
            var userId = userInfo.UserId!.Value;
            using var scope = serviceScopeFactory.CreateScopedUow();

            if (dto.EvaluationRequestId == Guid.Empty)
                return new NdaApproveResponse { IsSuccess = false, IsNdaApprovalPending = true, Message = "Invalid request id." };

            if (string.IsNullOrWhiteSpace(dto.ConflictReason) && dto.HasConflict)
                return new NdaApproveResponse { IsSuccess = false, IsNdaApprovalPending = true, Message = "Conflict reason is required." };

            var assignment = await scope.GetRepository<EvaluationRequestAssignment>()
                .GetAllActiveNonDeleted(x =>
                    x.EvaluationRequestId == dto.EvaluationRequestId &&
                    x.MinistryUserId == userId)
                .FirstOrDefaultAsync();

            if (assignment == null)
            {
                return new NdaApproveResponse
                {
                    IsSuccess = false,
                    IsNdaApprovalPending = true,
                    Message = "Assignment not found."
                };
            }

            assignment.Note = dto.ConflictReason;
            assignment.IsNDA =! dto.HasConflict;
            assignment.NdaDate = DateTime.UtcNow;
            //assignment.NdaStatusId = dto.NDAStatusId;

            await scope.CommitAsync();

            return new NdaApproveResponse
            {
                IsSuccess = true,
                IsNdaApprovalPending = false,
                Message = "Saved successfully."
            };
        }
        public async Task<List<GetServiceStatusDR>> GetServiceStatus()
        {
            return await uow
                .GetRepository<ServiceStatus>()
                .GetAllActiveNonDeleted()
                .Select(x => new GetServiceStatusDR
                {
                    Id = x.Id,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn
                })
                .ToListAsync();
        }
    }
}
