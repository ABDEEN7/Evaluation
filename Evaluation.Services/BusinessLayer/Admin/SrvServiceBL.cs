using AutoMapper;
using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Attachments;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.ServiceRequestEntities;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.StatusEntities;
using Evaluation.DAL.Entities.Template;
using Evaluation.DAL.Helper;
using Evaluation.DAL.SystemSetting;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
namespace Evaluation.Services.Models.Admin
{
    public class SrvServiceBL : AdminBase
    {
        public SrvServiceBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }

        #region Service
        public async Task<List<ServiceDTO>> GetServiceList(int Page, int PageSize, Guid? SystemModuleId)
        {

            Guid specialGuid = new Guid("00000000-0000-0000-0000-000000000000");
            if (SystemModuleId.Equals(specialGuid))
            {
                SystemModuleId = null;
            }

            var list = await uow.GetRepository<Service>()
                .GetAllNonDeleted()
                .Include(x => x.SystemModule)
                .Include(x => x.CreateBy)
               .Where(x => x.SystemModuleId == (SystemModuleId ?? x.SystemModuleId))
                 .OrderBy(x => x.OrderNo)
                .ThenByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<ServiceDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }

        public async Task<ServiceDTO> SaveService(ServiceDTO message)
        {


            var PartyTypeBackendName = await GenerateBackendNameBySystemModule(message.NameEn, message.SystemModuleId, "P");
            var existBackendName = await uow
             .GetRepository<Service>()
                  .GetAllNonDeleted(x => x.BackendName == PartyTypeBackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }
            if (message.Initialservice)
            {
                var initialservicecount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted().Where(x => x.SystemModuleId == message.SystemModuleId && x.Initialservice).FirstOrDefaultAsync();
                if (initialservicecount != null)
                {
                    var errormessage = await GetUiMessage(ConstantKeys.AdminBackendUI.ServiceAlreadyExists);
                    var newmessage = string.Format(errormessage, (_requestInfo.Lang == "ar" ? initialservicecount.NameAr : initialservicecount.NameEn));
                    message.ResponseStatus = DBResult.Exist;
                    message.ResponseMessage = newmessage;
                    return message;


                }
            }


            var datacount = await uow.GetRepository<Service>()
                      .GetAllNonDeleted()
                      .Where(x => x.PrefixCode == message.PrefixCode).ToListAsync();
            if (datacount.Count > 0)
            {
                message.ResponseStatus = DBResult.PrefixExist;
                return message;

            }
            Service obj = new Service();

            obj.SystemModuleId = message.SystemModuleId;
            obj.NameAr = message.NameAr;
            obj.NameEn = message.NameEn;
            obj.BackendName = PartyTypeBackendName;
            obj.DescriptionAr = message.DescriptionAr;
            obj.DescriptionEn = message.DescriptionEn;
            obj.PrefixCode = message.PrefixCode;
            obj.ReqNumberDef = message.ReqNumberDef;
            obj.IsAutoAssignEnabled = message.IsAutoAssignEnabled;
            obj.IsFreez = false;
            obj.FreezDate = message.FreezDate;
            obj.Icon = message.Icon;
            obj.StartDate = message.StartDate;
            obj.EndDate = message.EndDate;
            obj.ServiceSettings = message.ServiceSettings;
            obj.Initialservice = message.Initialservice;
            obj.IsActive = message.IsActive;
            await uow.GetRepository<Service>().InsertAsync(obj);
            if (!message.Initialservice)
            {
                //checking initial service 
                var initialserviceid = await uow.GetRepository<Service>()
                    .GetAllNonDeleted()
                    .Where(x => x.SystemModuleId == obj.SystemModuleId && x.Initialservice)
                    .FirstOrDefaultAsync();
                if (initialserviceid != null)
                {
                    //inserting formgroups list 
                    var formGroupsList = await uow.GetRepository<FormGroup>()
                                    .GetAllNonDeleted()
                                    .Include(x => x.FormGroupType)
                                    .Where(x => x.ServiceId == initialserviceid.Id && x.FormGroupType!.BackendName == "List")
                                    .AsNoTracking()
                                    .ToListAsync();
                    foreach (var formgroup in formGroupsList)
                    {
                        FormGroup formgroupinsert = new FormGroup();
                        formgroupinsert.TitleAr = formgroup.TitleAr;
                        formgroupinsert.TitleEn = formgroup.TitleEn;
                        formgroupinsert.ServiceId = obj.Id;
                        formgroupinsert.Order = formgroup.Order;
                        formgroupinsert.IsActive = formgroup.IsActive;
                        formgroupinsert.FormGroupTypeId = formgroup.FormGroupTypeId;
                        formgroupinsert.FormGroupCustomListId = formgroup.FormGroupCustomListId;
                        await uow.GetRepository<FormGroup>().InsertAsync(formgroupinsert);
                        //inserting fields list 
                        List<(Guid, Guid)> FieldsIdList = new List<(Guid, Guid)>();
                        var FieldsList = await uow.GetRepository<Field>()
                                    .GetAllNonDeleted()
                                    .Where(x => x.FormGroupId == formgroup.Id)
                                    .AsNoTracking()
                                    .ToListAsync();
                        if (FieldsList.Any())
                        {
                            foreach (var Fields in FieldsList)
                            {
                                Field Fieldinsert = new Field();
                                Fieldinsert.FormGroupId = formgroupinsert.Id;
                                Fieldinsert.TitleAr = Fields.TitleAr;
                                Fieldinsert.TitleEn = Fields.TitleEn;
                                Fieldinsert.BackendName = Fields.BackendName;
                                Fieldinsert.InfoAr = Fields.InfoAr;
                                Fieldinsert.InfoEn = Fields.InfoEn;
                                Fieldinsert.FieldTypeId = Fields.FieldTypeId;
                                Fieldinsert.ServiceId = obj.Id;
                                Fieldinsert.Column = Fields.Column;
                                Fieldinsert.Row = Fields.Row;
                                Fieldinsert.Description = Fields.Description;
                                if (Fields.DropDownParentFieldId != null)
                                {

                                    var parentFieldId = FieldsIdList
    .FirstOrDefault(x => x.Item1 == Fields.DropDownParentFieldId)
    .Item2;

                                    Fieldinsert.DropDownParentFieldId =
                                        parentFieldId != Guid.Empty ? parentFieldId : Fields.DropDownParentFieldId;

                                }
                                else
                                {
                                    Fieldinsert.DropDownParentFieldId = Fields.DropDownParentFieldId;
                                }
                                Fieldinsert.DropDownTypeId = Fields.DropDownTypeId;
                                Fieldinsert.FormGroupListId = Fields.FormGroupListId;
                                Fieldinsert.IsActive = Fields.IsActive;
                                Fieldinsert.FormGroupCustomListId = Fields.FormGroupCustomListId;
                                uow.GetRepository<Field>().Insert(Fieldinsert);
                                FieldsIdList.Add((Fields.Id, Fieldinsert.Id));

                                var FieldAttributeValueList = await uow.GetRepository<FieldAttributeValue>()
                                    .GetAllNonDeleted()
                                    .Where(x => x.FieldId == Fields.Id)
                                    .AsNoTracking()
                                    .ToListAsync();
                                if (FieldAttributeValueList.Any())
                                {
                                    foreach (var FieldAttributeValue in FieldAttributeValueList)
                                    {
                                        FieldAttributeValue.FieldId = Fieldinsert.Id;
                                    }
                                    await uow.GetRepository<FieldAttributeValue>().InsertRange(FieldAttributeValueList);
                                }


                                var FieldViewConditionList = await uow.GetRepository<FieldViewCondition>()
                                    .GetAllNonDeleted()
                                    .Where(x => x.FieldId == Fields.Id)
                                    .AsNoTracking()
                                    .ToListAsync();
                                if (FieldViewConditionList.Any())
                                {
                                    foreach (var FieldViewCondition in FieldViewConditionList)
                                    {
                                        FieldViewCondition.FieldId = Fieldinsert.Id;
                                    }
                                    await uow.GetRepository<FieldViewCondition>().InsertRange(FieldViewConditionList);
                                }

                            }


                        }

                    }






                }

            }



            //insert values to ServiceInitiatorPartyType
            if (message.ServiceInitiatorPartyType != null)
            {
                List<ServiceInitiatorPartyType> objentitylist = new List<ServiceInitiatorPartyType>();
                foreach (var item in message.ServiceInitiatorPartyType)
                {
                    ServiceInitiatorPartyType objentity = new ServiceInitiatorPartyType();
                    objentity.PartyTypeId = item;
                    objentity.serviceId = obj.Id;
                    objentity.IsActive = true;
                    objentitylist.Add(objentity);
                }
                if (objentitylist.Count > 0)
                {
                    await uow.GetRepository<ServiceInitiatorPartyType>().InsertRange(objentitylist);
                }


            }


            //insert values to ServiceRequestShowPartyType
            if (message.ServiceRequestShowPartyType != null)
            {
                List<ServiceRequestShowPartyType> objentitylist = new List<ServiceRequestShowPartyType>();
                foreach (var item in message.ServiceRequestShowPartyType)
                {
                    ServiceRequestShowPartyType objentity = new ServiceRequestShowPartyType();
                    objentity.PartyTypeId = item;
                    objentity.serviceId = obj.Id;
                    objentity.IsActive = true;
                    objentitylist.Add(objentity);
                }
                if (objentitylist.Count > 0)
                {
                    await uow.GetRepository<ServiceRequestShowPartyType>().InsertRange(objentitylist);
                }


            }

            await uow.CommitAsync();
            var result = mapper.Map<ServiceDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
            result.ServiceInitiatorPartyType = message.ServiceInitiatorPartyType;
            result.ServiceRequestShowPartyType = message.ServiceRequestShowPartyType;
            result.ResponseStatus = DBResult.Inserted;
            return result;

        }
        public async Task<ServiceDTO> UpdateService(ServiceDTO message)
        {


            try
            {

                var result = new ServiceDTO();

                if (message.Id is not null)
                {

                    if (message.Initialservice)
                    {
                        var initialservicecount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted().Where(x => x.SystemModuleId == message.SystemModuleId && x.Initialservice == true && x.Id != message.Id).FirstOrDefaultAsync();
                        if (initialservicecount != null)
                        {
                            var errormessage = await GetUiMessage(ConstantKeys.AdminBackendUI.ServiceAlreadyExists);
                            var newmessage = string.Format(errormessage, (_requestInfo.Lang == "ar" ? initialservicecount.NameAr : initialservicecount.NameEn));
                            message.ResponseStatus = DBResult.Exist;
                            message.ResponseMessage = newmessage;
                            return message;


                        }
                    }
                    Service obj = await uow.GetRepository<Service>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                    if (obj.IsFreez == true)
                    {
                        throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);

                    }

                    obj.SystemModuleId = message.SystemModuleId;
                    obj.NameAr = message.NameAr;
                    obj.NameEn = message.NameEn;
                    obj.BackendName = obj.BackendName;
                    obj.DescriptionAr = message.DescriptionAr;
                    obj.DescriptionEn = message.DescriptionEn;
                    obj.PrefixCode = obj.PrefixCode;
                    obj.ReqNumberDef = message.ReqNumberDef;
                    obj.IsAutoAssignEnabled = message.IsAutoAssignEnabled;
                    obj.IsFreez = false;
                    obj.FreezDate = message.FreezDate;
                    obj.Icon = message.Icon;
                    obj.StartDate = message.StartDate;
                    obj.EndDate = message.EndDate;
                    obj.ServiceSettings = message.ServiceSettings;
                    obj.Initialservice = message.Initialservice;
                    obj.IsActive = message.IsActive;
                    uow.GetRepository<Service>().Update(obj);
                    //update values to ServiceInitiatorPartyType
                    List<ServiceInitiatorPartyType> objServiceInitiatorentitydelete = await uow.GetRepository<ServiceInitiatorPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.serviceId == obj.Id)
                                      .ToListAsync();

                    var ServiceInitiatorexistids = new List<Guid>();
                    if (objServiceInitiatorentitydelete.Count > 0)
                    {
                        foreach (var item in objServiceInitiatorentitydelete)
                        {
                            if (message.ServiceInitiatorPartyType != null && message.ServiceInitiatorPartyType.Contains(item.PartyTypeId))
                            {
                                ServiceInitiatorexistids.Add(item.PartyTypeId);
                            }
                            else
                            {
                                uow.GetRepository<ServiceInitiatorPartyType>().Delete(item);
                            }

                        }
                    }
                    if (message.ServiceInitiatorPartyType != null)
                    {
                        var notInSelected = message.ServiceInitiatorPartyType.Except(ServiceInitiatorexistids).ToList();
                        List<ServiceInitiatorPartyType> objentitylist = new List<ServiceInitiatorPartyType>();
                        foreach (var item in notInSelected)
                        {
                            ServiceInitiatorPartyType objentity = new ServiceInitiatorPartyType();
                            objentity.serviceId = obj.Id;
                            objentity.PartyTypeId = item;
                            objentity.IsActive = true;
                            objentitylist.Add(objentity);
                        }
                        if (objentitylist.Count > 0)
                        {
                            await uow.GetRepository<ServiceInitiatorPartyType>().InsertRange(objentitylist);
                        }


                    }
                    //update values to ServiceRequestShowPartyType
                    List<ServiceRequestShowPartyType> objServiceRequestentitydelete = await uow.GetRepository<ServiceRequestShowPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.serviceId == obj.Id)
                                      .ToListAsync();

                    var ServiceRequestexistids = new List<Guid>();
                    if (objServiceRequestentitydelete.Count > 0)
                    {
                        foreach (var item in objServiceRequestentitydelete)
                        {
                            if (message.ServiceRequestShowPartyType != null && message.ServiceRequestShowPartyType.Contains(item.PartyTypeId))
                            {
                                ServiceRequestexistids.Add(item.PartyTypeId);
                            }
                            else
                            {
                                uow.GetRepository<ServiceRequestShowPartyType>().Delete(item);
                            }

                        }
                    }
                    if (message.ServiceRequestShowPartyType != null)
                    {
                        var notInSelected = message.ServiceRequestShowPartyType.Except(ServiceRequestexistids).ToList();
                        List<ServiceRequestShowPartyType> objentitylist = new List<ServiceRequestShowPartyType>();
                        foreach (var item in notInSelected)
                        {
                            ServiceRequestShowPartyType objentity = new ServiceRequestShowPartyType();
                            objentity.serviceId = obj.Id;
                            objentity.PartyTypeId = item;
                            objentity.IsActive = true;
                            objentitylist.Add(objentity);
                        }
                        if (objentitylist.Count > 0)
                        {
                            await uow.GetRepository<ServiceRequestShowPartyType>().InsertRange(objentitylist);
                        }


                    }


                    await uow.CommitAsync();
                    result = mapper.Map<ServiceDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                    result.ServiceInitiatorPartyType = message.ServiceInitiatorPartyType;
                    result.ServiceRequestShowPartyType = message.ServiceRequestShowPartyType;
                    result.ResponseStatus = DBResult.Updated;
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public async Task<bool> UpdateServiceOrder(List<OrderingDTO> message)
        {
            bool rtn = false;

            var updatedRows = from updatedItem in message
                              join rowToUpdate in uow.GetRepository<Service>().GetAllNonDeleted() on updatedItem.Id equals rowToUpdate.Id
                              select new { Row = rowToUpdate, updatedItem.OrderNo };



            updatedRows.ToList().ForEach(x => x.Row.OrderNo = x.OrderNo);

            await uow.CommitAsync();
            rtn = true;


            return rtn;
        }
        public async Task<ServiceDTO> UpdateServiceIsFreeze(ServiceDTO
         message)
        {

            if (message.Id is not null)
            {

                Service obj = await uow.GetRepository<Service>()
                                  .GetAllNonDeleted()
                                  .Include(x => x.CreateBy)
                                  .Where(x => x.Id == message.Id)
                                  .FirstAsync();
                obj.IsFreez = message.IsFreez;
                if (obj.IsFreez)
                {
                    obj.FreezDate = DateTime.Now;
                }
                uow.GetRepository<Service>().Update(obj);
                message.ResponseStatus = DBResult.Updated;
                message.FreezDate = obj.FreezDate;
            }
            await uow.CommitAsync();

            return message;
        }
        public async Task<ServiceDTO> DeleteService(Guid? Id)
        {




            var result = new ServiceDTO();
            if (Id is not null)
            {
                Service obj = await uow.GetRepository<Service>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();

                if (obj.IsFreez == true)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);
                }
                List<ServiceInitiatorPartyType> objdelete = await uow.GetRepository<ServiceInitiatorPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.serviceId == obj.Id)
                                      .ToListAsync();
                if (objdelete.Count > 0)
                {
                    foreach (var item in objdelete)
                    {
                        uow.GetRepository<ServiceInitiatorPartyType>().Delete(item);
                    }
                }
                List<ServiceRequestShowPartyType> objentitydelete = await uow.GetRepository<ServiceRequestShowPartyType>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.serviceId == obj.Id)
                                      .ToListAsync();
                if (objentitydelete.Count > 0)
                {
                    foreach (var item in objentitydelete)
                    {
                        uow.GetRepository<ServiceRequestShowPartyType>().Delete(item);
                    }
                }
                List<PlaceHolder> PlaceHolderdelete = await uow.GetRepository<PlaceHolder>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.ServiceId == obj.Id)
                                      .ToListAsync();
                if (PlaceHolderdelete.Count > 0)
                {
                    foreach (var item in PlaceHolderdelete)
                    {
                        uow.GetRepository<PlaceHolder>().Delete(item);
                    }
                }
                var FormGroup = await uow.GetRepository<FormGroup>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceId == obj.Id)
                      .ToListAsync();
                if (FormGroup.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsFormGroup);
                }



                var ServiceRequest = await uow.GetRepository<ServiceRequest>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceId == obj.Id)
                      .ToListAsync();
                if (ServiceRequest.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsServiceRequest);
                }

                var Field = await uow.GetRepository<Field>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceId == obj.Id)
                      .ToListAsync();
                if (Field.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsField);
                }
                var ServiceStatus = await uow.GetRepository<ServiceStatus>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceId == obj.Id)
                      .ToListAsync();
                if (ServiceStatus.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsServiceStatus);
                }
                var ServiceAction = await uow.GetRepository<ServiceAction>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceId == obj.Id)
                      .ToListAsync();
                if (ServiceAction.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsServiceAction);
                }
                //                var TemplateDoc = await uow.GetRepository<TemplateDocument>()
                //.GetAllNonDeleted()
                //                      .Where(x => x.ServiceId == obj.Id)
                //                      .ToListAsync();
                //                if (TemplateDoc.Count > 0)
                //                {
                //                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsTemplateDoc);
                //                }
                var EmailTemplate = await uow.GetRepository<EmailTemplate>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceId == obj.Id)
                      .ToListAsync();
                if (EmailTemplate.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceExistsEmailTemplate);
                }
                uow.GetRepository<Service>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<ServiceDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;

        }

        #endregion

        #region Placeholder

        public async Task<List<DropdownItem>> GetAllChildFields(Guid FieldId)
        {
            var result = new List<DropdownItem>();
            var customfield = await uow.GetRepository<Field>().GetAllActiveNonDeleted().Where(x => x.Id == FieldId).Select(x => x.FormGroupListId).FirstOrDefaultAsync();
            if (customfield != null)
            {
                result = await uow.GetRepository<Field>()
               .GetAllActiveNonDeleted()
               .Include(x => x.FormGroup)
              .Where(x => x.FormGroupId == customfield)
               .OrderByDescending(x => x.CreateDate)
               .Select(x => new DropdownItem
               {
                   Id = x.Id,
                   Title = _requestInfo.Lang == "ar" ? x.FormGroup!.TitleAr + "_" + x.TitleAr : x.FormGroup!.TitleEn + "_" + x.TitleEn,
               })
               .ToListAsync();
            }


            return result;

        }
        public async Task<List<DropdownItem>> GetRequestField(Guid serviceid)
        {

            var result = await uow.GetRepository<Field>()
                .GetAllActiveNonDeleted()
                .Include(x => x.Service)
                .Include(x => x.FieldType)
                .Include(x => x.CreateBy)
                .Include(x => x.FormGroup)
                .Include(x => x.FormGroup!.FormGroupType)
               .Where(x => x.ServiceId == serviceid && x.FormGroup!.FormGroupType!.BackendName == "FormGroup")
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new DropdownItem
                {
                    Id = x.Id,
                    Title = _requestInfo.Lang == "ar" ? x.FormGroup!.TitleAr + "_" + x.TitleAr : x.FormGroup!.TitleEn + "_" + x.TitleEn,
                    Type = "RequestField",
                    FieldType = x.FieldType!.BackendName
                })
                .ToListAsync();
            var rslt1 = await uow.GetRepository<SystemSetting>()
                        .GetAllActiveNonDeleted(x => x.SettingKey == ConstantKeys.AdminSettings.RequestColumn)
                        .Select(x => x.SettingValue)
                        .FirstOrDefaultAsync();
            if (rslt1 != null)
            {
                var jsonArray = JArray.Parse(rslt1);
                foreach (var data in jsonArray)
                {
                    DropdownItem rslt2 = new DropdownItem();
                    rslt2.Id = (data["Id"]?.ToString() ?? "");
                    rslt2.Title = _requestInfo.Lang == "ar"
    ? (data["TitleAr"]?.ToString() ?? "")
    : (data["TitleEn"]?.ToString() ?? "");
                    rslt2.Type = "RequestColumn";
                    rslt2.FieldType = "";
                    result.Add(rslt2);
                }
            }

            return result;

        }

        public async Task<List<PlaceHolderDTO>> GetPlaceHolderList(Guid serviceid)
        {


            var list = await uow.GetRepository<PlaceHolder>()
                .GetAllNonDeleted()
                .Include(x => x.CreateBy)
               .Where(x => x.ServiceId == serviceid)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var result = mapper.Map<List<PlaceHolderDTO>>(list, opts => opts.Items["Language"] = _requestInfo.Lang);

            return result;

        }
        public async Task<PlaceHolderDTO> SavePlaceHolder(PlaceHolderDTO message)
        {


            var isfreezcount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted(x => x.Id == message.ServiceId && x.IsFreez == true).ToListAsync();
            if (isfreezcount.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);

            }

            var existBackendName = await uow
             .GetRepository<PlaceHolder>()
                  .GetAllNonDeleted(x => x.PlaceHolderName == message.PlaceHolderName && x.ServiceId == message.ServiceId)
                  .ToListAsync();

            if (existBackendName.Count > 0)
            {

                message.ResponseStatus = DBResult.BackendExist;
                return message;
            }

            PlaceHolder obj = new PlaceHolder();

            obj.ServiceId = message.ServiceId;
            obj.PlaceHolderName = message.PlaceHolderName;
            obj.TypeDisplay = message.TypeDisplay;
            obj.Type = message.Type;
            if (message.Type == ConstantKeys.AdminSettings.RequestColumn || message.Type == ConstantKeys.AdminSettings.ScholarshipColumn)
            {
                obj.ColumnName = message.FieldId;
            }
            else
            {
                obj.FieldId = Guid.Parse(message.FieldId!);

            }
            if (message.ChildFieldIds != null && message.ChildFieldIds.Any())
            {
                obj.ChildFieldIds = string.Join(",", message.ChildFieldIds);

            }
            obj.IsActive = message.IsActive;
            uow.GetRepository<PlaceHolder>().Insert(obj);

            await uow.CommitAsync();
            var result = mapper.Map<PlaceHolderDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);

            result.ResponseStatus = DBResult.Inserted;
            return result;
        }

        public async Task<PlaceHolderDTO> UpdatePlaceHolder(PlaceHolderDTO
          message)
        {

            var result = new PlaceHolderDTO();
            if (message.Id is not null)
            {


                var existBackendName = await uow
             .GetRepository<PlaceHolder>()
                  .GetAllNonDeleted(x => x.PlaceHolderName == message.PlaceHolderName && x.ServiceId == message.ServiceId && x.Id != message.Id)
                  .ToListAsync();

                if (existBackendName.Count > 0)
                {

                    message.ResponseStatus = DBResult.BackendExist;
                    return message;
                }

                PlaceHolder obj = await uow
             .GetRepository<PlaceHolder>()
                        .GetAllNonDeleted()
                        .Where(x => x.Id == message.Id)
                        .FirstAsync();
                var isfreezcount = await uow.GetRepository<Service>()
                                          .GetAllNonDeleted(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();
                if (isfreezcount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);

                }
                obj.ServiceId = message.ServiceId;
                obj.PlaceHolderName = message.PlaceHolderName;
                obj.TypeDisplay = message.TypeDisplay;
                obj.Type = message.Type;
                if (message.Type == ConstantKeys.AdminSettings.RequestColumn || message.Type == ConstantKeys.AdminSettings.ScholarshipColumn)
                {
                    obj.ColumnName = message.FieldId!.ToString();
                }
                else
                {
                    obj.FieldId = Guid.Parse(message.FieldId!);

                }
                if (message.ChildFieldIds != null && message.ChildFieldIds.Any())
                {
                    obj.ChildFieldIds = string.Join(",", message.ChildFieldIds);

                }
                obj.IsActive = message.IsActive;
                uow.GetRepository<PlaceHolder>().Update(obj);

                await uow.CommitAsync();
                result = mapper.Map<PlaceHolderDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }
            return result;
        }

        public async Task<PlaceHolderDTO> DeletePlaceHolder(Guid? Id)
        {




            var result = new PlaceHolderDTO();
            if (Id is not null)
            {
                PlaceHolder obj = await uow.GetRepository<PlaceHolder>()
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
                uow.GetRepository<PlaceHolder>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<PlaceHolderDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;

        }
        #endregion
    }
}
