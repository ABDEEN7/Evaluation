using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
namespace Evaluation.Services.Models.Admin
{
    public class SrvFormGroupBL : AdminBase
    {
        private readonly CacheDataProvider _CacheDataProvider;
        public SrvFormGroupBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, CacheDataProvider CacheDataProvider) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            _CacheDataProvider = CacheDataProvider;
        }
        public async Task<List<FormGroup>> GetFormGroupList(Guid serviceId,string FormGrouptype)
        {
            //var mapper = await CreateMapperForAdmin<FormGroup, FormGroupDTO>();
            var result = await uow.GetRepository<FormGroup>()
                .GetAllNonDeleted()
                .Include(x=>x.FormGroupType)
                .Include(x=>x.FormGroupCustomList)
                .Where(x=>x.ServiceId==serviceId && x.FormGroupType!.BackendName==FormGrouptype)
                .OrderBy(x=>x.Order)
                .ToListAsync();

            //var result = mapper.Map<List<FormGroupDTO>>(list);

            return result;


        }
        public async Task<bool> UpdateFormGroupOrder(List<FormGroupDTO> message, Guid serviceId)
        {
            bool rtn = false;
            try
            {
                

                var updatedRows = from updatedItem in message
                                  join rowToUpdate in uow.GetRepository<FormGroup>().GetAllNonDeleted().Where(x => x.ServiceId == serviceId) on updatedItem.Id equals rowToUpdate
                    .Id
                                  select new { Row = rowToUpdate, updatedItem.Order };



                updatedRows.ToList().ForEach(x => x.Row.Order = x.Order);

                await uow.CommitAsync();
               
                
                rtn = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return rtn;
        }

        public async Task<List<Field>> GetAllSystemField(Guid systemmoduleid)
        {
            var insitalservice=await uow.GetRepository<Service>().GetAllActiveNonDeleted().Where(x=>x.SystemModuleId==systemmoduleid && x.Initialservice==true).Select(x=>x.Id).FirstOrDefaultAsync();

            var result = await uow.GetRepository<Field>()
                    .GetAllActiveNonDeleted()
                    .Where(x=>x.ServiceId==insitalservice)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
           

            return result;


        }
        public async Task<List<FormGroupDTO>> GetFormGroupListByTypeList(Guid serviceid)
        {

           // var mapper = await CreateMapperForAdmin<FormGroup, FormGroupDTO>();
            var result = await uow.GetRepository<FormGroup>()
                .GetAllNonDeleted()
                .Include(x=>x.FormGroupType)
                .Where(x=>x.FormGroupType!.BackendName=="List" && x.ServiceId==serviceid)
                .OrderBy(x=>x.Order)
                .ThenByDescending(x=>x.CreateDate)
                .Select(x=>new FormGroupDTO
                {
                    Id=x.Id,
                    TitleAr=x.TitleAr,
                    TitleEn=x.TitleEn,
                    FormGroupCustomListId=x.FormGroupCustomListId
                }).ToListAsync();

           // var result = mapper.Map<List<FormGroupDTO>>(list);

            return result;

        }
        public async Task<List<FieldDTO>> GetAllFormGroupFields(Guid FormGroupId)
        {
            var fields = await uow.GetRepository<Field>()
    .GetAllNonDeleted()
    .Include(x => x.FieldType)
    .Include(x => x.FormGroup)
    .Include(x => x.CreateBy)
    .Where(x => x.FormGroupId == FormGroupId)
    .OrderByDescending(x => x.CreateDate)
    .ToListAsync();

            var fieldPartyTypes = await uow.GetRepository<FieldPartyType>()
    .GetAllNonDeleted()
    .Where(fp => fields.Select(f => f.Id).Contains(fp.FieldId))
    .GroupBy(fp => fp.FieldId)
    .ToDictionaryAsync(g => g.Key, g => g.Select(fp => fp.PartyTypeId).ToArray());

           



            var list =  fields
                 .AsEnumerable()
                .Select(g => new FieldDTO
                {
                    BackendName=g.BackendName,
                    FormGroupId=g.FormGroupId,
                    Id = g.Id,
                    TitleAr = g.TitleAr,
                    TitleEn = g.TitleEn,
                    FieldTypeId=g.FieldTypeId,
                    FieldType= _requestInfo.Lang == "ar" ? g.FieldType!.NameAr : g.FieldType!.NameEn,
                    Type = g.FieldType.BackendName,
                    Title = _requestInfo.Lang == "ar" ? g.TitleAr : g.TitleEn,
                    Column = g.Column,
                    Row = g.Row,
                    IsActive=g.IsActive,
                    ReadFieldId=g.ReadFieldId,
                    InfoAr=g.InfoAr,
                    InfoEn=g.InfoEn,
                    ClassName=g.ClassName,
                    Description=g.Description,
                    MappingFieldId=g.MappingFieldId,
                    EvalFormId=g.EvalFormId,
                    DropDownParentFieldId=g.DropDownParentFieldId,
                    DropDownTypeId=g.DropDownTypeId,
                    UpdateBy=(g.UpdateBy != null?(_requestInfo.Lang == "ar" ?g.UpdateBy.NameAr: g.UpdateBy.NameEn):(_requestInfo.Lang == "ar" ?g.CreateBy!.NameAr: g.CreateBy!.NameEn)),
                    UpdateDate=(g.UpdateDate.HasValue ? g.UpdateDate.Value.ToString() : g.CreateDate.ToString()),
                    FieldPartyTypes = fieldPartyTypes.ContainsKey(g.Id)
            ? fieldPartyTypes[g.Id]
            : Array.Empty<Guid>(),
                    FormGroupCustomListId=g.FormGroupCustomListId,
                    ServiceId=g.ServiceId,
                    FormGroupListId=g.FormGroupListId,
                    FormGroupName=_requestInfo.Lang == "ar" ? g.FormGroup!.TitleAr:g.FormGroup!.TitleEn
                }). ToList();
            return list.OrderBy(x=>x.Row).ThenBy(x=>x.Column).ToList();

        }

        public async Task<List<DropdownItem>> GetAttributeList()
        {

            var list = await uow.GetRepository<DAL.Models.FormBuilder.Attribute>()
                                .GetAllNonDeleted()
                                .OrderByDescending(x=>x.CreateDate)
                                .Select(x=>new DropdownItem
                                {
                                    Id=x.Id,
                                    NameAr=x.Key,
                                    NameEn=x.Key
                                })
                                .ToListAsync();
            DropdownItem item=new DropdownItem();
            item.Id = Guid.Empty;
            item.NameAr = "Others";
            item.NameEn = "Others";
            list.Add(item);

            return list;




        }
        public async Task<List<FieldAttributeValueDTO>> GetFieldAttributeValueList(Guid FieldId)
        {
           
            var list = await uow.GetRepository<FieldAttributeValue>()
                                .GetAllNonDeleted()
                                .Include(x=>x.Field)
                                .Where(x=>x.FieldId==FieldId)
                                .OrderByDescending(x=>x.CreateDate)
                                .Select(x=>new FieldAttributeValueDTO
                                {
                                    Id=x.Id,
                                    AttributeKey=x.AttributeKey,
                                    AttributeValue=x.AttributeValue,
                                    MessageAr=x.MessageAr,
                                    MessageEn=x.MessageEn,
                                    Description=x.Description??string.Empty,
                                    IsActive=x.IsActive,
                                    UpdateBy=x.UpdateBy!=null?(_requestInfo.Lang=="ar"?x.UpdateBy.NameAr:x.UpdateBy.NameEn):(_requestInfo.Lang=="ar"?x.CreateBy!.NameAr:x.CreateBy!.NameEn),
                                    UpdateDate=x.UpdateDate!=null?x.UpdateDate.ToString():x.CreateDate.ToString(),
                                })
                                .ToListAsync();

            

            return list;

       


        }
        public async Task<List<FieldViewConditionDTO>> GetAllFieldConditionList(Guid FieldId)
        {
            var isArabic = _requestInfo.Lang == "ar";
            var list = await uow.GetRepository<FieldViewCondition>()
                                .GetAllNonDeleted()
                                .Include(x=>x.Field)
                                .Where(x=>x.FieldId==FieldId)
                                .OrderByDescending(x=>x.CreateDate)
                                .Select(x=>new FieldViewConditionDTO
                                {
                                    Id = x.Id,
                                    FieldId = x.FieldId   ,
                                    ParentFieldId = x.ParentFieldId,
                                    ParentField=x.ParentField!=null?(_requestInfo.Lang=="ar"?x.ParentField.TitleAr:x.ParentField.TitleEn):string.Empty,
                                    FieldDropDownValueIds = x.FieldDropDownValueIds != null
    ? x.FieldDropDownValueIds.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
    : new string[0],
                                    FieldValue = x.FieldValue,
                                    FieldValueDisplay=GetFieldValue(x.FieldValue, x.ParentFieldId, isArabic,serviceProvider),
                                    IsSufficient = x.IsSufficient,
                                    operators = x.operators,
                                    IsActive=x.IsActive,
                                    UpdateBy=x.UpdateBy!=null?(_requestInfo.Lang=="ar"?x.UpdateBy.NameAr:x.UpdateBy.NameEn):(_requestInfo.Lang=="ar"?x.CreateBy!.NameAr:x.CreateBy!.NameEn),
                                    UpdateDate=x.UpdateDate!=null?x.UpdateDate.ToString():x.CreateDate.ToString(),
                                })
                                .ToListAsync();



            return list;




        }
        public static string GetFieldValue(
     
      string fieldValue,
      Guid? refId,
      bool isArabic,
      IServiceProvider serviceProvider)
        {
            var uow=serviceProvider.CreateScopedUow();
            if (refId == null) return fieldValue;

            var fieldvaluelist=fieldValue.Split(",");

           

            var field = uow.GetRepository<Field>()
        .GetAll(x => x.Id == refId)
        .Include(x => x.FieldType)
        .FirstOrDefault();

            if(field!=null)
            {
                if (field?.FieldType!.BackendName is "select2" or "dropdown" or "VacancySeat")
                {

                    var dataSource = uow.GetRepository<DropDownType>()
            .GetAllNonDeleted()
            .Where(x => x.Id == field.DropDownTypeId)
            .Select(x => x.DataSourceTable)
            .FirstOrDefault();

                    if (!string.IsNullOrEmpty(dataSource))
                        return GetTitleFromTable(dataSource, isArabic, serviceProvider, fieldvaluelist);

                    var result= uow.GetRepository<FieldDropDownValue>()
                    .GetAllNonDeleted()
                    .Where(x => x.DropDownTypeId == field.DropDownTypeId && fieldvaluelist.Contains(x.Id.ToString()))
                    .Select(c => LangSelector(isArabic,c.TitleAr, c.TitleEn))
                    .ToListAsync();
                    return string.Join(", ", result.Result);
                }
            }

           

            return fieldValue;
        }
        private static string LangSelector(bool isArabic, string ar, string en) => isArabic ? ar : en;
        private static string GetTitleFromTable(string tableName, bool isArabic, IServiceProvider serviceProvider, string[] fieldvaluelist)

        {

            var uow=serviceProvider.CreateScopedUow();

            var finalresult="";

            switch (tableName)
            {
                


                

                case "AcademicYear" or "CurrentAcademicYear":
                   var result = uow.GetRepository<AcademicYear>()
                 .GetAllNonDeleted()
                 .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
                    .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
                    finalresult = string.Join(", ", result.Result);
                    break;

                

               

                case "DropDownTypes":
                    result = uow.GetRepository<DropDownType>()
                 .GetAllNonDeleted()
                .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
                    .Select(c => isArabic ? c.TitleAr : c.TitleEn).ToListAsync();
                    finalresult = string.Join(", ", result.Result);
                    break;
               
               
                case "Semester":
                    result = uow.GetRepository<Semester>()
                 .GetAllNonDeleted()
                 .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
                    .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
                    finalresult = string.Join(", ", result.Result);
                    break;
                case "UserGender":
                    result = uow.GetRepository<UserGender>()
                 .GetAllNonDeleted()
                .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
                    .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
                    finalresult = string.Join(", ", result.Result);
                    break;

            }

            return finalresult;
        }
        public async Task<List<FormGroupTypeDTO>> GetAllFormGroupType()
        {
            var mapper = await CreateMapperForAdmin<FormGroupType, FormGroupTypeDTO>();


            var rslt = await uow.GetRepository<FormGroupType>()
                    .GetAllNonDeleted()
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            var result = mapper.Map<List<FormGroupTypeDTO>>(rslt);
          
            return result;


        }
        public async Task<List<FieldTypeDTO>> GetAllFieldType()
        {
            var mapper = await CreateMapperForAdmin<FieldType, FieldTypeDTO>();


            var rslt = await uow.GetRepository<FieldType>()
                    .GetAllNonDeleted()
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            var result = mapper.Map<List<FieldTypeDTO>>(rslt);

            return result;


        }
        public async Task<List<PartyTypeDTO>> GetAllPartyType(Guid systemmoduleid)
        {
            var mapper = await CreateMapperForAdmin<PartyType, PartyTypeDTO>();


            var rslt = await uow.GetRepository<PartyType>()
                    .GetAllNonDeleted()
                    .Where(x=>x.SystemModuleId==systemmoduleid)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            var result = mapper.Map<List<PartyTypeDTO>>(rslt);

            return result;


        }
        public async Task<List<DropDownTypeDTO>> GetAllDropDownType()
        {
            var mapper = await CreateMapperForAdmin<DropDownType, DropDownTypeDTO>();


            var rslt = await uow.GetRepository<DropDownType>()
                    .GetAllNonDeleted()
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            var result = mapper.Map<List<DropDownTypeDTO>>(rslt);

            return result;


        }
        public async Task<List<Field>> GetAllParentDropDownField(Guid serviceid)
        {
            //var mapper = await CreateMapperForAdmin<Field, FieldDTO>();


            var result = await uow.GetRepository<Field>()
                    .GetAllNonDeleted()
                    .Include(x=>x.FieldType)
                    .Include(x=>x.FormGroup)
                    .Where(x=>x.ServiceId==serviceid && (x.FieldType!.NameEn=="dropdown"|| x.FieldType.NameEn=="select2"))
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();
            //var result = mapper.Map<List<FieldDTO>>(rslt);

            return result;


        }
        public async Task<List<FieldDTO>> GetAllField(Guid serviceid)
        {
            


            var rslt = await uow.GetRepository<Field>()
                    .GetAllNonDeleted()
                    .Include(x=>x.FieldType)
                    .Include(x=>x.FormGroup)
                    .Include(x=>x.FormGroup!.FormGroupType)
                    .Where(x=>x.ServiceId==serviceid && x.FormGroup!.FormGroupType!.BackendName=="FormGroup")
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x=>new FieldDTO
                    {
                        Id=x.Id,
                        TitleAr=x.FormGroup!.TitleAr+" _ "+x.TitleAr,
                        TitleEn=x.FormGroup.TitleEn+" _ "+x.TitleEn,
                        //Type=_requestInfo.Lang=="ar"?x.FieldType!.NameAr:x.FieldType!.NameEn,
                        Type=x.FieldType!.BackendName,
                        DropDownTypeId=x.DropDownTypeId
                    })
                    .ToListAsync();
           

            return rslt;


        }

        public async Task<List<DropdownItem>> GetAllEvalForm(Guid systemmoduleid)
        {
            var departmentid=await uow.GetRepository<SystemModule>().GetAllNonDeleted().Where(x=>x.Id==systemmoduleid).Select(x=>x.DepartmentId).FirstOrDefaultAsync();


            var rslt = await uow.GetRepository<EvalForm>()
                    .GetAllNonDeleted()
                    .Include(x=>x.FormEvalMatrix)
                    .Where(x=>x.FormEvalMatrix!.DepartmentId==departmentid)
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x=>new DropdownItem
                    {
                        Id=x.Id,
                        NameAr=x.NameAr,
                        NameEn=x.NameEn
                    })
                    .ToListAsync();


            return rslt;


        }

        public async Task<List<FieldDTO>> GetAllFieldFormGroupList(Guid serviceid)
        {



            var rslt = await uow.GetRepository<Field>()
                    .GetAllNonDeleted()
                    .Include(x=>x.FieldType)
                    .Include(x=>x.FormGroup)
                    .Include(x=>x.FormGroup!.FormGroupType)
                    .Where(x=>x.ServiceId==serviceid && x.FormGroup!.FormGroupType!.BackendName=="List")
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x=>new FieldDTO
                    {
                        Id=x.Id,
                        TitleAr=x.FormGroup!.TitleAr+" _ "+x.TitleAr,
                        TitleEn=x.FormGroup.TitleEn+" _ "+x.TitleEn,
                        Type=_requestInfo.Lang=="ar"?x.FieldType!.NameAr:x.FieldType!.NameEn,
                        DropDownTypeId=x.DropDownTypeId
                    })
                    .ToListAsync();


            return rslt;


        }
        public async Task<List<FieldDropDownValueDTO>> GetFieldDropDownValueList(Guid DropDownTypeId)
        {
            var result= new List<FieldDropDownValueDTO>();
            var hasdatasource=await uow.GetRepository<DropDownType>()
                                 .GetAllNonDeleted()
                                 .Where(x=>x.Id==DropDownTypeId)
                                 .Select(x=>x.DataSourceTable)
                                .FirstOrDefaultAsync();
            if(!string.IsNullOrEmpty(hasdatasource))
            {
                 result = await GetDataFromTable(hasdatasource);

            }
            else
            {
                result = await uow.GetRepository<FieldDropDownValue>()
                    .GetAllNonDeleted(x=>x.DropDownTypeId== DropDownTypeId)
                    .OrderByDescending(x => x.CreateDate)
                    .Select(c => new FieldDropDownValueDTO
                    {
                        Id = c.Id,
                        TitleAr = c.TitleAr,
                        TitleEn = c.TitleEn,
                    }).ToListAsync();

            }

            
                return  result.ToList();

           



        }
        private async Task<List<FieldDropDownValueDTO>> GetDataFromTable(string tableName)
        {


       
            var result=new List<FieldDropDownValueDTO>();

            switch (tableName)
            {
                

                case "AcademicYear" :
                    result = await uow.GetRepository<AcademicYear>()
                 .GetAllNonDeleted()
                 .Select(c => new FieldDropDownValueDTO
                 {
                     Id = c.Id,
                     TitleAr = c.NameAr,
                     TitleEn = c.NameEn
                 }).ToListAsync();
                    break;

                case "CurrentAcademicYear":
                    result = await uow.GetRepository<AcademicYear>()
                 .GetAllNonDeleted()
                 .Where(x=>x.IsCurrent==true)
                 .Select(c => new FieldDropDownValueDTO
                 {
                     Id = c.Id,
                     TitleAr = c.NameAr,
                     TitleEn = c.NameEn
                 }).ToListAsync();
                    break;

               

                case "DropDownTypes":
                    result = await uow.GetRepository<DropDownType>()
                 .GetAllNonDeleted()
                 .Select(c => new FieldDropDownValueDTO
                 {
                     Id = c.Id,
                     TitleAr = c.TitleAr,
                     TitleEn = c.TitleEn
                 }).ToListAsync();
                    break;
                
                case "Semester":
                    result = await uow.GetRepository<Semester>()
                 .GetAllNonDeleted()
                 .Select(c => new FieldDropDownValueDTO
                 {
                     Id = c.Id,
                     TitleAr = c.NameAr,
                     TitleEn = c.NameEn
                 }).ToListAsync();
                    break;
                case "UserGender":
                    result = await uow.GetRepository<UserGender>()
                 .GetAllNonDeleted()
                 .Select(c => new FieldDropDownValueDTO
                 {
                     Id = c.Id,
                     TitleAr = c.NameAr,
                     TitleEn = c.NameEn
                 }).ToListAsync();
                    break;
            
            }

            return result;
        }
        public async Task<FormGroupDTO> SaveFormGroup(FormGroupDTO message)
        {
            var BackendName = await GenerateBackendName(message.TitleEn, message.ServiceId, "FG");
            var existBackendName = await uow
             .GetRepository<FormGroup>()
                  .GetAllNonDeleted(x => x.BackendName == BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.Exist;
                return message;
            }
            var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == message.ServiceId && x.IsFreez == true).ToListAsync();

            if (servicefreezecount.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);
            }
            FormGroup obj = new FormGroup();

                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.BackendName = BackendName;
                obj.FormGroupTypeId = message.FormGroupTypeId;
                obj.FormGroupCustomListId = message.FormGroupCustomListId;
                obj.ServiceId = message.ServiceId;
                obj.IsActive = message.IsActive;
                uow.GetRepository<FormGroup>().Insert(obj);

            if(message.FormGroupCustomListId is not null)
            {
                FormGroupCustomList? FormGroupCustomList=await uow.GetRepository<FormGroupCustomList>().GetAllNonDeleted()
                    .Where(x=>x.Id==message.FormGroupCustomListId)
                    .FirstOrDefaultAsync();
                if(FormGroupCustomList!=null)
                {
                   
                    if(!string.IsNullOrEmpty(FormGroupCustomList.Schema))
                    {
                        List<Field> objfieldlist = new List<Field>();
                        var jsonArray = JArray.Parse(FormGroupCustomList.Schema);
                        foreach (var data in jsonArray)
                        {
                            Field objfield = new Field();
                            objfield.FormGroupId = obj.Id;
                            objfield.ServiceId = obj.ServiceId;
                            FieldType? FieldTypeId=await uow.GetRepository<FieldType>().GetAllNonDeleted()
                                .Where(x=>x.BackendName==(string)data["FieldType"]!)
                                .FirstOrDefaultAsync();
                            objfield.FieldTypeId = FieldTypeId!.Id;
                            objfield.FormGroupCustomListId = obj.FormGroupCustomListId;
                            objfield.TitleAr = (string)data["TitleAr"]!;
                            objfield.TitleEn = (string)data["TitleEn"]!;
                            objfield.BackendName = (string)data["BackendName"]!;
                            objfield.Row = (int)data["Row"]!;
                            objfield.Column = (int)data["Column"]!;
                            objfieldlist.Add(objfield);
                        }
                        if (objfieldlist.Count > 0)
                        {
                            await uow.GetRepository<Field>().InsertRange(objfieldlist);
                        }
                    }
                    

                }
            }
                await uow.CommitAsync();
                
                message.Id = obj.Id;
            message.CreateBy = userInfo.Name;
            message.ResponseStatus = DBResult.Inserted;

                return message;
            
        }
        public async Task<FormGroupDTO> UpdateFormGroup(FormGroupDTO message)
        {

           // var mapper = await CreateMapperForAdmin<FormGroup, FormGroupDTO>();
            var result =new FormGroupDTO() ;
         
                if (message.Id is not null)
                {
                    FormGroup obj = await uow.GetRepository<FormGroup>()
                                      .GetAllNonDeleted()
                                      .Include(x=>x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();

                if (servicefreezecount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);
                }
                obj.TitleAr = message.TitleAr;
                    obj.TitleEn = message.TitleEn;
                obj.BackendName = obj.BackendName;
                obj.FormGroupTypeId = message.FormGroupTypeId;
                obj.FormGroupCustomListId = obj.FormGroupCustomListId;
                obj.ServiceId = message.ServiceId;
                    obj.IsActive = message.IsActive;

                    uow.GetRepository<FormGroup>().Update(obj);

                //List<Field> objlist=await uow.GetRepository<Field>       ().GetAllNonDeleted()
                //    .Where(x=>x.FormGroupId==message.Id && x.FormGroupCustomListId != null)
                //    .ToListAsync();
                //foreach(var item in objlist)
                //{
                //    uow.GetRepository<Field>().Delete(item);
                //}
                //if (message.FormGroupCustomListId is not null)
                //{
                //    FormGroupCustomList FormGroupCustomList=await uow.GetRepository<FormGroupCustomList>().GetAllNonDeleted()
                //    .Where(x=>x.Id==message.FormGroupCustomListId)
                //    .FirstOrDefaultAsync();
                //    if (FormGroupCustomList != null)
                //    {

                //        if (!string.IsNullOrEmpty(FormGroupCustomList.Schema))
                //        {
                //            List<Field> objfieldlist = new List<Field>();
                //            var jsonArray = JArray.Parse(FormGroupCustomList.Schema);
                //            foreach (var data in jsonArray)
                //            {
                //                Field objfield = new Field();
                //                objfield.FormGroupId = obj.Id;
                //                objfield.ServiceId = obj.ServiceId;
                //                FieldType FieldTypeId=await uow.GetRepository<FieldType>().GetAllNonDeleted()
                //                .Where(x=>x.BackendName==(string)data["FieldType"])
                //                .FirstOrDefaultAsync();
                //                objfield.FieldTypeId = FieldTypeId.Id;
                //                objfield.FormGroupCustomListId = obj.FormGroupCustomListId;
                //                objfield.TitleAr = (string)data["TitleAr"];
                //                objfield.TitleEn = (string)data["TitleEn"];
                //                objfield.BackendName = (string)data["BackendName"];
                //                objfield.Row = (int)data["Row"];
                //                objfield.Column = (int)data["Column"];
                //                objfieldlist.Add(objfield);
                //            }
                //            if (objfieldlist.Count > 0)
                //            {
                //                await uow.GetRepository<Field>().InsertRange(objfieldlist);
                //            }
                //        }


                //    }
                //}
                await uow.CommitAsync();
                //result = mapper.Map<FormGroupDTO>(obj);

                message.CreateBy = userInfo.Name;
                message.ResponseStatus = DBResult.Updated;
                }
                return message;
           
        }
        public async Task<FormGroupDTO> DeleteFormGroup(Guid? Id)
        {

            
                
                var result = new FormGroupDTO();
                if (Id is not null)
                {
                    FormGroup obj = await uow.GetRepository<FormGroup>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();

                if (servicefreezecount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);
                }
                List<Field> objfieldlist=await uow.GetRepository<Field>().GetAllNonDeleted()
                    .Where(x=>x.FormGroupListId==Id)
                    .ToListAsync();
                if(objfieldlist.Count>0)
                {
                    result.ResponseStatus = DBResult.Exist;
                    return result;
                }

                List<Field> objformgroupfieldlist=await uow.GetRepository<Field>().GetAllNonDeleted()
                    .Where(x=>x.FormGroupId==Id)
                    .ToListAsync();
                if (objformgroupfieldlist.Count > 0)
                {
                    foreach(var item in objformgroupfieldlist)
                    {
                        var FieldVisabilityConfig = await uow.GetRepository<FieldVisabilityConfig>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == item.Id)
                      .ToListAsync();
                        if (FieldVisabilityConfig.Count > 0)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldVisabilityConfig);
                        }
                      
                        var FieldAttributeValue = await uow.GetRepository<FieldAttributeValue>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == item.Id)
                      .ToListAsync();
                        if (FieldAttributeValue.Count > 0)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldAttributeValue);
                        }
                        var Field = await uow.GetRepository<Field>()
.GetAllNonDeleted()
                      .Where(x => x.DropDownParentFieldId == item.Id)
                      .ToListAsync();
                        if (Field.Count > 0)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsDropDownParentField);
                        }
                       
                        var FieldPartyType = await uow.GetRepository<FieldPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == item.Id)
                      .ToListAsync();
                        if (FieldPartyType.Count > 0)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldPartyType);
                        }
                       
                        var FieldViewCondition = await uow.GetRepository<FieldViewCondition>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == item.Id ||x.ParentFieldId== item.Id)
                      .ToListAsync();
                        if (FieldViewCondition.Count > 0)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldViewCondition);
                        }
                        var PlaceHolder = await uow.GetRepository<PlaceHolder>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == item.Id)
                      .ToListAsync();
                        if (PlaceHolder.Count > 0)
                        {
                            throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsPlaceHolder);
                        }
                        
                        uow.GetRepository<Field>().Delete(item);
                    }
                }

                uow.GetRepository<FormGroup>().Delete(obj);
                    await uow.CommitAsync();
                    
                    result.ResponseStatus = DBResult.Deleted;
                }
                return result;
           

        }
        public async Task<FieldDTO> SaveField(FieldDTO message)
        {

           

           
                FormGroup? objj = await uow
             .GetRepository<FormGroup>()
             .GetAllNonDeleted(x => x.Id == message.FormGroupId).FirstOrDefaultAsync();
                if (objj == null)
                {
                    message.ResponseStatus = DBResult.NotFound;
                    return message;

                }
                var FieldBackendName= await GenerateBackendName(message.TitleEn, message.ServiceId, "F");
                var existBackendName = await uow
             .GetRepository<Field>()
                  .GetAllNonDeleted(x => x.BackendName == FieldBackendName)
                  .FirstOrDefaultAsync();

                if (existBackendName != null)
                {

                    message.ResponseStatus = DBResult.Exist;
                    return message;
                }
            var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == message.ServiceId && x.IsFreez == true).ToListAsync();

            if (servicefreezecount.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);
            }
            var existRowColumn = await uow
             .GetRepository<Field>()
                .GetAllNonDeleted(x => x.Row == message.Row && x.Column == message.Column && x.Id != message.Id && x.FormGroupId == message.FormGroupId && x.ServiceId == message.ServiceId && x.IsActive == true && x.IsDeleted == false)
                .FirstOrDefaultAsync();


                if (existRowColumn != null)
                {
                    message.ResponseStatus = DBResult.SameRowColumn;
                    return message;
                }
                Field obj = new Field();

                obj.FormGroupId = message.FormGroupId;
                obj.ServiceId = message.ServiceId;
                obj.TitleAr = message.TitleAr;
                obj.TitleEn = message.TitleEn;
                obj.InfoAr = message.InfoAr;
                obj.InfoEn = message.InfoEn;
                obj.BackendName = FieldBackendName;
                obj.ClassName = message.ClassName;
                obj.Column = message.Column;
                obj.Row = message.Row;
                obj.Description = message.Description;
                obj.DropDownParentFieldId = message.DropDownParentFieldId;
                obj.FieldTypeId = message.FieldTypeId;
                obj.DropDownTypeId = message.DropDownTypeId;
                obj.IsActive = message.IsActive;
                obj.ReadFieldId = message.ReadFieldId;
                obj.MappingFieldId = message.MappingFieldId;
                obj.FormGroupListId = message.FormGroupListId;
                obj.EvalFormId = message.EvalFormId;

                uow.GetRepository<Field>().Insert(obj);

                foreach (var PartyTypeId in message.FieldPartyTypes)
                {
                    var fieldPartyType = new FieldPartyType
                    {
                        FieldId = obj.Id,
                        PartyTypeId = PartyTypeId,

                    };

                    uow.GetRepository<FieldPartyType>().Insert(fieldPartyType);
                }
                await uow.CommitAsync();
            message.UpdateBy = userInfo.DBName;
            message.FieldType = await uow.GetRepository<FieldType>().GetAllNonDeleted().Where(x => x.Id == obj.FieldTypeId).Select(x => _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn).FirstOrDefaultAsync()??string.Empty;
                message.ResponseStatus = DBResult.Inserted;
            message.UpdateDate = obj.CreateDate.ToString();
            message.Id = obj.Id;
            message.FieldCount = 0;
            message.BackendName = obj.BackendName;
           // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_FIELDS);
            return message;
            
        }
        public async Task<FieldDTO> UpdateField(FieldDTO message)
        {
           
                var result =new FieldDTO() ;

                if (message.Id is not null)
                {


                    Field obj = await uow.GetRepository<Field>()
                               .GetAllNonDeleted()
                               .Include(x=>x.CreateBy)
                               .Where(x => x.Id == message.Id)
                               .FirstAsync();
                var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();

                if (servicefreezecount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);
                }
                var existRowColumn = await uow.GetRepository<Field>()
                 .GetAllNonDeleted(x => x.Row == message.Row && x.Column==message.Column && x.Id!=message.Id &&  x.FormGroupId==message.FormGroupId && x.ServiceId==message.ServiceId && x.IsActive==true && x.IsDeleted==false )
                 .FirstOrDefaultAsync();


                    if (existRowColumn != null)
                    {
                        message.ResponseStatus = DBResult.SameRowColumn;
                        return message;
                    }




                    obj.FormGroupId = message.FormGroupId;
                    obj.ServiceId = message.ServiceId;
                    obj.TitleAr = message.TitleAr;
                    obj.TitleEn = message.TitleEn;
                    obj.InfoAr = message.InfoAr;
                    obj.InfoEn = message.InfoEn;
                    obj.BackendName = obj.BackendName;
                    obj.ClassName = message.ClassName;
                    obj.Column = message.Column;
                    obj.Row = message.Row;
                    obj.Description = message.Description;
                    obj.DropDownParentFieldId = message.DropDownParentFieldId;
                    obj.FieldTypeId = message.FieldTypeId;
                    obj.DropDownTypeId = message.DropDownTypeId;

                    obj.ReadFieldId = message.ReadFieldId;
                    obj.MappingFieldId = message.MappingFieldId;
                    obj.FormGroupListId = message.FormGroupListId;
                    if (obj.FormGroupCustomListId is not null)
                    {
                        obj.IsActive = obj.IsActive;
                    }
                    else
                    {
                        obj.IsActive = message.IsActive;
                    }
                obj.EvalFormId = message.EvalFormId;
                uow.GetRepository<Field>().Update(obj);


                    var existingPartyTypes = await  uow.GetRepository<FieldPartyType>()
                 .GetAllNonDeleted(x => x.FieldId == message.Id)
                 .ToListAsync();

                    var newPartyTypes = message.FieldPartyTypes.ToList();

                    foreach (var partyType in newPartyTypes)
                    {
                        if (!existingPartyTypes.Any(e => e.PartyTypeId == partyType))
                        {
                            var newPartyTypeEntity = new FieldPartyType { FieldId = message.Id.Value, PartyTypeId = partyType };

                            uow.GetRepository<FieldPartyType>().Insert(newPartyTypeEntity);
                        }
                    }

                    foreach (var existingPartyType in existingPartyTypes)
                    {
                        if (!newPartyTypes.Contains(existingPartyType.PartyTypeId))
                        {
                            uow.GetRepository<FieldPartyType>().Delete(existingPartyType);
                        }
                    }



                    await uow.CommitAsync();
                    message.UpdateBy = userInfo.DBName;
                

                message.FieldType = await uow.GetRepository<FieldType>().GetAllNonDeleted().Where(x => x.Id == obj.FieldTypeId).Select(x => _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn).FirstOrDefaultAsync()?? string.Empty;
                    message.UpdateDate = obj.UpdateDate.ToString();
                    message.ResponseStatus = DBResult.Updated;
               // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_FIELDS);
            }


                return message;
            
            

            
        }
        public async Task<FieldDTO> DeleteField(Guid? Id)
        {

           
                var mapper = await CreateMapperForAdmin<Field, FieldDTO>();
                var result = new FieldDTO();
                if (Id is not null)
                {
                    var objcount=await uow.GetRepository<FieldViewCondition>().GetAllNonDeleted(x=>x.FieldId== Id || x.ParentFieldId== Id).ToListAsync();
                    if (objcount.Count() > 0)
                    {
                        result.ResponseStatus = DBResult.Exist;
                        return result;
                    }

                    Field obj = await uow.GetRepository<Field>()
                               .GetAllNonDeleted()
                               .Where(x => x.Id == Id)
                               .FirstAsync();
                var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();

                if (servicefreezecount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);
                }
                //if (obj.FormGroupListId is not null)
                //{
                //    result.ResponseStatus = DBResult.CustomField;
                //    return result;

                //}
                var FieldVisabilityConfig = await uow.GetRepository<FieldVisabilityConfig>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == obj.Id)
                      .ToListAsync();
                if (FieldVisabilityConfig.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldVisabilityConfig);
                }
               
                var FieldAttributeValue = await uow.GetRepository<FieldAttributeValue>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == obj.Id)
                      .ToListAsync();
                if (FieldAttributeValue.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldAttributeValue);
                }
                var Field = await uow.GetRepository<Field>()
.GetAllNonDeleted()
                      .Where(x => x.DropDownParentFieldId == obj.Id)
                      .ToListAsync();
                if (Field.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsDropDownParentField);
                }
              
                var FieldPartyType = await uow.GetRepository<FieldPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == obj.Id)
                      .ToListAsync();
                if (FieldPartyType.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldPartyType);
                }
                
                var FieldViewCondition = await uow.GetRepository<FieldViewCondition>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == obj.Id ||x.ParentFieldId== obj.Id)
                      .ToListAsync();
                if (FieldViewCondition.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsFieldViewCondition);
                }
                var PlaceHolder = await uow.GetRepository<PlaceHolder>()
.GetAllNonDeleted()
                      .Where(x => x.FieldId == obj.Id)
                      .ToListAsync();
                if (PlaceHolder.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.FieldExistsPlaceHolder);
                }
                
                uow.GetRepository<Field>().Delete(obj);
                    await uow.CommitAsync();
                    result = mapper.Map<FieldDTO>(obj);
                    result.ResponseStatus = DBResult.Deleted;
               // await _CacheDataProvider.ClearCacheByKey(ConstantKeys.WebAppCacheTableName.CACHE_FIELDS);
            }
                return result;
           

        }

        public async Task<FieldAttributeValueDTO> SaveFieldAttribute(FieldAttributeValueDTO message)
        {

            

            Field? objj = await uow
             .GetRepository< Field>()
             .GetAllNonDeleted(x => x.Id == message.FieldId).FirstOrDefaultAsync();
                if (objj == null)
                {
                    message.ResponseStatus = DBResult.NotFound;
                    return message;

                }

                FieldAttributeValue obj = new FieldAttributeValue();

                obj.AttributeKey = message.AttributeKey;
                obj.AttributeValue = message.AttributeValue;
                obj.MessageAr = message.MessageAr;
                obj.MessageEn = message.MessageEn;
                obj.IsActive = message.IsActive;
                obj.FieldId = message.FieldId;
                obj.Description = message.Description;

                uow.GetRepository<FieldAttributeValue>().Insert(obj);

               
                await uow.CommitAsync();
            message.Id = obj.Id;
                message.UpdateBy = userInfo.DBName;
                message.UpdateDate = obj.CreateDate.ToString();
                message.ResponseStatus = DBResult.Inserted;

                return message;
           
        }
        public async Task<FieldAttributeValueDTO> UpdateFieldAttribute(FieldAttributeValueDTO message)
        {

            
           
                if (message.Id is not null)
                {
                    Field? objj = await uow
             .GetRepository< Field>()
             .GetAllNonDeleted(x => x.Id == message.FieldId).FirstOrDefaultAsync();
                    if (objj == null)
                    {
                        message.ResponseStatus = DBResult.NotFound;
                        return message;

                    }

                    FieldAttributeValue obj = await uow.GetRepository<FieldAttributeValue>()
                               .GetAllNonDeleted()
                               .Include(x=>x.CreateBy)
                               .Where(x => x.Id == message.Id)
                               .FirstAsync();

                    obj.AttributeKey = message.AttributeKey;
                    obj.AttributeValue = message.AttributeValue;
                    obj.MessageAr = message.MessageAr;
                    obj.MessageEn = message.MessageEn;
                    obj.IsActive = message.IsActive;
                    obj.FieldId = message.FieldId;
                    obj.Description = message.Description;
                    uow.GetRepository<FieldAttributeValue>().Update(obj);
                    await uow.CommitAsync();
                message.UpdateBy = userInfo.DBName;
                message.UpdateDate = obj.UpdateDate.ToString();
                message.ResponseStatus = DBResult.Updated;
                }
                return message;
            
        }
        public async Task<FieldAttributeValueDTO> DeleteFieldAttribute(Guid? Id)
        {

           
                var mapper = await CreateMapperForAdmin<FieldAttributeValue, FieldAttributeValueDTO>();
                var result = new FieldAttributeValueDTO();
                if (Id is not null)
                {


                    FieldAttributeValue obj = await uow.GetRepository<FieldAttributeValue>()
                               .GetAllNonDeleted()
                               .Where(x => x.Id == Id)
                               .FirstAsync();
                    uow.GetRepository<FieldAttributeValue>().Delete(obj);
                    await uow.CommitAsync();
                    result = mapper.Map<FieldAttributeValueDTO>(obj);
                    result.ResponseStatus = DBResult.Deleted;
                }
                return result;
            

        }

        public async Task<FieldViewConditionDTO> SaveFieldCondition(FieldViewConditionDTO message)
        {

            var mapper = await CreateMapperForAdmin<FieldViewCondition, FieldViewConditionDTO>();

            var isArabic = _requestInfo.Lang == "ar";
            Field? objj = await uow
             .GetRepository< Field>()
             .GetAllNonDeleted(x => x.Id == message.FieldId).FirstOrDefaultAsync();
                if (objj == null)
                {
                    message.ResponseStatus = DBResult.NotFound;
                    return message;

                }

                FieldViewCondition obj = new FieldViewCondition();
                if (message.FieldDropDownValueIds.Any())
                {
                    obj.FieldValue = string.Join(",", message.FieldDropDownValueIds);
                    obj.FieldDropDownValueIds = obj.FieldValue;

                 }
                else
                {
                    obj.FieldValue = message.FieldValue;
                }
                obj.IsSufficient = message.IsSufficient;
                obj.FieldId = message.FieldId!.Value;
                obj.operators = message.operators;
                obj.ParentFieldId = message.ParentFieldId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<FieldViewCondition>().Insert(obj);


                await uow.CommitAsync();
                message.Id = obj.Id;
                message.UpdateBy = userInfo.DBName;
                message.UpdateDate = obj.CreateDate.ToString();
                message.ResponseStatus = DBResult.Inserted;
            message.ParentField = await uow.GetRepository<Field>().GetAllNonDeleted().Where(x => x.Id == obj.ParentFieldId).Select(x => (_requestInfo.Lang == "ar" ? x.TitleAr : x.TitleEn)).FirstOrDefaultAsync();
            message.FieldValueDisplay = GetFieldValue(obj.FieldValue, message.ParentFieldId, isArabic, serviceProvider);
            return message;
            
        }
        public async Task<FieldViewConditionDTO> UpdateFieldCondition(FieldViewConditionDTO message)
        {

            var isArabic = _requestInfo.Lang == "ar";

            if (message.Id is not null)
                {
                Field? objj = await uow
             .GetRepository< Field>()
             .GetAllNonDeleted(x => x.Id == message.FieldId).FirstOrDefaultAsync();
                    if (objj == null)
                    {
                        message.ResponseStatus = DBResult.NotFound;
                        return message;

                    }

                    FieldViewCondition obj = await uow.GetRepository<FieldViewCondition>()
                               .GetAllNonDeleted()
                               .Include(x=>x.CreateBy)
                               .Where(x => x.Id == message.Id)
                               .FirstAsync();

                    if (message.FieldDropDownValueIds.Any())
                    {
                        obj.FieldValue = string.Join(",", message.FieldDropDownValueIds);
                        obj.FieldDropDownValueIds = obj.FieldValue;

                    }
                    else
                    {
                        obj.FieldValue = message.FieldValue;
                    }
                    obj.FieldId = message.FieldId!.Value;
                    obj.IsSufficient = message.IsSufficient;
                    obj.operators = message.operators;
                    obj.ParentFieldId = message.ParentFieldId;
                    obj.IsActive = message.IsActive;
                    uow.GetRepository<FieldViewCondition>().Update(obj);
                    await uow.CommitAsync();
                    //result = mapper.Map<FieldDTO>(obj);
                    message.Id = obj.Id;
                message.UpdateBy = userInfo.DBName;
                message.UpdateDate = obj.UpdateDate.ToString();
                message.ResponseStatus = DBResult.Updated;
                message.ParentField = await uow.GetRepository<Field>().GetAllNonDeleted().Where(x => x.Id == obj.ParentFieldId).Select(x => (_requestInfo.Lang == "ar" ? x.TitleAr : x.TitleEn)).FirstOrDefaultAsync();
                message.FieldValueDisplay = GetFieldValue(obj.FieldValue, message.FieldId, isArabic, serviceProvider);
            }
                return message;
           
        }
        public async Task<FieldViewConditionDTO> DeleteFieldCondition(Guid? Id)
        {

            
               
                var result = new FieldViewConditionDTO();
                if (Id is not null)
                {


                    FieldViewCondition obj = await uow.GetRepository<FieldViewCondition>()
                               .GetAllNonDeleted()
                               .Where(x => x.Id == Id)
                               .FirstAsync();
                    uow.GetRepository<FieldViewCondition>().Delete(obj);
                    await uow.CommitAsync();
                 result = mapper.Map<FieldViewConditionDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
                }
                return result;
            

        }
    }
}
