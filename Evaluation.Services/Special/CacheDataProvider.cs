using Evaluation.DAL.SystemSetting;
using Evaluation.DAL.UnitOfWork;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public class CacheDataProvider
    {
        private readonly CacheManager cacheManager;
        private readonly UnitOfWork uow;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<CacheDataProvider> logger;

        public CacheDataProvider(CacheManager cacheManager, UnitOfWork uow, 
            IServiceScopeFactory serviceScopeFactory, ILogger<CacheDataProvider> logger)
        {
            this.cacheManager = cacheManager;
            this.uow = uow;
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        // -------------------------- Utility Methods -------------------------- //

        private async Task<int> GetCacheDurationAsync()
        {
            var str = await GetSystemSettingValue(ConstantKeys.AdminSettings.ClearCacheDuration);
            return int.TryParse(str, out var hours) ? hours : 1;
        }

        private async Task<bool> IsCachingEnabledAsync()
        {
            var setting = await GetSystemSettingValue(ConstantKeys.AdminSettings.EnableCaching);
            return string.IsNullOrEmpty(setting) || bool.Parse(setting);
        }
        public async Task<string> GetSystemSettingValue(string key)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                var list = await GetSystemSettings(new List<string> { key });
                var item = list.FirstOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.SettingValue))
                {
                    result = item.SettingValue;
                }
            }
            return result;
        }
        private async Task<List<SystemSettingDTO>> GetSystemSettings(List<string> keys)
        {
            //using (var uow = serviceScopeFactory.CreateScopedUow())
            //{
            //    var result = new List<SystemSettingDTO>();

            //    if (keys != null)
            //    {
            //        var list = await uow.GetRepository<SystemSetting>()
            //            .GetAllActiveNonDeleted()
            //            .Where(x => keys.Contains(x.SettingKey))
            //            .ToListAsync();

            //        result = mapper.Map<List<SystemSettingDTO>>(list);
            //    }

            //    return result;
            //}

            return null;
        }

        private async Task<List<T>> GetOrSetCacheAsync<T>(string key, Func<Task<List<T>>> dataFetcher)
        {
            if (!await IsCachingEnabledAsync())
                return await dataFetcher();

            var data = cacheManager.GetValue<List<T>>(key);
            if (data != null)
                return data;

            var freshData = await dataFetcher();
            var hours = await GetCacheDurationAsync();
            cacheManager.SetValue(key, freshData, TimeSpan.FromHours(hours));
            return freshData;
        }

        // -------------------------- Core Methods -------------------------- //

        public async Task<string> GetExceptionMessage(string message, string lang)
        {
            if (string.IsNullOrEmpty(message)) return "{{#Default Error Message#}}";

            var pageName = "Exceptions";
            var controls = await GetUiControlsByPageNames(new List<string> { pageName }, lang);

            var control = controls.FirstOrDefault(x => x.BackEndName == message);
            if (control == null)
                return $"Missing [{message}]";

            return lang == "ar"
                ? (string.IsNullOrEmpty(control.ArValue) ? $"Missing [{message}]" : control.ArValue)
                : (string.IsNullOrEmpty(control.EnValue) ? $"Missing [{message}]" : control.EnValue);
        }

        public async Task<List<UiControlDTO>> GetUiControlsByPageNames(List<string> pageNames, string lang)
        {
            var results = new List<UiControlDTO>();

            foreach (var pageName in pageNames)
            {
                var cacheKey = $"UICONTROLS_{pageName}_{lang}";

                var list = await GetOrSetCacheAsync(cacheKey, async () =>
                {
                    var repo = serviceScopeFactory.CreateScopedUow().GetRepository<UiControl>();

                    var entities = await repo.GetAllActiveNonDeleted()
                                             .Where(x => x.PageName == pageName)
                                             .ToListAsync();

                    return entities.Select(x => new UiControlDTO
                    {
                        Id = x.Id,
                        PageName = x.PageName,
                        UserUiname = x.UserUiname,
                        ControlName = x.ControlName,
                        EnValue = x.ValueAr,
                        ArValue = x.ValueEn,
                        Url = x.Url,
                        txtValue = lang == "ar" ? x.ValueAr : x.ValueEn,
                        
                    }).ToList();
                });

                results.AddRange(list);
            }

            return results;
        }


        //public async Task<IList<ActionStatusConfiguration>> GetActionStatusConfiguration()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.CACHE_ACTIONSTATUSCONFIG;

        //    var list = await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<ActionStatusConfiguration>();
        //        var data = await repo.GetAllQueryFiltered()
        //                             .Include(c => c.Notifications)
        //                             .Include(c => c.CurrentStatus)
        //                             .Include(c => c.ServiceAction)
        //                             .ToListAsync();
        //        return data;
        //    });

        //    return list.Where(c => c.IsActive && !c.IsDeleted).ToList();
        //}



        // -------------------------------------------------------------------
        // 📨 EMAIL PROFILES
        // -------------------------------------------------------------------
        //public async Task<List<EmailProfileDTO>> GetEmailProfiles()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.EmailProfiles;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<EmailProfile>();
        //        var list = await repo.GetAllActiveNonDeleted().ToListAsync();
        //        return mapper.Map<List<EmailProfileDTO>>(list);
        //    });
        //}

        // -------------------------------------------------------------------
        // 📱 SMS PROFILES
        // -------------------------------------------------------------------
        //public async Task<List<SMSProfileDTO>> GetSMSProfiles()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.SMSProfiles;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<SMSProfile>();
        //        var list = await repo.GetAllActiveNonDeleted().ToListAsync();
        //        return mapper.Map<List<SMSProfileDTO>>(list);
        //    });
        //}

        // -------------------------------------------------------------------
        // 📧 EMAIL TEMPLATES
        // -------------------------------------------------------------------
        //public async Task<List<EmailTemplateDTO>> GetEmailTemplates()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.EmailTemplates;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<EmailTemplate>();
        //        var list = await repo.GetAllActiveNonDeleted().ToListAsync();
        //        return mapper.Map<List<EmailTemplateDTO>>(list);
        //    });
        //}

        // -------------------------------------------------------------------
        // 💬 SMS TEMPLATES
        // -------------------------------------------------------------------
        //public async Task<List<SMSTemplateDTO>> GetSMSTemplates()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.SMSTemplates;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<SMSTemplate>();
        //        var list = await repo.GetAllActiveNonDeleted().ToListAsync();
        //        return mapper.Map<List<SMSTemplateDTO>>(list);
        //    });
        //}

        // -------------------------------------------------------------------
        // ⚙️ SERVICE STATUS CONFIGURATION
        // -------------------------------------------------------------------
        //public async Task<List<SchServiceStatusConfiguration>> GetServiceStatusConfiguration()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.SchServiceStatusConfiguration;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<SchServiceStatusConfiguration>();
        //        var list = await repo.GetAllQueryFiltered()
        //                             .Include(c => c.Service)
        //                             .ToListAsync();
        //        return list;
        //    });
        //}

        // -------------------------------------------------------------------
        // 🚀 SERVICE INITIATOR PARTY TYPES
        // -------------------------------------------------------------------
        //public async Task<List<ServiceInitiatorPartyType>> GetServiceIntiator()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.ServiceInitiatorPartyType;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<ServiceInitiatorPartyType>();
        //        var list = await repo.GetAllActiveNonDeleted().ToListAsync();
        //        return list;
        //    });
        //}

        // -------------------------------------------------------------------
        // 🧩 ACTION PARTY TYPES
        // -------------------------------------------------------------------
        //public async Task<List<ActionPartyTypeDTO>> GetActionPartyTypes()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.ActionPartyTypes;
        //    return await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<ActionPartyType>();
        //        var list = await repo.GetAllActiveNonDeleted().ToListAsync();
        //        return mapper.Map<List<ActionPartyTypeDTO>>(list);
        //    });
        //}

        // -------------------------------------------------------------------
        // 🧮 SERVICE STATUS (with deleted)
        // -------------------------------------------------------------------
        //public async Task<IList<ServiceStatus>> GetStatusAllWithDeleted()
        //{
        //    var key = ConstantKeys.WebAppCacheTableName.ServiceStatus;
        //    var data = await GetOrSetCacheAsync(key, async () =>
        //    {
        //        using var scopedUow = serviceScopeFactory.CreateScopedUow();
        //        var repo = scopedUow.GetRepository<ServiceStatus>();
        //        var list = await repo.GetAll()
        //                             .Include(c => c.StatusPartyTypeDisplayNames)
        //                             .ToListAsync();
        //        return mapper.Map<List<ServiceStatus>>(list);
        //    });

        //    return data;
        //}
    }

}
