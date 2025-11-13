using AutoMapper;
using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.Template;
using Evaluation.DAL.Helper;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
namespace Evaluation.Services.Models.Admin
{
    public class SrvServiceActionBL : AdminBase
    {
        private readonly SrvBaseBL srvApplicationBL;

        public SrvServiceActionBL(IServiceProvider serviceProvider, UnitOfWork uow,
            LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
            IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo, SrvBaseBL srvApplicationBL)
            : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {
            this.srvApplicationBL = srvApplicationBL;
        }

        public async Task<List<ServiceActionDTO>> GetAllServiceActionList(Guid serviceId, int Page, int PageSize)
        {
            var mapper = await CreateMapperForAdmin<ServiceAction, ServiceActionDTO>();
            var list = await uow.GetRepository<ServiceAction>()
                .GetAllNonDeleted()
                .Where(x => x.ServiceId == serviceId)
                .Include(x => x.CreateBy)
                .OrderByDescending(x => x.CreateDate)
                // .Skip((Page-1)*PageSize)
                // .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<ServiceActionDTO>>(list);

            return result;
        }
        private static string LangSelector(bool isArabic, string ar, string en) => isArabic ? ar : en;

        public async Task<List<ActionConditionDTO>> GetActionConditionByActionList(Guid actionId)
        {
            var lang = _requestInfo.Lang;
            var isArabic = _requestInfo.Lang == "ar";
            var Fields = uow.GetRepository<Field>()
        .GetAllActiveNonDeleted()
        .ToDictionary(x => x.Id, x => LangSelector(isArabic, x.TitleAr, x.TitleEn));


            var list = await uow.GetRepository<ActionCondition>()
        .GetAllNonDeleted()
        .Include(x => x.ServiceAction)
        .Include(x => x.CreateBy)
        .Include(x => x.UpdateBy)
        .Where(x => x.ServiceActionId == actionId)
        .OrderByDescending(x => x.CreateDate)
        .ToListAsync();

            var result = list.Select(x => new ActionConditionDTO
            {
                Id = x.Id,
                Type = x.Type,
                operators = x.operators,
                IsActive = x.IsActive,
                RefID = x.RefID ?? Guid.Empty,
                Ref = (Fields.TryGetValue(x.RefID.Value, out var val) ? val : ""),
                FieldValue = x.FieldValue,
                FieldValueDisplay = GetFieldValue(x.Type, x.FieldValue, x.RefID, isArabic, serviceProvider),
                UpdateBy = x.UpdateBy != null
        ? (isArabic ? x.UpdateBy.NameAr : x.UpdateBy.NameEn)
        : (isArabic ? x.CreateBy?.NameAr : x.CreateBy?.NameEn),
                UpdateDate = x.UpdateDate.HasValue
        ? x.UpdateDate.Value.ToString()
        : x.CreateDate.ToString(),
            }).ToList();

            return mapper.Map<List<ActionConditionDTO>>(result);
        }


        public static string GetFieldValue(
      string type,
      string fieldValue,
      Guid? refId,
      bool isArabic,
      IServiceProvider serviceProvider)
        {
            var uow = serviceProvider.CreateScopedUow();
            if (refId == null) return fieldValue;

            var fieldvaluelist = fieldValue.Split(",");



            var fieldType = "";
            Guid? fielddropdowntype;

            var field = uow.GetRepository<Field>()
        .GetAll(x => x.Id == refId)
        .Include(x => x.FieldType)
        .FirstOrDefault();

            fieldType = field?.FieldType?.BackendName;
            fielddropdowntype = field?.DropDownTypeId;

            if (fieldType is "select2" or "dropdown" or "VacancySeat")
            {

                var dataSource = uow.GetRepository<DropDownType>()
            .GetAllNonDeleted()
            .Where(x => x.Id == fielddropdowntype)
            .Select(x => x.DataSourceTable)
            .FirstOrDefault();

                //if (!string.IsNullOrEmpty(dataSource))
                //    return GetDataFromTable(dataSource, isArabic,serviceProvider, fieldvaluelist);

                var result = uow.GetRepository<FieldDropDownValue>()
                    .GetAllNonDeleted()
                    .Where(x => x.DropDownTypeId == fielddropdowntype && fieldvaluelist.Contains(x.Id.ToString()))
                    .Select(c => LangSelector(isArabic, c.TitleAr, c.TitleEn))
                    .ToListAsync();
                return string.Join(", ", result.Result);
            }

            return fieldValue;
        }

        //private static string GetDataFromTable(string tableName, bool isArabic, IServiceProvider serviceProvider,string[] fieldvaluelist)

        //{

        //    var uow=serviceProvider.CreateScopedUow();

        //    var finalresult="";

        //    switch (tableName)
        //    {
        //        case "FinancialRegulations":
        //           var result = uow.GetRepository<FinRegulation>()
        //            .GetAllNonDeleted()
        //            .Where(x=> fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult= string.Join(", ", result);
        //            break;

        //        case "Major":
        //            result = uow.GetRepository<Major>()
        //           .GetAllNonDeleted()
        //           .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.IsMain == false)
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "MainMajor":
        //            result = uow.GetRepository<Major>()
        //           .GetAllNonDeleted()
        //           .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.IsMain == true)
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "MainDegree":
        //            result = uow.GetRepository<AcademicDegree>()
        //           .GetAllNonDeleted()
        //           .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.ParentDegreeId == null)
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "AcademicDegree":
        //            result = uow.GetRepository<AcademicDegree>()
        //           .GetAllNonDeleted()
        //           .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.ParentDegreeId != null)
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "SP.EnrollmentType":
        //            result = uow.GetRepository<EnrollmentType>()
        //           .GetAllNonDeleted()
        //          .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "Country" or "AllCountry" or "AccreditedCountry":
        //            result = uow.GetRepository<Country>()
        //           .GetAllNonDeleted()
        //          .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "BankBranch":
        //            result = uow.GetRepository<BankBranch>()
        //          .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "vp.EntityContract":
        //            result = uow.GetRepository<EntityContract>()
        //         .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "Vacancy":
        //            result = uow.GetRepository<Vacancy>()
        //        .GetAllNonDeleted()
        //        .Include(x => x.Major)
        //       .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.Major != null)
        //            .Select(c => c.Major != null?( isArabic ? c.Major.NameAr : c.Major.NameEn):"").ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "city":
        //            result = uow.GetRepository<City>()
        //          .GetAllNonDeleted()
        //        .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "AcademicYear" :
        //            result = uow.GetRepository<AcademicYear>()
        //         .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.IsCurrent==false)
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "CurrentAcademicYear":
        //            result = uow.GetRepository<AcademicYear>()
        //         .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.IsCurrent == true)
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "AccreditedUniversity":
        //            result = uow.GetRepository<AccreditedUniversity>()
        //         .GetAllNonDeleted()
        //         .Include(x => x.University)
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()) && x.University!=null)
        //            .Select(c => c.University!=null?( isArabic ? c.University.NameAr : c.University.NameEn):"").ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "Bank":
        //            result = uow.GetRepository<Bank>()
        //          .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;



        //        case "Currency":
        //            result = uow.GetRepository<Currency>()
        //          .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;

        //        case "DropDownTypes":
        //            result = uow.GetRepository<DropDownType>()
        //         .GetAllNonDeleted()
        //        .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.TitleAr : c.TitleEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "SP.Track":
        //            result = uow.GetRepository<Track>()
        //         .GetAllNonDeleted()
        //        .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "Programs":
        //            result = uow.GetRepository<SchProgram>()
        //         .GetAllNonDeleted()
        //        .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "RelationshipType":
        //            result = uow.GetRepository<RelationshipType>()
        //         .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //             finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "Sector":
        //            result = uow.GetRepository<Sector>()
        //         .GetAllNonDeleted()
        //        .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "Semester":
        //            result = uow.GetRepository<Semester>()
        //         .GetAllNonDeleted()
        //         .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.NameAr : c.NameEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;
        //        case "UserGender":
        //            result = uow.GetRepository<UserGender>()
        //         .GetAllNonDeleted()
        //        .Where(x => fieldvaluelist.Contains(x.Id.ToString()))
        //            .Select(c => isArabic ? c.TitleAr : c.TitleEn).ToListAsync();
        //            finalresult = string.Join(", ", result.Result);
        //            break;

        //    }

        //    return finalresult;
        //}

        public async Task<List<DropdownItem>> GetServicesForServiceAction()
        {
            using (var uow = serviceScopeFactory.CreateScopedUow())
            {
                var list = await uow.GetRepository<Service>()
                    .GetAllNonDeleted()
                    .Select(x => new DropdownItem
                    {
                        Id = x.Id,
                        NameAr = x.NameAr,
                        NameEn = x.NameEn,
                        OrderNo = x.OrderNo,
                        Type = "Service",
                    })
                    .OrderBy(x => x.OrderNo)
                    .ToListAsync();

                return list;
            }

        }
        public async Task<List<DropdownItem>> GetServiceActionTypes()
        {
            using (var uow = serviceScopeFactory.CreateScopedUow())
            {
                var list = await uow.GetRepository<ActionType>()
                .GetAllNonDeleted()
                .Select(x => new DropdownItem
                {
                    Id = x.Id,
                    NameAr = x.TitleAr,
                    NameEn = x.TitleEn,
                    OrderNo = x.OrderNo == null ? 0 : x.OrderNo.Value,
                    Type = "ActionType",
                    BackendName = x.BackendName
                })
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

                return list;
            }
        }

        public async Task<List<DropdownItem>> GetPartyTypesList()
        {
            using (var uow = serviceScopeFactory.CreateScopedUow())
            {
                var partyTypesList = new List<DropdownItem>();
                partyTypesList = await uow.GetRepository<PartyType>()
                                    .GetAllNonDeleted()
                                    .Select(x => new DropdownItem
                                    {
                                        Id = x.Id,
                                        NameAr = x.NameAr,
                                        NameEn = x.NameEn,
                                        Type = "PartyType",
                                        BackendName = x.BackendName ?? string.Empty
                                    })
                                    //.OrderBy(x => x.OrderNo)
                                    .ToListAsync();
                return partyTypesList;
            }
        }


        public async Task<List<DropdownItem>> GetTemplateDocsList(Guid serviceId)
        {
            using (var uow = serviceScopeFactory.CreateScopedUow())
            {
                var templateDocsList = new List<DropdownItem>();
                templateDocsList = await uow.GetRepository<TemplateDocument>()
                                    .GetAllNonDeleted()
                                    .Select(x => new DropdownItem
                                    {
                                        Id = x.Id,
                                        //NameAr = x.NameAr,
                                        //NameEn = x.NameEn,
                                        Type = "TemplateDoc",
                                    })
                                    //.OrderBy(x => x.OrderNo)
                                    .ToListAsync();

                return templateDocsList;
            }
        }

        public async Task<ServiceActionDTO> SaveServiceAction(ManageServiceActionDTO model)
        {
            if (model == null || model.action == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }


            bool validateObject = await srvApplicationBL.ValidateObject(model, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS);
            if (!validateObject)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            var mapper = await CreateMapperForAdmin<ServiceAction, ServiceActionDTO>();
            var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == model.action.ServiceId && x.IsFreez == true).ToListAsync();

            //            if (servicefreezecount.Count > 0)
            //            {
            //                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);
            //            }

            var entity = mapper.Map<ServiceAction>(model.action);

            // entity.BackendName = $"{model.action.ServiceId}_{model.action.NameEn}";
            entity.BackendName = await GenerateBackendName(model.action.NameEn, model.action.ServiceId, "A");
            var existBackendName = await uow
             .GetRepository<PartyType>()
                  .GetAllNonDeleted(x => x.BackendName == entity.BackendName)
                  .FirstOrDefaultAsync();

            if (existBackendName != null)
            {

                model.action.ResponseStatus = DBResult.Exist;
                return model.action;
            }
            if (!entity.IsInitialAction)
            {
                entity.AllowDraft = false;
            }
            PartyType obj = new PartyType();

            entity = uow.GetRepository<ServiceAction>().Insert(entity);

            if (model.action.ActionPartyTypeList != null)
            {
                foreach (var partyTypeId in model.action.ActionPartyTypeList)
                {
                    var entityDB = new ActionPartyType
                    {
                        ServiceActionId = entity.Id,
                        PartyTypeId = partyTypeId
                    };
                    uow.GetRepository<ActionPartyType>().Insert(entityDB);
                }
            }

            if (model.action.AssignActionPartyTypeList != null)
            {
                foreach (var partyTypeId in model.action.AssignActionPartyTypeList)
                {
                    var entityDB = new ActionAssignPartyType
                    {
                        EvaluationActionId = entity.Id,
                        PartyTypeId = partyTypeId
                    };
                    uow.GetRepository<ActionAssignPartyType>().Insert(entityDB);
                }
            }

            if (model.action.ActionShowLogPartyTypeList != null)
            {
                foreach (var partyTypeId in model.action.ActionShowLogPartyTypeList)
                {
                    var entityDB = new ActionShowLogPartyType
                    {
                        ServiceActionId = entity.Id,
                        PartytypeId = partyTypeId
                    };
                    uow.GetRepository<ActionShowLogPartyType>().Insert(entityDB);
                }
            }

            if (model.action.ActionTemplateDocList != null)
            {
                foreach (var templateDocId in model.action.ActionTemplateDocList)
                {
                    var entityDB = new ActionTemplateDoc
                    {
                        ServiceActionId = entity.Id,
                        TemplateDocId = templateDocId
                    };
                    uow.GetRepository<ActionTemplateDoc>().Insert(entityDB);
                }
            }

            await uow.CommitAsync();

            var result = mapper.Map<ServiceActionDTO>(entity);
            result.ResponseStatus = DBResult.Inserted;

            return result;

        }
        public async Task<ServiceActionDTO> UpdateServiceAction(ManageServiceActionDTO model)
        {
            if (model == null || model.action == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            if (model.action.Id is null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            bool validateObject = await srvApplicationBL.ValidateObject(model, ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS);
            if (!validateObject)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            var entity = await uow.GetRepository<ServiceAction>()
                                .GetAllNonDeleted()
                                .Where(x => x.Id == model.action.Id)
                                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
            }
            var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == entity.ServiceId && x.IsFreez == true).ToListAsync();

            if (servicefreezecount.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);
            }
            if (!model.action.IsInitialAction)
            {
                model.action.AllowDraft = false;
            }
            entity.NameAr = model.action.NameAr;
            entity.NameEn = model.action.NameEn;
            entity.IsConfirmationAction = model.action.IsConfirmationAction;
            entity.AllowDraft = model.action.AllowDraft;
            entity.ActionTypeId = model.action.ActionTypeId;
            entity.ServiceId = model.action.ServiceId;
            entity.IsActive = model.action.IsActive;
            entity.ConfirmationBodyAr = model.action.ConfirmationBodyAr;
            entity.ConfirmationBodyEn = model.action.ConfirmationBodyEn;
            entity.ConfirmationTitleAr = model.action.ConfirmationTitleAr;
            entity.ConfirmationTitleEn = model.action.ConfirmationTitleEn;
            entity.IsInitialAction = model.action.IsInitialAction;

            entity = uow.GetRepository<ServiceAction>().Update(entity);


            var all_actionPartyTypeList = uow.GetRepository<ActionPartyType>()
                                .GetAllNonDeleted()
                                .Where(x => x.ServiceActionId == entity.Id)
                                .ToList();
            var all_ActionAssignPartyTypeList = uow.GetRepository<ActionAssignPartyType>()
                                .GetAllNonDeleted()
                                .Where(x => x.EvaluationActionId == entity.Id)
                                .ToList();


            if (model.action.ActionPartyTypeList != null)
            {
                var actionPartyTypeListToBeDeleted = all_actionPartyTypeList
                      .Where(x => !model.action.ActionPartyTypeList.Contains(x.PartyTypeId))
                      .ToList();

                foreach (var item in actionPartyTypeListToBeDeleted)
                {
                    uow.GetRepository<ActionPartyType>().Delete(item);
                }

                var partyTypes = all_actionPartyTypeList.Select(x => x.PartyTypeId).ToList();
                var actionPartyTypeListToInserted = model.action.ActionPartyTypeList
                    .Where(x => !partyTypes.Contains(x))
                    .ToList();

                foreach (var PartyTypeId in actionPartyTypeListToInserted)
                {
                    var actionPartyType = new ActionPartyType
                    {
                        PartyTypeId = PartyTypeId,
                        ServiceActionId = model.action.Id.Value
                    };
                    uow.GetRepository<ActionPartyType>().Insert(actionPartyType);
                }
            }

            if (model.action.AssignActionPartyTypeList != null)
            {
                var AssignActionPartyTypeListListToBeDeleted = all_ActionAssignPartyTypeList
                      .Where(x => !model.action.AssignActionPartyTypeList.Contains(x.PartyTypeId))
                      .ToList();

                foreach (var item in AssignActionPartyTypeListListToBeDeleted)
                {
                    uow.GetRepository<ActionAssignPartyType>().Delete(item);
                }

                var partyTypes = all_ActionAssignPartyTypeList.Select(x => x.PartyTypeId).ToList();
                var ActionAssignPartyTypeToInserted = model.action.AssignActionPartyTypeList
                    .Where(x => !partyTypes.Contains(x))
                    .ToList();

                foreach (var PartyTypeId in ActionAssignPartyTypeToInserted)
                {
                    var ActionAssignPartyType = new ActionAssignPartyType
                    {
                        PartyTypeId = PartyTypeId,
                        EvaluationActionId = model.action.Id.Value
                    };
                    uow.GetRepository<ActionAssignPartyType>().Insert(ActionAssignPartyType);
                }
            }

            if (model.action.ActionShowLogPartyTypeList != null)
            {

                var all_actionShowLogPartyTypeList = uow.GetRepository<ActionShowLogPartyType>()
                                .GetAllNonDeleted()
                                .Where(x => x.ServiceActionId == entity.Id)
                                .ToList();


                var actionShowLogPartyTypeListToBeDeleted = all_actionShowLogPartyTypeList
                    .Where(x => !model.action.ActionShowLogPartyTypeList.Contains(x.PartytypeId)).ToList();

                foreach (var item in actionShowLogPartyTypeListToBeDeleted)
                {
                    uow.GetRepository<ActionShowLogPartyType>().Delete(item);
                }

                var partyTypes = all_actionShowLogPartyTypeList.Select(x => x.PartytypeId).ToList();
                var actionShowLogPartyTypeToInserted = model.action.ActionShowLogPartyTypeList.Where(x => !partyTypes.Contains(x))
                    .ToList();

                foreach (var PartyTypeId in actionShowLogPartyTypeToInserted)
                {
                    var actionShowLogPartyType = new ActionShowLogPartyType
                    {
                        PartytypeId = PartyTypeId,
                        ServiceActionId = model.action.Id.Value
                    };
                    uow.GetRepository<ActionShowLogPartyType>().Insert(actionShowLogPartyType);
                }
            }

            if (model.action.ActionTemplateDocList != null)
            {
                var all_actionTemplateDocList = uow.GetRepository<ActionTemplateDoc>()
                       .GetAllNonDeleted()
                       .Where(x => x.ServiceActionId == entity.Id)
                       .ToList();

                var actionTemplateDocListToBeDeleted = all_actionTemplateDocList
                       .Where(x => !model.action.ActionTemplateDocList.Contains(x.TemplateDocId)).ToList();

                foreach (var item in actionTemplateDocListToBeDeleted)
                {
                    uow.GetRepository<ActionTemplateDoc>().Delete(item);
                }

                var templateDocs = all_actionTemplateDocList.Select(x => x.TemplateDocId).ToList();
                var templateDocToInserted = model.action.ActionTemplateDocList.Where(x => !templateDocs.Contains(x))
                    .ToList();

                foreach (var templateDocId in templateDocToInserted)
                {
                    var actionShowLogPartyType = new ActionTemplateDoc
                    {
                        TemplateDocId = templateDocId,
                        ServiceActionId = model.action.Id.Value
                    };
                    uow.GetRepository<ActionTemplateDoc>().Insert(actionShowLogPartyType);
                }
            }


            await uow.CommitAsync();

            var mapper = await CreateMapperForAdmin<ServiceAction, ServiceActionDTO>();

            var result = mapper.Map<ServiceActionDTO>(entity);

            result.ResponseStatus = DBResult.Updated;

            return result;

        }

        public async Task<bool> DeleteServiceAction(Guid? Id)
        {
            if (Id is null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            var entity = await uow.GetRepository<ServiceAction>()
                                .GetAllNonDeleted()
                                .Where(x => x.Id == Id)
                                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
            }
            var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == entity.ServiceId && x.IsFreez == true).ToListAsync();

            if (servicefreezecount.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);
            }
            var existentity = await uow.GetRepository<ActionStatusConfiguration>()
                                .GetAllNonDeleted()
                                .Where(x => x.ServiceActionId == Id)
                                .ToListAsync();
            if (existentity.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ServiceActionUsed);
            }
            var ActionAssignPartyType = await uow.GetRepository<ActionAssignPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.EvaluationActionId == entity.Id)
                      .ToListAsync();
            if (ActionAssignPartyType.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ActionExistsActionAssignPartyType);
            }
            var ActionCondition = await uow.GetRepository<ActionCondition>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceActionId == entity.Id)
                      .ToListAsync();
            if (ActionCondition.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ActionExistsActionCondition);
            }
            var ActionPartyType = await uow.GetRepository<ActionPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceActionId == entity.Id)
                      .ToListAsync();
            if (ActionPartyType.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ActionExistsActionPartyType);
            }
            var ActionShowLogPartyType = await uow.GetRepository<ActionShowLogPartyType>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceActionId == entity.Id)
                      .ToListAsync();
            if (ActionShowLogPartyType.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ActionExistsActionShowLogPartyType);
            }


            var ActionTemplateDoc = await uow.GetRepository<ActionTemplateDoc>()
.GetAllNonDeleted()
                      .Where(x => x.ServiceActionId == entity.Id)
                      .ToListAsync();
            if (ActionTemplateDoc.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.ActionExistsActionTemplateDoc);
            }
            uow.GetRepository<ServiceAction>().Delete(entity);
            await uow.CommitAsync();

            return true;

        }

        public async Task<ServiceActionDetailsDTO> GetServiceActionDetails(Guid? ServiceActionId)
        {

            var result = new ServiceActionDetailsDTO { };
            if (ServiceActionId is null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            var entity = await uow.GetRepository<ServiceAction>()
                                .GetAllNonDeleted()
                                .Where(x => x.Id == ServiceActionId)
                                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
            }

            var service = await uow.GetRepository<Service>()
                                .GetAllNonDeleted()
                                .Where(x => x.Id == entity.ServiceId)
                                .FirstOrDefaultAsync();

            if (service == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
            }

            var mapper = await CreateMapperForAdmin<ServiceAction, ServiceActionDTO>();

            var ServiceAction = mapper.Map<ServiceActionDTO>(entity);
            result.PartyTypesList = new List<DropdownItem>();
            result.PartyTypesList = await GetPartyTypesList();

            result.TemplateDocsList = new List<DropdownItem>();
            result.TemplateDocsList = await GetTemplateDocsList(entity.ServiceId);

            ServiceAction.ActionPartyTypeList = new List<Guid>();
            ServiceAction.ActionPartyTypeList = await uow.GetRepository<ActionPartyType>()
                    .GetAllNonDeleted()
                    .Include(x => x.PartyType)
                    .Where(x => x.ServiceActionId == ServiceActionId)
                    .Where(x => x.PartyType != null && x.PartyType.IsDeleted == false)
                    .Select(x => x.PartyTypeId)
                    .ToListAsync();

            ServiceAction.AssignActionPartyTypeList = new List<Guid>();
            ServiceAction.AssignActionPartyTypeList = await uow.GetRepository<ActionAssignPartyType>()
                    .GetAllNonDeleted()
                    .Include(x => x.PartyType)
                    .Where(x => x.EvaluationActionId == ServiceActionId)
                    .Where(x => x.PartyType != null && x.PartyType.IsDeleted == false)
                    .Select(x => x.PartyTypeId)
                    .ToListAsync();

            ServiceAction.ActionShowLogPartyTypeList = new List<Guid>();
            ServiceAction.ActionShowLogPartyTypeList = await uow.GetRepository<ActionShowLogPartyType>()
                    .GetAllNonDeleted()
                    .Include(x => x.Partytype)
                    .Where(x => x.ServiceActionId == ServiceActionId)
                    .Where(x => x.Partytype != null && x.Partytype.IsDeleted == false)
                    .Select(x => x.PartytypeId)
                    .ToListAsync();

            ServiceAction.ActionTemplateDocList = new List<Guid>();
            ServiceAction.ActionTemplateDocList = await uow.GetRepository<ActionTemplateDoc>()
                    .GetAllNonDeleted()
                    .Include(x => x.TemplateDoc)
                    .Where(x => x.ServiceActionId == ServiceActionId)
                    .Where(x => x.TemplateDoc != null && x.TemplateDoc.IsDeleted == false)
                    .Select(x => x.TemplateDocId)
                    .ToListAsync();

            result.ServiceAction = ServiceAction;
            return result;
        }

        public async Task<ActionFieldTreeDTO> GetActionFieldTree(Guid? actionId, Guid? stepId)
        {
            if (actionId is null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidRequest);
            }

            var entity = await uow.GetRepository<ServiceAction>()
                                .GetAllNonDeleted()
                                .Where(x => x.Id == actionId)
                                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.NoDataFound);
            }

            var service = await uow.GetRepository<Service>()
                                .GetAllNonDeleted()
                                .Where(x => x.Id == entity.ServiceId)
                                .FirstOrDefaultAsync();

            if (service == null)
            {
                throw new BusinessException("Service not found");
            }

            var formGroups = await uow.GetRepository<FormGroup>()
                .GetAllNonDeleted(x => x.ServiceId == service.Id)
                .Include(x => x.Fields)
                .Include(x => x.FormGroupType)
                .Where(x => x.FormGroupType != null && x.FormGroupType.BackendName != "List")
                .Select(x => new ActionFormGroupDTO
                {
                    id = x.Id,
                    text = _requestInfo.Lang == "ar" ? x.TitleAr : x.TitleEn,
                    children = x.Fields
                                    .Where(y => y.IsDeleted == false)
                                    .Select(y => new ActionFieldFormGroupDTO
                                    {
                                        id = y.Id,
                                        value = y.Id.ToString(),
                                        text = _requestInfo.Lang == "ar" ? y.TitleAr : y.TitleEn,
                                        parentId = y.FormGroupId,
                                        ParentName = _requestInfo.Lang == "ar" ? x.TitleAr : x.TitleEn
                                    }).ToList(),
                })
                .ToListAsync();


            var fieldIdsList = formGroups.SelectMany(x => x.children).Select(x => x.id).ToList();








            var result = new ActionFieldTreeDTO { ActionId = actionId.Value, FormGroups = formGroups };

            return result;
        }




    }
}



