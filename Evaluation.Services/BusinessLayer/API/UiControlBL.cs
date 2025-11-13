using AutoMapper;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Helper;
using Evaluation.DAL.SystemSetting;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Models.API
{
    public class UiControlBL : ApiBase
    {
        public UiControlBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
        {
        }


        public string GetSetting(string Key)
        {
            using (var newuow = serviceProvider.CreateScopedUow())
            {

                var result = newuow.GetRepository<SystemSetting>()
                                       .GetAllNonDeleted()
                                       .Where(c => c.SettingKey == Key)
                                       .Select(c => c.SettingValue).FirstOrDefault();
                return result ?? "";
            }
        }
        //public string GetWebLogo()
        //{
        //    using (var newuow = serviceProvider.CreateScopedUow())
        //    {

        //        var result = newuow.GetRepository<SystemSetting>()
        //                               .GetAllNonDeleted()
        //                               .Where(c => c.SettingKey == (requestInfo.Lang=="ar"?ConstantKeys.AdminSettings.WebLogoAr:ConstantKeys.AdminSettings.WebLogoEn))
        //                               .Select(c => c.SettingValue).FirstOrDefault();
        //        return result ?? "";
        //    }
        //}

        public async Task<List<UiControlDTO>> GetUiControlsByBackendKeys(params string[] backendKeys)
        {
            var result = new List<UiControlDTO>();

            if (backendKeys != null)
            {
                var list = await uow.GetRepository<UiControl>()
                   .GetAllActiveNonDeleted()
                   .Where(x => backendKeys.Contains(x.BackendName))
                .ToListAsync();

                result = list.Adapt<List<UiControlDTO>>();
            }

            return result;
        }
        private async Task<IEnumerable<ControlValidationDTO>> GetAppConstraints(string PermissionbackendName)
        {



            var query = await uow.GetRepository<ControlValidation>()
                                        .GetAllActiveNonDeleted()
                                        .Include(x=>x.Permission)
                                        .Where(x => x.Permission.BackendName == PermissionbackendName).
                                        OrderBy(x=>x.RowOrder)
                                        .ThenBy(x=>x.ColumnOrder)
                                        .ToListAsync();
            var result = query.Adapt<List<ControlValidationDTO>>();
            return result;




        }
        public async Task<List<UiControlDTO>> LoadControlsByPage(string Pagename, string Lang)
        {
            //this.cacheService.RemoveCahe(PageName);
           
            var _UiControles = await uow.GetRepository<UiControl>().GetAllNonDeleted()
                                           .Where(x => x.PageName == Pagename)
                   .Select(c => new UiControlDTO
                   {
                       Id = c.Id,
                       BackEndName = c.BackendName,
                       PageName = c.PageName,
                       ControlName = c.ControlName,
                       ArValue = c.ValueAr!=null?c.ValueAr.Trim():string.Empty,
                       EnValue =c.ValueEn!=null? c.ValueEn.Trim():string.Empty,
                       txtValue = Lang == "ar" ? (c.ValueAr!=null?c.ValueAr.Trim():string.Empty) : (c.ValueEn!=null? c.ValueEn.Trim():string.Empty),
                       Url = c.Url
                   }).ToListAsync();
            return _UiControles;

        }
        public async Task<List<UiControlDTO>> LoadAllUiControls(params string[] PagenameList)
        {
            List<UiControlDTO> ControlsList=new List<UiControlDTO>();
            if (PagenameList != null && PagenameList.Any())
            {

                foreach (var Pagename in PagenameList)
                {
                    var ControlsListnew = new List<UiControlDTO>();
                    ControlsListnew = await LoadControlsByPage(Pagename, requestInfo.Lang);
                    ControlsList.AddRange(ControlsListnew);


                }
            }

            var uiControlsCommon = await LoadControlsByPage(AdminPages.AdminCommon, requestInfo.Lang);
            //var uiControlsCommon = await AMBL.srvUiControl.GetUiControls(ConstantKeys.AdminPages.AdminCommon, Lang);
            if (uiControlsCommon.Count > 0)
            {
                ControlsList.AddRange(uiControlsCommon);
            }
            return ControlsList;

        }
        public async Task<List<UiControlItemDTO>> LoadAllControlValidations(string[] ControlValidationPermissionBackendNameList, string[] PagenameList)
        {
            List<UiControlItemDTO> UiControlItems=new List<UiControlItemDTO>();
            var ControlsList=await LoadAllUiControls(PagenameList);
            if (ControlValidationPermissionBackendNameList != null && ControlValidationPermissionBackendNameList.Any())
            {
                foreach (var ControlValidationPermissionBackendName in ControlValidationPermissionBackendNameList)
                {
                    IEnumerable<ControlValidationDTO> Constraints = await GetAppConstraints(ControlValidationPermissionBackendName);

                    foreach (var constraint in Constraints.OrderBy(x => x.RowOrder))
                    {
                        var item = new UiControlItemDTO
                        {
                            Constraint = constraint,
                            Control = constraint == null || string.IsNullOrEmpty(constraint.UibackendName)  ? new UiControlDTO() : ControlsList.Where(x => x.BackEndName == constraint.UibackendName).FirstOrDefault()?? new UiControlDTO(),
                            Lang = requestInfo.Lang,
                            ControlName = constraint == null || string.IsNullOrEmpty(constraint.ControlName) ? "" : constraint.ControlName,
                            UibackendName = constraint == null || string.IsNullOrEmpty(constraint.UibackendName) ? "" : constraint.UibackendName,
                            RowOrder = constraint!.RowOrder,
                            ControlType = constraint == null || string.IsNullOrEmpty(constraint.ControlType) ? "" : constraint.ControlType,
                            TabulatorConfig = constraint == null || string.IsNullOrEmpty(constraint.TabulatorConfig) ? null : constraint.TabulatorConfig,


                        };

                        //if (!string.IsNullOrEmpty(item.Constraint.ControlJsonConfig))
                        //    item.Constraint.ControlJsonConfig = JsonConvert.SerializeObject(item.Constraint.ControlJsonConfig);
                        UiControlItems.Add(item);
                    }
                }
            }
            UiControlItems = UiControlItems.OrderBy(x => x.RowOrder).ToList();
            return UiControlItems;
        }
    }
}
