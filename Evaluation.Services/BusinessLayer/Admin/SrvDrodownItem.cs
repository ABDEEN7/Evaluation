using AutoMapper;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.DAL.SystemSetting;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
namespace Evaluation.Services.Models.Admin
{
    public class SrvDrodownItem : AdminBase
    {
        private readonly CacheDataProvider _CacheDataProvider;
        public SrvDrodownItem(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, CacheDataProvider CacheDataProvider) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            _CacheDataProvider = CacheDataProvider;
        }


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
                            if(Settintaskvalue!=null)
                            {
                                var jsonArray = JArray.Parse(Settintaskvalue);
                                foreach (var data in jsonArray)
                                {
                                    DropdownItem rslt = new DropdownItem();
                                    rslt.Id = (data["Id"]?.ToString()??"");
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
                        .Where(a => !a.IsDynamic)//&& a.GetName().Name=="Scholarship.DAL"
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
        return model.parentReferenceValue != null && stringValue!=null && model.parentReferenceValue.Contains(stringValue);
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
            var finalresult=_requestInfo.Lang=="ar"?result.OrderBy(x=>x.NameAr).ToList():result.OrderBy(x=>x.NameEn).ToList();
            return finalresult;
        }
        public async Task<List<DropDownTypeDTO>> GetDropDownTypeList()
        {
            var mapper = await CreateMapperForAdmin<DropDownType, DropDownTypeDTO>();



            var rslt = await uow.GetRepository<DropDownType>()
                    .GetAllNonDeleted()
                    .Where(c => string.IsNullOrEmpty(c.DataSourceTable))
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

            var result = mapper.Map<List<DropDownTypeDTO>>(rslt);
            return result;
        }

        public async Task<List<FieldDropDownValueDTO>> GetDropDownByTypeList(Guid dropdowntype)
        {
           



            var rslt = await uow.GetRepository<FieldDropDownValue>()
                    .GetAllNonDeleted()
                    .Include(x => x.dropDownType)
                    .Where(x => x.DropDownTypeId == dropdowntype)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

            var result = mapper.Map<List<FieldDropDownValueDTO>>(rslt, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;
        }
      
        public async Task<List<FieldDropDownValueDTO>> GetDropDownList(Guid? dropdowntype, int Page, int PageSize)
        {



            
            Guid specialGuid = new Guid("00000000-0000-0000-0000-000000000000");
            if (dropdowntype.Equals(specialGuid))
            {
                dropdowntype = null;
            }
            var list = await uow.GetRepository<FieldDropDownValue>()
                .GetAllNonDeleted()
                .Include(x => x.dropDownType)
                .Include(x => x.CreateBy)
                .Where(x => (dropdowntype==null? x.DropDownTypeId== x.DropDownTypeId:x.DropDownTypeId==dropdowntype))
                .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page*PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<FieldDropDownValueDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);
            return result;


        }

        public async Task<FieldDropDownValueDTO> SaveDropDown(FieldDropDownValueDTO message)
        {

           

            var BackendName = "DROPDOWN" + "_" + await GenerateBackendNameByTitle(message.TitleEn);
            var existBackendName = await uow
             .GetRepository<FieldDropDownValue>()
                  .GetAllNonDeleted(x => x.DropDownBackendName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }
            if (message.ParentDropDownId != null)
            {
                var parentexist = await uow.GetRepository<FieldDropDownValue>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.ParentDropDownId)
          .AnyAsync();
                if (!parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                }
            }
            var DropDownType = await uow.GetRepository<DropDownType>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.DropDownTypeId)
          .Select(x=>x.DataSourceTable)
          .FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(DropDownType))
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.DropDownTypeHasDataSource);
            }

            FieldDropDownValue obj = new FieldDropDownValue();

            obj.TitleAr = message.TitleAr;
            obj.TitleEn = message.TitleEn;
            obj.DropDownBackendName = BackendName;
            obj.DropDownTypeId = message.DropDownTypeId;
            obj.ParentDropDownId = message.ParentDropDownId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<FieldDropDownValue>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<FieldDropDownValueDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ResponseStatus = DBResult.Inserted;
           // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);
            return result;

        }
        public async Task<FieldDropDownValueDTO> UpdateDropDown(FieldDropDownValueDTO message)
        {



          
            var result = new FieldDropDownValueDTO();

            if (message.Id is not null)
            {


                FieldDropDownValue obj = await uow.GetRepository<FieldDropDownValue>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                if (message.ParentDropDownId == obj.Id)
                {
                    result.ResponseStatus = DBResult.SameRecord;
                    return result;
                }
                if (message.ParentDropDownId != null)
                {
                    var parentexist = await uow.GetRepository<FieldDropDownValue>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.ParentDropDownId)
          .AnyAsync();
                    if (!parentexist)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.ParentDoesNotExist);
                    }
                }
                var DropDownType = await uow.GetRepository<DropDownType>()
          .GetAllNonDeleted()
          .Where(x=>x.Id==message.DropDownTypeId)
          .Select(x=>x.DataSourceTable)
          .FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(DropDownType))
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.DropDownTypeHasDataSource);
                }
                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.DropDownBackendName = obj.DropDownBackendName;
                obj.DropDownTypeId = message.DropDownTypeId;
                obj.ParentDropDownId = message.ParentDropDownId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<FieldDropDownValue>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<FieldDropDownValueDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
               // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);
            }

            return result;

        }
        public async Task<bool> UpdateDropDownOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<FieldDropDownValue>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<FieldDropDownValueDTO> DeleteDropDown(Guid? Id)
        {



          
            var result = new FieldDropDownValueDTO();
            if (Id is not null)
            {
                FieldDropDownValue obj = await uow.GetRepository<FieldDropDownValue>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var parentexist = await uow.GetRepository<FieldDropDownValue>()
          .GetAllNonDeleted()
          .Where(x=>x.ParentDropDownId==obj.Id)
          .AnyAsync();
                if (parentexist)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.CannotDeleteItsParent);
                }
              
                uow.GetRepository<FieldDropDownValue>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<FieldDropDownValueDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
               // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_DROPDOWNVALUE);
            }
            return result;


        }

    }
}
