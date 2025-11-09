using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvPartyType( IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
           : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
        {

        public async Task<List<DAL.Entities.Authentication.PartyType>?> GetUserPartyTypeAsync()
        {
            var UserPartyType= await serviceScopeFactory.CreateScopedUow().GetRepository<DAL.Entities.Authentication.PartyType>()
                .GetAllActiveNonDeleted()
                .Include(pt => pt.PartyTypeCountyUniversity)
                .Include(pt => pt.PartyTypeEntityContract)
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
                .Include(x => x.PartyTypeCountyUniversity)
                .AsNoTracking()
                .ToListAsync();

            var result = new List<SelectListItemDTO>();
            var userPartyTypes = await userPartyTypesTask;
            var canViewAll = userPartyTypes!.Any(x => x.CanViewAllRequests && (x.PartyTypeCountyUniversity == null || !x.PartyTypeCountyUniversity.Any()));
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

            var userCountries = userPartyTypes!
                .Where(x => x.CanViewAllRequests && x.PartyTypeCountyUniversity != null && x.PartyTypeCountyUniversity.Any())
                .SelectMany(x => x.PartyTypeCountyUniversity!)
                .Where(c => c.CountryId != Guid.Empty)
                .Select(c => c.CountryId)
                .Distinct()
                .ToList();

            if (userCountries.Any())
            {
                result = partyTypes
                    .Where(pt => pt.PartyTypeCountyUniversity != null &&
                                 pt.PartyTypeCountyUniversity.Any(c => userCountries.Contains(c.CountryId)))
                    .Select(x => new SelectListItemDTO
                    {
                        Value = x.Id.ToString(),
                        Text = lang == "ar" ? x.NameAr : x.NameEn
                    })
                    .Distinct()
                    .ToList();
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
                                        .Where(x => !x.PartyType!.PartyTypeCountyUniversity!.Any())
                                        .AnyAsync(x => x.PartyType!.CanViewAllRequests);

            return result;
        }
        public async Task<bool> IsAllowedToViewAllScholarshipWitoutFilterationAsync(Guid? userId, Guid? ModuleId)
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
                                        .Where(x => !x.PartyType!.PartyTypeCountyUniversity!.Any())
                                        .AnyAsync(x => x.PartyType!.CanViewAllScholarships);

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
                    CanViewAllScholarships = x.PartyType.CanViewAllScholarships,
                    PartyTypeCountyUniversity = x.PartyType.PartyTypeCountyUniversity!
                        .Select(ptcu => new PartyTypeCountyUniversityDTO
                        {
                            CountryId = ptcu.CountryId,
                            UniversityId = ptcu.UniversityId
                        }).ToList(),
                    PartyTypeEntityContract = x.PartyType.PartyTypeEntityContract!
                        .Select(ptcu => new EntityContractPartyTypeDTO
                        {
                            EntityContractId = ptcu.EntityContractId,
                        }).ToList(),
                })
                .ToListAsync();
        }

    }
}
