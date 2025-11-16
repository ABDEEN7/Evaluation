using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvPartyType( IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
           : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
        {

        public async Task<List<PartyType>?> GetUserPartyTypeAsync()
        {
            var UserPartyType= await serviceScopeFactory.CreateScopedUow().GetRepository<PartyType>()
                .GetAllActiveNonDeleted()
                .AsNoTracking()
                .Where(pt => userInfo.PartyTypes.Contains(pt.Id)).ToListAsync();

            return UserPartyType;
        }

        public async Task<List<SelectListItemDTO>> GetPartyTypesByModuleAsync(Guid moduleId)
        {
            string lang = _requestInfo.Lang;

            var userPartyTypesTask =  GetUserPartyTypeAsync();

            var partyTypeRepo = serviceScopeFactory.CreateScopedUow().GetRepository<PartyType>();

          
            var partyTypes = await partyTypeRepo
                .GetAllQueryFiltered(x => x.IsEmployeePartyType && !x.CanViewAllRequests && x.SystemModuleId == moduleId)
                .AsNoTracking()
                .ToListAsync();

            var result = new List<SelectListItemDTO>();
            var userPartyTypes = await userPartyTypesTask;
            var canViewAll = userPartyTypes!.Any(x => x.CanViewAllRequests );
            if (canViewAll)
            {
                result = partyTypes
                    .Select(x => new SelectListItemDTO
                    {
                        Value = x.Id.ToString(),
                        Text = lang == "ar" ? x.NameAr : x.NameEn
                    })
                    .Distinct()
                    .ToList();

                return result;
            }

          

            return result;
        }


        public async Task<bool> IsAllowedToViewAllRequestsWitoutFilterationAsync(Guid? userId, Guid? ModuleId)
        {
            if (ModuleId is null)
            {
                return false;
            }

            var result = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<UserPartyType>()
                                        .GetAllQueryFiltered()
                                        .Include(x => x.PartyType)
                                        .Where(x => x.UserId == userId)
                                        .Where(x => x.PartyType!.SystemModuleId == ModuleId)
                                        .AnyAsync(x => x.PartyType!.CanViewAllRequests);

            return result;
        }
        public async Task<bool> IsAllowedToViewAllPlansWitoutFilterationAsync(Guid? userId, Guid? ModuleId)
        {
            if (ModuleId is null)
            {
                return false;
            }

            var result = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<UserPartyType>()
                                        .GetAllQueryFiltered()
                                        .Include(x => x.PartyType)
                                        .Where(x => x.UserId == userId)
                                        .Where(x => x.PartyType!.SystemModuleId == ModuleId)
                                        .AnyAsync(x => x.PartyType!.CanViewAllEvaluations);

            return result;
        }
        public async Task<List<UserPartyTypeDTO>> GetUserPartyTypeData(Guid? userId, Guid? moduleId)
        {
            return await serviceScopeFactory.CreateScopedUow().GetRepository<UserPartyType>()
                .GetAllQueryFiltered()
                .Where(x => x.UserId == userId && x.PartyType!.SystemModuleId == moduleId)
                .Select(x => new UserPartyTypeDTO
                {
                    PartyTypeId = x.PartyTypeId,
                    CanViewAllRequests = x.PartyType!.CanViewAllRequests,
					CanViewAllEvaluations = x.PartyType.CanViewAllEvaluations,
                })
                .ToListAsync();
        }

    }
}
