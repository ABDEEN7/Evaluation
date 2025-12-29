using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
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
        //    using (var newuow = serviceProvider.CreateScopeduow())
        //    {

        //        var result = newuow.GetRepository<SystemSetting>()
        //                               .GetAllNonDeleted()
        //                               .Where(c => c.SettingKey == (requestInfo.Lang=="ar"?ConstantKeys.AdminSettings.WebLogoAr:ConstantKeys.AdminSettings.WebLogoEn))
        //                               .Select(c => c.SettingValue).FirstOrDefault();
        //        return result ?? "";
        //    }
        //}
        public async Task<List<DropdownItem>> GetDropDownValues(DropDownValuesRequestDTO model)
        {
            var result = new List<DropdownItem>();
            try
            {


                var controlValidationItem = await uow.GetRepository<ControlValidation>()
                .GetAllNonDeleted()
                .Where(x => x.UibackendName == model.controlUibackendName)
                .FirstOrDefaultAsync();

                if (controlValidationItem == null)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
                }

                if (!string.IsNullOrEmpty(controlValidationItem.ControlJsonConfig))
                {
                    var dropdownJsonConfig = JsonConvert.DeserializeObject<DropdownJsonConfig>(controlValidationItem.ControlJsonConfig);

                    if (dropdownJsonConfig != null)
                    {
                        // Dynamically find the dropdown data from settings
                        Type objType = typeof(ConstantKeys.AdminSettings);
                        bool exists = objType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                             .Any(f => f.Name == dropdownJsonConfig.TableNameSource);

                        // Dynamically find the dropdown data from custom datasource
                        Type CustomDataSourceType = typeof(ConstantKeys.CustomDataSource);
                        bool CustomDataSourceexists = CustomDataSourceType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                             .Any(f => f.Name == dropdownJsonConfig.TableNameSource);


                        if (exists)
                        {
                            var Settintaskvalue = await uow.GetRepository<SystemSetting>().GetAllNonDeleted()
                                .Where(x => x.SettingKey == dropdownJsonConfig.TableNameSource)
                                .Select(x => x.SettingValue).FirstOrDefaultAsync();
                            if (Settintaskvalue != null)
                            {
                                var jsonArray = JArray.Parse(Settintaskvalue);
                                foreach (var data in jsonArray)
                                {
                                    DropdownItem rslt = new DropdownItem();
                                    rslt.Id = (data["Id"]?.ToString() ?? "");
                                    rslt.NameAr = (data["TitleAr"]?.ToString() ?? "");
                                    rslt.NameEn = (data["TitleEn"]?.ToString() ?? "");
                                    result.Add(rslt);
                                }
                            }



                        }
                        else if (CustomDataSourceexists)
                        {

                            switch (dropdownJsonConfig.TableNameSource)
                            {

                                default:
                                    // Fallback if none match
                                    break;
                            }


                        }
                        else
                        {
                            // Dynamically find the entity type
                            Type? entityType = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a => !a.IsDynamic&& a.GetName().Name=="Evaluation.DAL")
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.Name.Equals(dropdownJsonConfig.TableNameSource, StringComparison.OrdinalIgnoreCase));

                            if (entityType != null)
                            {
                                // Get the generic repository method
                                MethodInfo? getRepositoryMethod = typeof(UnitOfWork).GetMethod("GetRepository");
                                if (getRepositoryMethod != null)
                                {
                                    // Make the method generic with the dynamically found entity type
                                    MethodInfo genericMethod = getRepositoryMethod.MakeGenericMethod(entityType);
                                    object? repository = genericMethod.Invoke(uow, null);

                                    if (repository != null)
                                    {
                                        // Ensure the repository implements IGenericRepository<T>
                                        Type repoType = typeof(Repository<>).MakeGenericType(entityType);
                                        if (repoType.IsInstanceOfType(repository))
                                        {
                                            var getAllNonDeletedMethod = repoType.GetMethod("GetAllNonDeleted");

                                            if (getAllNonDeletedMethod != null)
                                            {
                                                // Invoke GetAllNonDeleted and pass the necessary parameters (using null for optional params)
                                                var queryableResult = (IQueryable<object>)getAllNonDeletedMethod.Invoke(repository, new object[] { null, null, null, null, null });

                                                if (queryableResult != null)
                                                {
                                                    // ToListAsync can be used to execute the query and get the result as a List
                                                    var items = await queryableResult
                                                .ToListAsync();

                                                    // Apply filtering based on ParentReferenceId if provided
                                                    if (!string.IsNullOrEmpty(dropdownJsonConfig.ParentReferenceId) && model.parentReferenceValue != null && model.parentReferenceValue.Any())
                                                    {
                                                        items = items
    .Where(item =>
    {
        var property = item.GetType().GetProperty(dropdownJsonConfig.ParentReferenceId);
        if (property == null) return false;

        var value = property.GetValue(item);
        if (value == null) return false;

        var stringValue = value.ToString();
        return model.parentReferenceValue != null && stringValue != null && model.parentReferenceValue.Contains(stringValue);
    })
    .ToList();
                                                    }

                                                    if (!string.IsNullOrEmpty(dropdownJsonConfig.IsActive))
                                                    {
                                                        items = items.Where(item => item.GetType().GetProperty("IsActive")?.GetValue(item)?.ToString()?.ToLower() == dropdownJsonConfig.IsActive.ToLower()).ToList();
                                                    }

                                                    // Map the items to DropdownItem
                                                    result = items.Select(item => new DropdownItem
                                                    {
                                                        Id = item.GetType().GetProperty(dropdownJsonConfig.IdName)?.GetValue(item)?.ToString() ?? string.Empty,
                                                        NameAr = item.GetType().GetProperty(dropdownJsonConfig.DisplayNameAr)?.GetValue(item)?.ToString() ?? string.Empty,
                                                        NameEn = item.GetType().GetProperty(dropdownJsonConfig.DisplayNameEn)?.GetValue(item)?.ToString() ?? string.Empty,
                                                        Type = dropdownJsonConfig.TableNameSource,
                                                        OrderNo = Convert.ToInt32(item.GetType().GetProperty("OrderNo")?.GetValue(item) ?? 0)
                                                    }).OrderBy(x => x.OrderNo).ToList();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            var finalresult=requestInfo.Lang=="ar"?result.OrderBy(x=>x.NameAr).ToList():result.OrderBy(x=>x.NameEn).ToList();
            return finalresult;
        }
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
