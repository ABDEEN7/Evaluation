using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.StatusEntities;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using Evaluation.SharedHelper.Models.Api.StatusDTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
            
    public class SrvStatus(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices,  UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo) :
        ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices,  userInfo, serviceProvider, requestInfo)
    {
        

        #region Status

        public async Task<StatusDTO?> GetStatusById(Guid statusId, string lang)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var status = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.Id == statusId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (status == null)
                return null;

            return MapToStatusDTO(status, lang);
        }
        public async Task<ServiceStatus?> GetStatusById(Guid statusId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var status = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.Id == statusId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return status;
        }
        public async Task<List<StatusDTO>> GetAllStatusesByServiceId(Guid serviceId, string lang)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var statuses = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.ServiceId == serviceId)
                .AsNoTracking()
                .ToListAsync();

            return statuses.Select(status => MapToStatusDTO(status, lang)).ToList();
        }

        public async Task<ServiceStatus> GetInitialStatusByServiceId(Guid serviceId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var status = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.ServiceId == serviceId && x.IsInitial)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return status ;
        }  
        public async Task<Guid?> GetInitialStatusIdByServiceId(Guid serviceId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var status = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.ServiceId == serviceId && x.IsInitial)
                .Select(x=>x.Id)
                .FirstOrDefaultAsync();

            return status ;
        }

        public async Task<StatusDTO?> GetClosedStatusByServiceId(Guid serviceId, string lang)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var status = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.ServiceId == serviceId && !x.IsOpen)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return status == null ? null : MapToStatusDTO(status, lang);
        }

        public async Task<StatusDTO?> GetOpenStatusByServiceId(Guid serviceId, string lang)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var status = await scopedUow.GetRepository<ServiceStatus>()
                .GetAllQueryFiltered(x => x.ServiceId == serviceId && x.IsOpen)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return status == null ? null : MapToStatusDTO(status, lang);
        }

        public async Task<List<SelectListItemDTO>> GetStatusesByServiceAsync(Guid serviceId, string statusType)
        {
            string lang = requestInfo.Lang;
            var result = new List<SelectListItemDTO>();

            result = await serviceScopeFactory.CreateScopedUow().GetRepository<ServiceStatus>()
                .GetAll(x => x.ServiceId == serviceId)
                .Where(x => string.IsNullOrEmpty(statusType) ? true : statusType == "0" ? x.IsOpen == false : x.IsOpen == true)
                  .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                .Select(x => new SelectListItemDTO
                {
                    Value = x.Id.ToString(),
                    TextAr = x.NameAr,
                    TextEn = x.NameEn,
                    Text = lang == "ar" ? x.NameAr : x.NameEn,
                    Parent = serviceId,
                })
                .OrderBy(x => x.Text)
                .ToListAsync();
            return result;
        }
        #endregion

        

        #region StatusDisplayName

        public async Task<StatusPartyTypeDisplayNameDTO?> GetStatusDisplayNameByPartyType(Guid statusId, Guid partyTypeId, string lang)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var displayName = await scopedUow.GetRepository<ServiceStatusPartyTypeDisplayName>()
                .GetAllQueryFiltered(x => x.StatusId == statusId && x.PartyTypeId == partyTypeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (displayName == null)
                return null;

            return MapToStatusPartyTypeDisplayNameDTO(displayName, lang);
        }

        public string GetStatusDisplayName(Guid? statusId, Guid? ModuleId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();

            string lang =requestInfo.Lang;
            var PartyTypes = userInfo.PartyTypes.ToList();
            var DepartementUserId = scopedUow.GetRepository<PartyType>()
                                    .GetAllActiveNonDeleted(x => x.SystemModuleId == ModuleId)
                                    .Select(x => x.Id)
                                    .ToList();

            var filteredPartyTypes = PartyTypes.FirstOrDefault(id => DepartementUserId.Contains(id));
            var status = cacheDataProvider.GetStatusAllWithDeleted().Result.FirstOrDefault(c => c.Id == statusId);
            var result = lang == "ar" ? status.NameAr : status.NameEn;
            var partydisplay = status.StatusPartyTypeDisplayNames.Where(x => x.IsDeleted != true)
                .FirstOrDefault(c => c.PartyTypeId == filteredPartyTypes);
            if (partydisplay != null && !string.IsNullOrEmpty(partydisplay.TitleAr)) { result = lang == "ar" ? partydisplay.TitleAr : partydisplay.TitleEn; }
            return result;
        }
        #endregion

        #region StatusPreventPartyType

        public async Task<bool> CheckStatusPreventionByPartyType(Guid statusId, Guid partyTypeId)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var prevention = await scopedUow.GetRepository<ServiceStatusPreventPartyType>()
                .GetAllQueryFiltered(x => x.StatusId == statusId && x.PartyTypeId == partyTypeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return prevention == null ? false : true;
        }

        #endregion

        #region MapDTO
        private StatusDTO MapToStatusDTO(ServiceStatus status, string lang)
        {
            var resultStatus = status.Adapt<StatusDTO>();
            resultStatus.Name = lang.ToLower() == "ar" ? status.NameAr : status.NameEn;
            return resultStatus;
        }

        private StatusPartyTypeDisplayNameDTO MapToStatusPartyTypeDisplayNameDTO(ServiceStatusPartyTypeDisplayName displayName, string lang)
        {
            return new StatusPartyTypeDisplayNameDTO
            {
                Title = lang.ToLower() == "ar" ? displayName.TitleAr : displayName.TitleEn,
                StatusId = displayName.StatusId,
                PartyTypeId = displayName.PartyTypeId
            };
        }

        private StatusPreventPartyTypeDTO MapToStatusPreventPartyTypeDTO(ServiceStatusPreventPartyType prevention)
        {
            return new StatusPreventPartyTypeDTO
            {
                StatusId = prevention.StatusId,
                PartyTypeId = prevention.PartyTypeId,
            };
        }
        #endregion


    }

}
