using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scholarship.DAL.Framework;
using Scholarship.DAL.Models.Masters;
using Scholarship.DAL.Models.PartyTypeEntities;
using Scholarship.DAL.Models.StatusEntities;
using Scholarship.Services.Extensions;
using Scholarship.Services.Models.Admin;
using Scholarship.Services.Models.API;
using Scholarship.Services.Special;
using Scholarship.SharedHelper.Enums;
using Scholarship.SharedHelper.Extensions;
using Scholarship.SharedHelper.Models;
using Scholarship.SharedHelper.Models.Api;
using Scholarship.SharedHelper.Models.Api.StatusDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
            
    public class SrvStatus(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo) : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
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

        #region StatusGroups
        public async Task<List<StatusGroupDTO>> GetAllStatusGroupsByServiceId(Guid serviceId, string lang)
        {
            var scopedUow = serviceScopeFactory.CreateScopedUow();
            var statusGroups = await scopedUow.GetRepository<ServiceStatusGroup>()
                .GetAllQueryFiltered(x => x.ServiceId == serviceId)
                .AsNoTracking()
                .ToListAsync();

            return statusGroups.Select(group => MapToStatusGroupDTO(group, lang)).ToList();
        }
        public async Task<List<StatusGroupDTO>> GetApplicationStatusGroup(Guid requestStatusId, Guid serviceId, string lang)
        {
            var uow = serviceScopeFactory.CreateScopedUow();
            var userPartyTypeIds = new HashSet<Guid>(userInfo.PartyTypes);

            var CurrentstatusGroups = await uow.GetRepository<ServiceStatus>()
                .GetAll()
                .FirstOrDefaultAsync(sg => sg.Id == requestStatusId);

            // 1. Try to get the full list from cache
            var data = cacheDataProvider.GetFromCache<List<ServiceStatusGroup>>(ConstantKeys.WebAppCacheTableName.CACHE_SERVICESTATUSGROUP);

            // 2. If cache miss, fetch from DB and cache the result
            if (data == null)
            {
                using var scopedUow = serviceScopeFactory.CreateScopedUow();
                var repository = scopedUow.GetRepository<ServiceStatusGroup>();

                data = await repository.GetAllQueryFiltered()
                                    .Include(sg => sg.StatusGroupPartyTypes)
                .Include(sg => sg.Service.Statuses)
                .ToListAsync();

                await cacheDataProvider.SetToCache(ConstantKeys.WebAppCacheTableName.CACHE_SERVICESTATUSGROUP, data);
            }

            var statusGroups = data
                .Where(sg => sg.ServiceId == serviceId)
                .Select(sg => new StatusGroupDTO
                {

                    Title = lang == "ar" ? sg.TitleAr : sg.TitleEn,
                    ServiceId = sg.ServiceId,
                    OrderNo = sg.OrderNo,
                    Description = lang == "ar" ? sg.DescriptionAr : sg.DescriptionEn,
                    Icon = sg.Icon,
                    ColorCode = sg.ColorCode,
                    Current = sg.Id == CurrentstatusGroups.StatusGroupId,
                    Id = sg.Id,
                    AllowedPartyTypes = sg.StatusGroupPartyTypes?.Select(pt => pt.PartyTypeId).ToList()
                })
                .ToList();

            var filteredStatusGroups = statusGroups
                                        .Where(sg => sg.Current == true
                                            || sg.AllowedPartyTypes == null || sg.AllowedPartyTypes.Count == 0
                                            || sg.AllowedPartyTypes.Intersect(userPartyTypeIds).Any())
                                        .ToList();


            await filteredStatusGroups.ParallelForEachAsync(async (sg) =>
            {
                if (sg.AllowedPartyTypes == null && sg.AllowedPartyTypes.Count == 0 && !sg.AllowedPartyTypes.Intersect(userPartyTypeIds).Any())
                {
                    sg.Title = "Under Process";
                }
            });
            return filteredStatusGroups;
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
            var resultStatus = mapper.Map<StatusDTO>(status);
            resultStatus.Name = lang.ToLower() == "ar" ? status.NameAr : status.NameEn;
            return resultStatus;
        }

        private StatusGroupDTO MapToStatusGroupDTO(ServiceStatusGroup group, string lang)
        {
            return new StatusGroupDTO
            {
                Title = lang.ToLower() == "ar" ? group.TitleAr : group.TitleEn,
                ServiceId = group.ServiceId,
                OrderNo = group.OrderNo,
                Description = lang.ToLower() == "ar" ? group.DescriptionAr : group.DescriptionEn,
                Icon = group.Icon,
                ColorCode = group.ColorCode
            };
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
