using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvUser( IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
           : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {

      
        public async Task<MinistryUser?> GetByIDActiveNonDeleted(Guid userId)
        {
            using var scope = serviceScopeFactory.CreateScopedUow();

			var UserProfile = await scope.GetRepository<MinistryUser>().GetByIDActiveNonDeleted(userId);

            return UserProfile;
        }

        public async Task<MinistryUser?> GetByStudentIDActiveNonDeleted(Guid userId)
        {
			using var scope = serviceScopeFactory.CreateScopedUow();

			var UserProfile = await scope.GetRepository<MinistryUser>().GetByIDActiveNonDeleted(userId);

            return UserProfile;
        }

        public async Task<bool> HasPermission(Guid userId, string permissionName)
        {
			using var scope = serviceScopeFactory.CreateScopedUow();

			return await scope.GetRepository<UserRole>()
                            .GetAllQueryFiltered()
                            .Include(ur => ur.Role!.RolePermission)
                            .AnyAsync(ur => ur.UserId == userId &&
                                            ur.Role!.RolePermission.Any(rp => rp.Permission!.BackendName == permissionName));
        }

        public async Task<MinistryUser?> GetStudentByIdAsync(Guid userId)
        {
			using var scope = serviceScopeFactory.CreateScopedUow();

			return await scope.GetRepository<MinistryUser>()
                .GetByIDActiveNonDeleted(userId);
        }



        public async Task<PartyType?> GetFirstUserPartyTypeAsync()
        {
			using var scope = serviceScopeFactory.CreateScopedUow();

			return await scope.GetRepository<PartyType>()
                .GetAllActiveNonDeleted()
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => userInfo.PartyTypes.Contains(pt.Id));
        }

        //public async Task<List<SelectListItemDTO>> GetMinistryUsersByModuleForScholarshipAsync(Guid moduleId, List<Guid> partyTypesIdsList)
        //{
        //	string lang = requestInfo.Lang;
        //	var result = new List<SelectListItemDTO>();
          //  using var scope = serviceScopeFactory.CreateScopedUow();

        //	if (moduleId != Guid.Empty && partyTypesIdsList != null && partyTypesIdsList.Any())
        //	{
        //		var ministryUserIdsList = await serviceScopeFactory.CreateScopedUow().GetRepository<UserPartyType>()
        //			.GetAllQueryFiltered(x => x.IsActive == true && partyTypesIdsList.Contains(x.PartyTypeId))
        //			.Select(x => x.UserId)
        //			.ToListAsync();

        //		var query = serviceScopeFactory.CreateScopedUow().GetRepository<SchAssignment>()
        //			.GetAll(x => x.IsActive == true)
        //			.Include(x => x.PartyType)
        //			.Include(x => x.MinistryUser)
        //			.Include(x => x.Scholarship)
        //			.Where(x => x.Scholarship.IsActive == true && x.Scholarship.IsDeleted == false)
        //			.Where(x => x.PartyType.SystemModuleId == moduleId)
        //			.Where(x => ministryUserIdsList.Contains(x.MinistryUserId.Value));


        //		result = (await query.Select(x => new { x.MinistryUser, x.Scholarship }).ToListAsync())
        //			.GroupBy(x => x.MinistryUser)
        //			.Select(x => new
        //			{
        //				Value = x.Key.Id,
        //				Text = lang == "ar" ? x.Key.FullNameAr : x.Key.FullNameEn,
        //				Count = x.Select(y => y.Scholarship).Distinct().Count(),
        //			})
        //			.Select(x => new SelectListItemDTO
        //			{
        //				Value = x.Value.ToString(),
        //				Text = $"{SplitAssignedUserName(x.Text)} ({x.Count})"
        //			})
        //			.ToList();
        //	}

        //	return result;
        //}
        //public async Task<List<SelectListItemDTO>> GetMinistryUsersByDepartmentForRequestsAsync(Guid moduleId, List<Guid> partyTypesIdsList, string? StatusTypeId)
        //{
        //	string lang = requestInfo.Lang;
        //	var result = new List<SelectListItemDTO>();

        //	if (moduleId != Guid.Empty && partyTypesIdsList != null && partyTypesIdsList.Any())
        //	{
        //		var ministryUserIdsList = await serviceScopeFactory.CreateScopedUow().GetRepository<UserPartyType>()
        //			.GetAllQueryFiltered(x => x.IsActive == true && partyTypesIdsList.Contains(x.PartyTypeId))
        //			.Select(x => x.UserId)
        //			.ToListAsync();

        //		var query = serviceScopeFactory.CreateScopedUow().GetRepository<EvaluationRequestAssignment>()
        //			.GetAll(x => x.IsActive == true)
        //			.Include(x => x.PartyType)
        //			.Include(x => x.MinistryUser)
        //			.Include(x => x.ServiceRequest)
        //			.Include(x => x.ServiceRequest.Status)
        //			.Where(x => x.ServiceRequest.IsActive == true && x.ServiceRequest.IsDeleted == false)
        //			.Where(x => x.PartyType.SystemModuleId == moduleId)
        //			.Where(x => ministryUserIdsList.Contains(x.MinistryUserId));

        //		if (!string.IsNullOrEmpty(StatusTypeId))
        //		{
        //			query = query.Where(x => StatusTypeId == "0" ? x.ServiceRequest.Status.IsOpen == false : x.ServiceRequest.Status.IsOpen == true);
        //		}

        //		result = (await query.Select(x => new { x.MinistryUser, x.ServiceRequest }).ToListAsync())
        //			.GroupBy(x => x.MinistryUser)
        //			.Select(x => new
        //			{
        //				Value = x.Key.Id,
        //				Text = lang == "ar" ? x.Key.NameAr : x.Key.NameEn,
        //				Count = x.Select(y => y.ServiceRequest).Distinct().Count(),
        //			})
        //			.Select(x => new SelectListItemDTO
        //			{
        //				Value = x.Value.ToString(),
        //				Text = $"{SplitAssignedUserName(x.Text)} ({x.Count})"
        //			})
        //			.ToList();
        //	}

        //	return result;
        //}

        private string SplitAssignedUserName(string assignedTo)
        {
            var output = string.Empty;
            var splitArray = assignedTo.Split(" ");
            if (splitArray.Count() >= 2)
            {
                output = $"{splitArray[0]} {splitArray[splitArray.Length - 1]}";
            }
            else
            {
                if (splitArray.Count() == 1)
                {
                    output = splitArray[0];
                }
            }

            return output;

        }


        public async Task<List<Guid>> GetEmployeeUserPartyTypeIdsAsync(Guid userProfileId, Guid departementId)
        {
			using var scope = serviceScopeFactory.CreateScopedUow();

			var employeeUserPartyTypes = await scope.GetRepository<UserPartyType>()
                .GetAllQueryFiltered(x => x.UserId == userProfileId)
                .Where(x => x.PartyType!.DepartmentId == departementId
                            && x.PartyType.IsEmployeePartyType)
                .Select(x => x.PartyType!.Id)
                .Distinct()
                .ToListAsync();

            return employeeUserPartyTypes;
        }

    }
}
