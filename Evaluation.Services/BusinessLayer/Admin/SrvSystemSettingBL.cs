using AutoMapper;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.SystemSetting;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvSystemSettingBL : AdminBase
    {
        public SrvSystemSettingBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }
       
        public async Task<List<SystemSettingDTO>> GetSystemSettingList(AdminSearchDTO message)
        {

            var mapper = await CreateMapperForAdmin<SystemSetting, SystemSettingDTO>();

            var list = await uow.GetRepository<SystemSetting>()
                .GetAllNonDeleted()
                 .Include(x => x.CreateBy)
                .OrderByDescending(x=>x.CreateDate)
                .ToListAsync();


            if (!string.IsNullOrEmpty(message.Title))
            {
                list = list.Where(c =>
     !string.IsNullOrEmpty(c.SettingKey) && c.SettingKey.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
     !string.IsNullOrEmpty(c.SettingValue) && c.SettingValue.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
     !string.IsNullOrEmpty(c.Description) && c.Description.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0 ||
     !string.IsNullOrEmpty(c.SettingGroup) && c.SettingGroup.IndexOf(message.Title, StringComparison.OrdinalIgnoreCase) >= 0
 ).ToList();

            }
            list = list.Skip(message.PageNum!.Value * message.PageSize!.Value).Take(message.PageSize.Value).ToList();
            var result = mapper.Map<List<SystemSettingDTO>>(list);

            return result;

        }
        public async Task<SystemSettingDTO> UpdateSystemSetting(SystemSettingDTO message)
        {



            var mapper = await CreateMapperForAdmin<SystemSetting, SystemSettingDTO>();
            var result = new SystemSettingDTO();

            if (message.Id is not null)
            {


                SystemSetting obj = await uow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();


                obj.SettingValue = message.SettingValue!;
                obj.Description = message.Description;


                uow.GetRepository<SystemSetting>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<SystemSettingDTO>(obj);
                result.UpdateBy = userInfo.DBName;
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }
        public async Task<IEnumerable<ControlValidationDTO>> GetAppConstraints(string PermissionbackendName)
        {
            
               
               
                var query = await uow.GetRepository<ControlValidation>()
                                        .GetAllActiveNonDeleted()
                                        .Include(x=>x.Permission)
                                        .Where(x => x.Permission.BackendName == PermissionbackendName).
                                        OrderBy(x=>x.RowOrder)
                                        .ThenBy(x=>x.ColumnOrder)
                                        .ToListAsync();
                var result = mapper.Map<List<ControlValidationDTO>>(query);
                return result;

           


        }
        public async Task<List<SystemSettingDTO>> GetSettings(List<string> Keys)
        {
           
                using (var newuow = serviceProvider.CreateScopedUow())
                {
                    var result = await newuow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Where(c => Keys.Contains(c.SettingKey))
                                      .Select(c => new SystemSettingDTO
                                      {
                                          Id = c.Id,
                                          SettingKey = c.SettingKey,
                                          SettingValue = c.SettingValue
                                      }).ToListAsync();
                    return result.ToList();
                }

           


        }
        public string GetSetting(string Key)
        {
            using (var newuow = serviceProvider.CreateScopedUow())
            {

                var result = newuow.GetRepository<SystemSetting>()
                                       .GetAllNonDeleted()
                                       .Where(c => c.SettingKey == Key)
                                       .Select(c => c.SettingValue).FirstOrDefault();
                return result??"";
            }
        }

        public async Task<SysLogoDTO> GetSysLogo()
        {

            var settingKeyGroup = new[]
{
    ConstantKeys.AdminSettings.AdminLogoAr,
    ConstantKeys.AdminSettings.AdminLogoEn,
    ConstantKeys.AdminSettings.WebLogoAr,
    ConstantKeys.AdminSettings.WebLogoEn,
    ConstantKeys.AdminSettings.Favicon
};

            var settings = await uow.GetRepository<SystemSetting>()
    .GetAllNonDeleted()
    .Where(x => settingKeyGroup.Contains(x.SettingKey))
    .ToListAsync();

            var result = new SysLogoDTO();

            foreach (var setting in settings)
            {
                switch (setting.SettingKey)
                {
                    case var key when key == ConstantKeys.AdminSettings.AdminLogoAr:
                        result.AdminLogoAr = setting.SettingValue;
                        break;
                    case var key when key == ConstantKeys.AdminSettings.AdminLogoEn:
                        result.AdminLogoEn = setting.SettingValue;
                        break;
                    case var key when key == ConstantKeys.AdminSettings.WebLogoAr:
                        result.WebLogoAr = setting.SettingValue;
                        break;
                    case var key when key == ConstantKeys.AdminSettings.WebLogoEn:
                        result.WebLogoEn = setting.SettingValue;
                        break;
                    case var key when key == ConstantKeys.AdminSettings.Favicon:
                        result.Favicon = setting.SettingValue;
                        break;
                }
            }

            return result;


        }
        public async Task<SysLogoDTO> SetSysLogo(SysLogoDTO message)
        {



           
            var result = new SysLogoDTO();

            if (message is not null)
            {

                if(message.AdminLogoAr!=null)
                {
                    SystemSetting obj = await uow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.SettingKey == ConstantKeys.AdminSettings.AdminLogoAr)
                                      .FirstAsync();
                    obj.SettingValue = message.AdminLogoAr;
                    uow.GetRepository<SystemSetting>().Update(obj);
                }
                if (message.AdminLogoEn != null)
                {
                    SystemSetting obj = await uow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.SettingKey == ConstantKeys.AdminSettings.AdminLogoEn)
                                      .FirstAsync();
                    obj.SettingValue = message.AdminLogoEn;
                    uow.GetRepository<SystemSetting>().Update(obj);
                }
                if (message.WebLogoAr != null)
                {
                    SystemSetting obj = await uow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.SettingKey == ConstantKeys.AdminSettings.WebLogoAr)
                                      .FirstAsync();
                    obj.SettingValue = message.WebLogoAr;
                    uow.GetRepository<SystemSetting>().Update(obj);
                }
                if (message.WebLogoEn != null)
                {
                    SystemSetting obj = await uow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.SettingKey == ConstantKeys.AdminSettings.WebLogoEn)
                                      .FirstAsync();
                    obj.SettingValue = message.WebLogoEn;
                    uow.GetRepository<SystemSetting>().Update(obj);
                }
                if (message.Favicon != null)
                {
                    SystemSetting obj = await uow.GetRepository<SystemSetting>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.SettingKey == ConstantKeys.AdminSettings.Favicon)
                                      .FirstAsync();
                    obj.SettingValue = message.Favicon;
                    uow.GetRepository<SystemSetting>().Update(obj);
                }

                
                await uow.CommitAsync();
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

    }
}
