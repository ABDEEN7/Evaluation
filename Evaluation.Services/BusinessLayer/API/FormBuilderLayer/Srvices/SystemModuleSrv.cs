using AutoMapper;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.SystemModulesEntities;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class  SystemModuleSrv(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
        {
        

        public async Task<SystemModule> GetSystemModuleByRoutingAsync(string routing)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();

            var SystemModule = await scopedUow.GetRepository<SystemModule>()
                .GetAllQueryFiltered()
                .FirstOrDefaultAsync(c => c.Routing == routing);

            return SystemModule!;
        }

        public async Task<SystemModuleDTO> GetSystemModuleDTOByIdAsync(Guid SystemModuleId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();

            var SystemModule = await scopedUow.GetRepository<SystemModule>()
                .GetAllQueryFiltered()
                .Include(c => c.Services) // Including related services
                .FirstOrDefaultAsync(c => c.Id == SystemModuleId);

            return SystemModule != null ? MapToSystemModuleDTO(SystemModule) : null!;
        }
        public async Task<SystemModule> GetSystemModuleByIdAsync(Guid SystemModuleId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();

            var SystemModule = await scopedUow.GetRepository<SystemModule>()
                .GetAllQueryFiltered()
                .Include(c => c.Services)
                .FirstOrDefaultAsync(c => c.Id == SystemModuleId);

            return SystemModule!;
        }
        private SystemModuleDTO MapToSystemModuleDTO(SystemModule SystemModule)
        {
            return new SystemModuleDTO
            {
                Id = SystemModule.Id,
                Name = SystemModule.NameAr,
                Routing = SystemModule.Routing,
            };
        }


        public async Task<List<SystemModuleDTO>?> GetSystemModuleList(Guid? userIdClaim = null)
        {
            if (userIdClaim is null)
            {
                return null;
            }

            string Lang = _requestInfo.Lang;
            using var Uow = serviceScopeFactory.CreateScopedUow();

            var routingList = await Uow.GetRepository<UserPartyType>()
                                       .GetAll(x => x.UserId == userIdClaim && x.PartyType!.IsEmployeePartyType)
                                       .Include(x => x.PartyType!.SystemModule)
                                       .Select(x => x.PartyType!.SystemModule!.Routing.ToLower())
                                       .Distinct()
                                       .ToListAsync();

            if (routingList == null || !routingList.Any())
            {
                return new List<SystemModuleDTO>();
            }

            var result = await Uow.GetRepository<SystemModule>()
                                  .GetAll()
                                  .Where(c => c.IsDeleted == false && c.IsActive == true && routingList.Contains(c.Routing.ToLower()))
                                  .OrderBy(c => c.OrderNo)
                                  .Select(c => new SystemModuleDTO
                                  {
                                      Id = c.Id,
                                      Name = Lang == "ar" ? c.NameAr : c.NameEn,
                                      Description = Lang == "ar" ? c.DescriptionAr : c.DescriptionEn,
                                      Icon = c.Icon,
                                      Routing = c.Routing
                                  })
                                  .ToListAsync();

            return result;
        }

    }
}
