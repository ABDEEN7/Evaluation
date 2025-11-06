using Azure.Core;
using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Globalization;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
#pragma warning disable CS8620

    public class SrvAction (SrvDropdown SrvDropdown, SrvUser SrvUser, SrvField SrvField, IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {

        public async Task<ServiceAction?> GetActionByBackendNameAsync( Guid serviceId,string backendName,bool includeActionFields = false)
        {
            using var scope = serviceScopeFactory.CreateScopedUow();

            IQueryable<ServiceAction> query = scope
                .GetRepository<ServiceAction>()
                .GetAllQueryFiltered(x => x.BackendName == backendName && x.ServiceId == serviceId)
                .Include(x => x.ActionType)
                .Include(x => x.ActionFields)
            .ThenInclude(asf => asf.ActionFieldAttribute);

            if (includeActionFields)
            {
                query = query
                    .Include(x => x.ActionFields)
                    .Include(x => x.ActionFields)
                        .ThenInclude(asf => asf.Field)
                            .ThenInclude(f => f!.FieldType)
                    .Include(x => x.ActionFields)
                        .ThenInclude(asf => asf.Field)
                            .ThenInclude(f => f!.MappingField)
                    .Include(x => x.ActionFields)
                        .ThenInclude(asf => asf.Field)
                            .ThenInclude(f => f!.FieldAttributeValues);
            }

            var action = await query
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return action;
        }
        public async Task<ServiceAction?> GetInitialActionAsync(Guid serviceId, string actionBackendName)
        {
            var action = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ServiceAction>()
                .GetAllQueryFiltered()
                .Include(x => x.ActionType)
                .Include(x => x.ActionFields)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ServiceId == serviceId && c.IsInitialAction && (actionBackendName == null || c.BackendName == actionBackendName));

            return action;
        }
        public async Task<List<ActionField>> GetActionFieldsAsync(Guid actionId)
        {
            var fields = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ActionField>()
                .GetAllQueryFiltered(x => x.ServiceActionId == actionId)
                .Include(x => x.Field)
                .AsNoTracking()
                .ToListAsync();

            return fields; 
        }
        public async Task<List<ActionTemplateDoc>> GetActionDocumentsAsync(Guid actionId)
        {
            var documents = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ActionTemplateDoc>()
                .GetAllQueryFiltered(x => x.ServiceActionId == actionId)
                .AsNoTracking()
                .ToListAsync();

            return documents;
        }
        public async Task<List<Evaluation.SharedHelper.Models.Api.PartyTypeDTOs.PartyTypeDTO>> GetActionPartyTypesAsync(Guid actionId)
        {
            var partyTypes = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ActionPartyType>()
                .GetAllQueryFiltered(x => x.ServiceActionId == actionId)
                .Include(x => x.PartyType)
                .AsNoTracking()
                .ToListAsync();

            return partyTypes.Select(pt => MapToPartyTypeDTO(pt.PartyType!)).ToList();
        }
       
        #region ValidateActionAndActionField
        public async Task<IList<FieldValueDTO>> ValidateActionAndActionFieldAsync(ServiceRequest? application,Guid StudentId, IList<FieldValueDTO> allFields, string ActionRemarks, List<IFormFile> otherAttachement, Service serviceObj, Guid applicationStatusId, ServiceAction actionObj, List<FileFieldDTO> fileFields, bool saveAsDraft = false)
        {
            string Lang = requestInfo.Lang;
            if (null == serviceObj && null == actionObj)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.IncompleteRequest);
            }

            var studentTask = SrvUser.GetByStudentIDActiveNonDeleted(StudentId);

            var actionTask = GetActionByBackendNameAsync(serviceObj!.Id, actionObj.BackendName);

            var attributesTask = serviceScopeFactory.CreateScopedUow()
                                                    .GetRepository<FieldAttributeValue>()
                                                    .GetAllQueryFiltered()
                                                    .Include(c => c.Field!.FieldType)
                                                    .Include(c => c.Field!.FieldViewConditions)
                                                    .AsSplitQuery()
                                                    .Where(c => allFields.Select(f => f.FieldId).ToList().Contains(c.FieldId)).AsNoTracking()
                                                    .ToListAsync();

            var ActionFieldAttributesTask = serviceScopeFactory.CreateScopedUow()
                                                .GetRepository<ActionField>()
                                                .GetAllQueryFiltered()
                                                .Where(c => allFields.Select(f => f.FieldId).Contains(c.FieldId) && c.ServiceActionId == actionObj.Id)
                                                .SelectMany(c => c.ActionFieldAttribute)
                                                .ToListAsync();

            var actionStatusConfigTask = GetActionConfigurationAsync(actionObj.Id, applicationStatusId);

            await Task.WhenAll(actionTask, actionStatusConfigTask);
            var actionDb = actionTask.Result;
            var actionStatusConfig = actionStatusConfigTask.Result;

            if (actionDb == null)
            {
                throw new BusinessException(ExceptionMessage.InvalidRequest);
            }

            if (actionStatusConfig == null)
            {
                throw new BusinessException(ExceptionMessage.lbl_you_are_not_authorized_to_perform_this_action_in_the_current_status);
            }
            var validateFieldsTask = ValidateActionFieldsAsync(allFields, actionDb.Id, actionDb.ActionTypeId, application?.Id);
            var validateRemarksTask = ValidateRemaksAndOtherAttachementAsync(actionStatusConfig, ActionRemarks, otherAttachement);

            if (!saveAsDraft && actionDb.ActionType!.IsRequiredValidation == true)
            {
                var mergedAttributes = MergeAttributes(attributesTask.Result, ActionFieldAttributesTask.Result, Lang);

                var editableFieldIds = await serviceScopeFactory.CreateScopedUow()
                    .GetRepository<ActionField>()
                    .GetAllQueryFiltered()
                    .AsNoTracking()
                    .Where(c => c.ServiceActionId == actionDb.Id && c.IsEditable)
                    .Select(c => c.FieldId)
                    .ToListAsync();

                var enabledFieldIds = mergedAttributes
                    .Where(c => !string.Equals(c.AttributeKey, "readonly", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(c.AttributeKey, "disabled", StringComparison.OrdinalIgnoreCase))
                    .Select(c => c.FieldId)
                    .ToHashSet();

                var fieldsToValidate = allFields
                    .Where(f => f.FieldId.HasValue &&
                                editableFieldIds.Contains(f.FieldId.Value) &&
                                enabledFieldIds.Contains(f.FieldId.Value))
                    .ToList();

                var fieldAttributesToValidate = mergedAttributes
                    .Where(attr => fieldsToValidate.Select(f => f.FieldId).Contains(attr.FieldId))
                    .ToList();

                var student = await studentTask;
                var validationErrorsTask = ValidateFieldsAttributesAsync(fieldsToValidate, allFields, fileFields, fieldAttributesToValidate, student, Lang, application?.ScholarshipId);

                await Task.WhenAll(validationErrorsTask, validateRemarksTask);

                var (validationErrors, updatedFields) = await validationErrorsTask;
                if (validationErrors.Any(e => !string.IsNullOrWhiteSpace(e.error)))
                    throw new BusinessException(validationErrors);

                ApplyUpdatedFields(allFields, updatedFields);
            }
            else if (saveAsDraft)
            {
                var filledFields = allFields.Where(f => f.FieldId.HasValue && !string.IsNullOrWhiteSpace(f.Value) && f.Value!="[]").ToList();

                if (filledFields.Any())
                {
                    var mergedAttributes = MergeAttributes(attributesTask.Result, ActionFieldAttributesTask.Result, Lang);
                    var fieldAttributesToValidate = mergedAttributes
                        .Where(attr => filledFields.Select(f => f.FieldId).Contains(attr.FieldId))
                        .ToList();

                    var student = await studentTask;
                    var validationErrorsTask = ValidateFieldsAttributesAsync(filledFields, allFields, fileFields, fieldAttributesToValidate, student, Lang, application.ScholarshipId);

                    await Task.WhenAll(validationErrorsTask, validateRemarksTask);

                    var (validationErrors, updatedFields) = await validationErrorsTask;
                    if (validationErrors.Any(e => !string.IsNullOrWhiteSpace(e.error)))
                        throw new BusinessException(validationErrors);

                    ApplyUpdatedFields(allFields, updatedFields);
                }
                else
                {
                    await validateRemarksTask;
                }
            }
            else
            {
                await validateRemarksTask;
            }

           
            if (await validateFieldsTask)
            {
                throw new BusinessException(ExceptionMessage.lblSomeFieldsCannotBePartOfAction);
            }
            return allFields;
        }

        private static List<FieldAttributeValue> MergeAttributes(List<FieldAttributeValue> attributes, List<ActionFieldAttribute> ActionFieldAttributes, string lang)
        {
            var actionAttrs = ActionFieldAttributes
                .Select(attr => new FieldAttributeValue
                {
                    AttributeKey = attr.AttributeKey,
                    AttributeValue = attr.AttributeValue,
                    Description = lang == "ar" ? attr.MessageAr : attr.MessageEn,
                    FieldId = attr.FieldId,
                    Field = attr.Field,
                });

            var merged = attributes
                .Where(attr => !ActionFieldAttributes.Any(a => a.FieldId == attr.FieldId && a.AttributeKey == attr.AttributeKey))
                .Select(attr => new FieldAttributeValue
                {
                    AttributeKey = attr.AttributeKey,
                    AttributeValue = attr.AttributeValue,
                    Description = lang == "ar" ? attr.MessageAr : attr.MessageEn,
                    FieldId = attr.FieldId,
                    Field = attr.Field,

                });

            return actionAttrs.Concat(merged)
                .GroupBy(a => new { a.FieldId, a.AttributeKey })
                .Select(g => g.First())
                .ToList();
        }

        private static void ApplyUpdatedFields(IList<FieldValueDTO> allFields, IEnumerable<FieldValueDTO> updatedFields)
        {
            foreach (var updated in updatedFields)
            {
                var existing = allFields.FirstOrDefault(f => f.FieldId == updated.FieldId);
                if (existing != null)
                    existing.Value = updated.Value;
            }
        }
        public async Task<ActionStatusConfiguration?> GetActionConfigurationAsync(Guid actionId, Guid currentStatusId)
        {
          var ActionConfiguration= await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ActionStatusConfiguration>().GetAllQueryFiltered().Include(x=>x.Notifications)
                .FirstOrDefaultAsync(c => c.ServiceActionId == actionId && c.CurrentStatusId == currentStatusId);

            return ActionConfiguration;
        }
        private async Task<bool> ValidateActionFieldsAsync(IList<FieldValueDTO> fields, Guid actionId, Guid? ActionTypeId, Guid? applicationId)
        {
            if (ActionTypeId == ActionTypeIds.RequestDataChange || ActionTypeId == ActionTypeIds.SubmitMissingData) { return false; }
            else
            {
                var MissingField = serviceScopeFactory.CreateScopedUow()
                                                              .GetRepository<ServiceRequestFieldsValue>()
                                                              .GetAllQueryFiltered()
                                                              .AsNoTracking()
                                                              .Where(x => x.ServiceRequestId == applicationId && x.IsMissing == false)
                                                              .Select(c => c.FieldId).ToList();



                var actionFieldIdsList = serviceScopeFactory.CreateScopedUow()
                                                            .GetRepository<ActionField>()
                                                            .GetAllQueryFiltered()
                                                            .AsNoTracking()
                                                            .Where(c => c.ServiceActionId == actionId)
                                                            .Select(c => c.FieldId);

                var invalidFieldsList = fields.Where(c => !actionFieldIdsList.Contains(c.FieldId!.Value) && !MissingField.Contains(c.FieldId.Value)).ToList();

                return invalidFieldsList.Any();
            }
        }
        public async Task<List<DAL.Models.FormBuilder.Field?>> GetActionMappedFieldsAsync(Guid actionId, string RelatedFieldsGroupBackendName)
        {

            var actionMappedFieldIdsList = await serviceScopeFactory
                                            .CreateScopedUow()
                                            .GetRepository<DAL.Models.FormBuilder.ActionField>()
                                            .GetAllQueryFiltered()
                                            .Include(c => c.Field)
                                            .Include(c => c.Field!.MappingSystemField)
                                            .Include(c => c.Field!.MappingSystemField!.RelatedFieldsGroup)
                                            .AsSplitQuery()
                                            .AsNoTracking()
                                            .Where(c =>  c.ServiceActionId == actionId)
                                            .Select(c => c.Field)
                                            .ToListAsync();

            return actionMappedFieldIdsList;

        }
        public async Task<(List<FieldErrorDTO> errors, IList<FieldValueDTO> updatedFields)> ValidateFieldsAttributesAsync( IList<FieldValueDTO> fields, IList<FieldValueDTO> allFields, IList<FileFieldDTO> files, List<FieldAttributeValue> fullAttributes, StudentUser student, string lang,Guid? ScholarshipId)
        {
            var listFieldsTask = ValidateListFieldsAsync(fields, allFields, files, student, lang,ScholarshipId);
            var simpleFieldsTask = ValidateSimpleFieldsAsync(fields, allFields, files, fullAttributes, student, lang);

            await Task.WhenAll(listFieldsTask, simpleFieldsTask);

            var (listFieldErrors, updatedFields) = await listFieldsTask;
            var simpleFieldErrors = await simpleFieldsTask;

            var allErrors = listFieldErrors.Concat(simpleFieldErrors).ToList();

            if (allErrors.Any())
            {
                throw new BusinessException(allErrors);
            }

            return (allErrors, updatedFields);
        }
        private async Task<(List<FieldErrorDTO> errors, IList<FieldValueDTO> updatedFields)> ValidateListFieldsAsync(IList<FieldValueDTO> fields, IList<FieldValueDTO> allFields, IList<FileFieldDTO> files,StudentUser? Student, string lang,Guid? ScholarshipId)
        {
            var validator = new FieldValidatorBL(cacheDataProvider);
            var errors = new List<FieldErrorDTO>();
            var fieldsIds = fields.Select(x => x.FieldId).ToList();

            var FieldwithJasonSchema = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<DAL.Models.FormBuilder.Field>()
                .GetAllQueryFiltered(c => fieldsIds.Contains(c.Id))
                .Include(c => c.FieldAttributeValues)
                .Include(c => c.MappingSystemField)
                .AsSplitQuery()
                .Where(c => c.FormGroupListId != null && c.MappingSystemFieldId != null)
                .ToListAsync();

            if (FieldwithJasonSchema.Any())
            {
                foreach (var item in FieldwithJasonSchema)
                {
                    var fildList = await SrvField.GetFieldListByFieldId(item.Id);
                    var FieldValue = fields.FirstOrDefault(x => x.FieldId == item.Id)?.Value;
                    var listDicts = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(FieldValue!) ?? new();

                    var notEqualErrors =  validator.ValidateNotEqualListFields(listDicts, fildList, Student, lang);
                    errors.AddRange(WrapErrors(item.Id, notEqualErrors));

                    foreach (var dic in listDicts)
                    {
                        foreach (var keyValue in dic.ToList())
                        {
                            var field = fildList.FirstOrDefault(x => x.BackendName == keyValue.Key);
                            if (field == null)
                                continue;

                            bool shouldValidate = true;

                            if (field.FieldViewConditions?.Any() == true)
                            {
                                var conditionTasks = field.FieldViewConditions.Select(async condition =>
                                {
                                    var parentField = allFields.FirstOrDefault(f => f.FieldId == condition.ParentFieldId);
                                    return await ValidatedConditionAsync(parentField?.Value, condition.operators, condition.FieldValue);
                                });

                                var results = await Task.WhenAll(conditionTasks);
                                shouldValidate = results.All(result => result); 
                            }

                            if (!shouldValidate)
                                continue;

                            var tempField = new FieldValueDTO
                            {
                                FieldName = lang == "ar" ? field.TitleAr : field.TitleEn,
                                Type = field.FieldType!.BackendName,
                                Value = keyValue.Value?.ToString(),
                                FieldId = field.Id
                            };

                            var attributes = field.FieldAttributeValues?.Select(a => new FieldAttributeValue
                            {
                                AttributeKey = a.AttributeKey,
                                AttributeValue = a.AttributeValue,
                                MessageAr = a.MessageAr,
                                MessageEn = a.MessageEn,
                                FieldId = field.Id,
                            }).ToList();

                            if (attributes != null)
                            {
                                errors.AddRange(WrapErrors(field.Id, validator.ValidateRegex(tempField, attributes, lang)));
                                errors.AddRange(WrapErrors(field.Id, validator.ValidateRequired(tempField, attributes, lang, files)));

                                switch (tempField.Type.ToLower())
                                {
                                    case "text":
                                    case "jqte":
                                    case "textarea":
                                        errors.AddRange(WrapErrors(field.Id, validator.ValidateMaxLength(tempField, attributes, lang)));
                                        errors.AddRange(WrapErrors(field.Id, validator.ValidateMinLength(tempField, attributes, lang)));
                                        break;
                                    case "number":
                                        errors.AddRange(WrapErrors(field.Id, validator.ValidateNumber(tempField, attributes, lang)));
                                        break;
                                    case "date":
                                    case "datetime":
                                        errors.AddRange(WrapErrors(field.Id, validator.ValidateDate(tempField, attributes, lang)));
                                        break;
                                    case "file":
                                    case "filev2":
                                        errors.AddRange(WrapErrors(field.Id, validator.ValidateFile(tempField, files, attributes, lang)));
                                        break;
                                }
                            }
                        }
                    }

                    if (item.MappingSystemField?.BackendName == MappingSystemField.SchCompanion)
                    {
                        var companions = await SrvField.ReturnEntityListAsync<SchCompanion>(fildList, FieldValue!, item.Id, "SchCompanion_");

                        if (companions.Any())
                        {
                            var minAge = item.FieldAttributeValues?.FirstOrDefault(x => x.AttributeKey == "MinAge")?.AttributeValue;
                            var maxAge = item.FieldAttributeValues?.FirstOrDefault(x => x.AttributeKey == "MaxAge")?.AttributeValue;

                            int? minA = int.TryParse(minAge, out var parsedMin) ? parsedMin : (int?)null;
                            int? maxA = int.TryParse(maxAge, out var parsedMax) ? parsedMax : (int?)null;

                            if (minA.HasValue || maxA.HasValue)
                            {
                                errors.AddRange(WrapErrors(item.Id, await ValidateMinAndMaxAgeAsync(companions, minA, maxA, lang)));
                            }

                            if (item.FieldAttributeValues!.Any(x => x.AttributeKey == "MustHasntActiveScholarship"))
                                errors.AddRange(WrapErrors(item.Id, await ValidateActiveScholarshipAsync(companions, lang)));

                            if (!errors.Any(x => x.fieldId == item.Id))
                            {
                                var valTask = ValidateCompanionListAsync(companions, lang);
                                var moiTask = item.FieldAttributeValues!.Any(x => x.AttributeKey == "MOIData") ? CheckAndUpdateMOIAsync(companions, lang) : Task.FromResult(new List<string>());
                                var qidTask = item.FieldAttributeValues!.Any(x => x.AttributeKey == "IsUniqueQID") ? ValidateUniqueQidAsync(companions, lang) : Task.FromResult(new List<string>());

                                await Task.WhenAll(valTask, moiTask, qidTask);

                                errors.AddRange(WrapErrors(item.Id, await valTask));
                                errors.AddRange(WrapErrors(item.Id, await moiTask));
                                errors.AddRange(WrapErrors(item.Id, await qidTask));
                            }

                            if (!errors.Any(x => x.fieldId == item.Id) && item.FieldAttributeValues!.Any(x => x.AttributeKey == "MOIData") )
                                fields.FirstOrDefault(x => x.FieldId == item.Id)!.Value = await UpdateCompanionListJsonAsync(companions, item.Id, FieldValue);
                        }
                    }

                    if (item.MappingSystemField?.BackendName == MappingSystemField.SchGuarantor)
                    {
                        var guarantorList = await SrvField.ReturnEntityListAsync<SchGuarantor>(fildList, FieldValue, item.Id, "SchGuarantor_");

                        if (guarantorList.Any())
                        {
                            var qidCheckTask = item.FieldAttributeValues!.Any(x => x.AttributeKey == "IsUniqueQID")
                                ? ValidateUniqueQidAsync(guarantorList, lang)
                                : Task.FromResult(new List<string>());

                            var moiTask = item.FieldAttributeValues.Any(x => x.AttributeKey == "MOIData")
                                ? CheckAndUpdateMOIAsync(guarantorList, lang)
                                : Task.FromResult(new List<string>());

                            await Task.WhenAll(qidCheckTask, moiTask);

                            errors.AddRange(WrapErrors(item.Id, await qidCheckTask));
                            errors.AddRange(WrapErrors(item.Id, await moiTask));

                            if (!errors.Any(e => e.fieldId == item.Id) && item.FieldAttributeValues!.Any(x => x.AttributeKey == "MOIData"))
                            {
                                fields.FirstOrDefault(x => x.FieldId == item.Id)!.Value = await UpdateGuarantorListJsonAsync(guarantorList, item.Id, FieldValue);
                            }
                        }
                    }

                    if (item.MappingSystemField?.BackendName == MappingSystemField.SchBankAccount)
                    {
                        var bankAccounts = await SrvField.ReturnEntityListAsync<BankAccount>(fildList, FieldValue!, item.Id, "BankAccount_");

                        if (bankAccounts.Any())
                        {
                            var defaultBankErrors = await validateDefaultBankAccountAsync(bankAccounts, lang);
                            var ibanErrors = await validateUniqueIbanNumberAsync(bankAccounts, Student.Id, lang);

                            errors.AddRange(WrapErrors(item.Id, defaultBankErrors));
                            errors.AddRange(WrapErrors(item.Id, ibanErrors));

                        }
                    }

                    if (item.MappingSystemField?.BackendName == MappingSystemField.SchSemester)
                    {
                        var semesterList = await SrvField.ReturnEntityListAsync<SchSemester>(fildList, FieldValue!, item.Id, "SchSemester_");

                        if (semesterList.Any())
                        {
                            var semesterErrors = await validateUniqueSemesterYearAsync(semesterList, lang);
							var deletedErrors = await validateDeletedSemestersAsync(ScholarshipId,FieldValue!, lang);
                            errors.AddRange(WrapErrors(item.Id, semesterErrors));
							errors.AddRange(WrapErrors(item.Id, deletedErrors));
						}
                    }
					if (item.MappingSystemField?.BackendName == MappingSystemField.SchAward && ScholarshipId.HasValue)
					{
						var deletedErrors = await validateDeletedAwardsAsync(ScholarshipId, FieldValue!, lang);
						errors.AddRange(WrapErrors(item.Id, deletedErrors));
					}
					if (item.FieldAttributeValues!.Any(x => x.AttributeKey == "MOIData") && item.MappingSystemField!.IsCoreColumn == false)
                    {
                        var qidField = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "CheckQIDMOI"));
                        var expiryField = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "CheckQIDExpiryMOI"));
                        var GenderListTask = serviceScopeFactory.CreateScopedUow()
                                      .GetRepository<UserGender>()
                                      .GetAllQueryFiltered()
                                       .ToListAsync();
                        if (qidField != null && expiryField != null)
                        {
                            foreach (var dic in listDicts)
                            {
                                var qid = dic.ToList().FirstOrDefault(x => x.Key == qidField.Id.ToString()).Value?.ToString();
                                var qidExpiryString = dic.FirstOrDefault(x => x.Key == expiryField.Id.ToString()).Value?.ToString();
                                DateOnly? qidExpiryDate = DateOnly.TryParseExact(qidExpiryString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnlyValue) ? dateOnlyValue : null;

                                string message = $"MOIData contact details , ExpiryDate:{qidExpiryDate}";
                                loggingServices.WriteLineToFile(true, message);
                                if (!string.IsNullOrEmpty(qid) && qidExpiryDate.HasValue)
                                {
                                    var moiMessageRequest = new MOIMessageRequest { QID = qid, QIDExpiryDate = qidExpiryDate.Value };
                                    var moiData = await mOIServices._GetPersonalInfo(moiMessageRequest);
                                    var GenderList = await GenderListTask;

                                    if (moiData != null)
                                    {
                                        var NameAr = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "MOIData" && a.AttributeValue == "NameAr"));
                                        var NameEn = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "MOIData" && a.AttributeValue == "NameEn"));
                                        var nationalityCode = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "MOIData" && a.AttributeValue == "nationalityCode"));
                                        var Gender = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "MOIData" && a.AttributeValue == "Gender"));
                                        var dobfield = fildList.FirstOrDefault(x => x.FieldAttributeValues!.Any(a => a.AttributeKey == "MOIData" && a.AttributeValue == "dob"));

                                        var ArabicName = string.Join(" ", new List<string> { moiData.ArabicName1, moiData.ArabicName2, moiData.ArabicName3, moiData.ArabicName4, moiData.ArabicName5 }.Where(n => !string.IsNullOrEmpty(n)));
                                        var fullNameEnProperty = string.Join(" ", new List<string> { moiData.EnglishName1, moiData.EnglishName2, moiData.EnglishName3, moiData.EnglishName4, moiData.EnglishName5 }.Where(n => !string.IsNullOrEmpty(n)));

                                        var nationalityId = await SrvDropdown.GetCountryBycode(moiData.NationalityCode);
                                        var nationalityCodeProperty = nationalityId?.Id;
                                        var genderId = moiData.Gender?.ToUpper() == "FEMALE" ? GenderList?.FirstOrDefault(x => x.TitleEn == "Female")?.Id : GenderList?.FirstOrDefault(x => x.TitleEn == "Male")?.Id;
                                        DateOnly? dobValue = null;
                                        if (!string.IsNullOrWhiteSpace(moiData.DateOfBirth) &&
                                         DateTime.TryParseExact(moiData.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                                        {
                                            dobValue = DateOnly.FromDateTime(dob);
                                        }

                                        if (NameAr != null && dic.ContainsKey(NameAr.Id.ToString()))
                                        {
                                            dic[NameAr.Id.ToString()] = ArabicName;
                                        }

                                        if (NameEn != null && dic.ContainsKey(NameEn.Id.ToString()))
                                        {
                                            dic[NameEn.Id.ToString()] = fullNameEnProperty;
                                        }

                                        if (nationalityCode != null && dic.ContainsKey(nationalityCode.Id.ToString()))
                                        {
                                            dic[nationalityCode.Id.ToString()] = nationalityCodeProperty!;
                                        }

                                        if (Gender != null && dic.ContainsKey(Gender.Id.ToString()))
                                        {
                                            dic[Gender.Id.ToString()] = genderId!;
                                        }

                                        if (dobfield != null && dic.ContainsKey(dobfield.Id.ToString()) && dobValue.HasValue)
                                        {
                                            dic[dobfield.Id.ToString()] = dobValue.Value;
                                        }

                                        var updatedJson = Newtonsoft.Json.JsonConvert.SerializeObject(new List<Dictionary<string, object>> { dic });

                                        var fieldToUpdate = fields.FirstOrDefault(x => x.FieldId == item.Id);
                                        if (fieldToUpdate != null)
                                        {
                                            fields!.FirstOrDefault(x => x.FieldId == item.Id)!.Value = updatedJson;
                                        }
                                    }
                                    else
                                    {
                                        // If MOI data is not found
                                        var errorMessage = lang == "ar"
                                            ? $"{qid} غير موجود في سجل وزارة الداخلية."
                                            : $"{qid} not found in MOI records.";
                                        var errorsList = new List<string>();
                                        errorsList.Add(errorMessage);
                                        errors.AddRange(WrapErrors(item.Id, errorsList));
                                    }
                                }
                                else
                                {
                                    // Error if QID or expiry date is invalid
                                   var mess=lang == "ar" ? "رقم الهوية أو تاريخ انتهاء الهوية غير صالح." : "Invalid QID or Expiry Date.";
                                    var errorsList = new List<string>();
                                    errorsList.Add(mess);
                                    errors.AddRange(WrapErrors(item.Id, errorsList));
                                }
                            }
                        }
                        else
                        {
                            // Error if "CheckQIDMOI" or "CheckQIDExpiryMOI" attributes are not found in the field list
                            var mess = lang == "ar" ? "الحقول المطلوبة للتحقق من رقم الهوية غير موجودة." : "Required fields for QID verification not found.";
                            var errorsList = new List<string>();
                            errorsList.Add(mess);
                            errors.AddRange(WrapErrors(item.Id, errorsList));
                        }
                    }


                }
            }

            return (errors, fields);
        }
        private async Task<List<FieldErrorDTO>> ValidateSimpleFieldsAsync(IList<FieldValueDTO> fields, IList<FieldValueDTO> allFields, IList<FileFieldDTO> files, List<FieldAttributeValue> fullAttributes, StudentUser? Student, string lang)
        {
            var validator = new FieldValidatorBL(cacheDataProvider);
            var errors = new List<FieldErrorDTO>();

            foreach (var item in fields)
            {
                var attributes = fullAttributes.Where(x => x.FieldId == item.FieldId).ToList();
                var field = attributes.FirstOrDefault()?.Field;

                if (field != null)
                {
                    bool shouldValidate = true;

                    // Check if field has conditional visibility
                    if (field.FieldViewConditions?.Any() == true)
                    {
                        // AND logic: all conditions must be true
                        foreach (var condition in field.FieldViewConditions)
                        {
                            var parentField = allFields.FirstOrDefault(f => f.FieldId == condition.ParentFieldId);
                            bool result = await ValidatedConditionAsync(parentField?.Value, condition.operators, condition.FieldValue);

                            if (!result)
                            {
                                shouldValidate = false;
                                break; // Short-circuit if any condition fails
                            }
                        }
                    }

                    if (shouldValidate)
                    {
                        item.FieldName = lang == "ar" ? field.TitleAr : field.TitleEn;
                        item.Type = field.FieldType!.NameEn;
                        item.FieldTooltip = lang == "ar" ? field.InfoAr : field.InfoEn;

                        errors.AddRange(WrapErrors(item.FieldId, validator.ValidateRegex(item, attributes, lang)));
                        errors.AddRange(WrapErrors(item.FieldId, validator.ValidateRequired(item, attributes, lang, files)));

                        switch (item.Type.ToLower())
                        {
                            case "text":
                            case "jqte":
                            case "textarea":
                                errors.AddRange(WrapErrors(item.FieldId, validator.ValidateMaxLength(item, attributes, lang)));
                                errors.AddRange(WrapErrors(item.FieldId, validator.ValidateMinLength(item, attributes, lang)));
                                break;

                            case "number":
                                errors.AddRange(WrapErrors(item.FieldId, validator.ValidateNumber(item, attributes, lang)));
                                break;

                            case "date":
                            case "datetime":
                                errors.AddRange(WrapErrors(item.FieldId, validator.ValidateDate(item, attributes, lang)));
                                break;

                            case "file":
                            case "filev2":
                                errors.AddRange(WrapErrors(item.FieldId, validator.ValidateFile(item, files, attributes, lang)));
                                break;
							case "list":
								errors.AddRange(WrapErrors(item.FieldId,await validator.ValidateList(item, files, attributes, lang)));
								break;
						}
                    }
                }

            }

            var notEqualErrors =  validator.ValidateNotEqualFields(fields, fullAttributes, Student!, lang);
            errors.AddRange(WrapErrors(null, notEqualErrors));

            return errors;
        }
        private List<FieldErrorDTO> WrapErrors(Guid? fieldId, IList<string> messages)
        {
            return messages.Select(msg => new FieldErrorDTO { fieldId = fieldId, error = msg }).ToList();
        }
        public async Task<List<string>> CheckAndUpdateMOIAsync<TEntity>(List<TEntity> entities, string lang) where TEntity : EntityBase, new()
        {
           
            var errors = new List<string>();

            if (entities == null || !entities.Any())
                return errors;
            var GenderListTask =  serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<UserGender>()
                                       .GetAllQueryFiltered()
                                        .ToListAsync();
            foreach (var entity in entities)
            {
                var qidProperty = entity.GetType().GetProperty("QID");
                var qidExpiryDateProperty = entity.GetType().GetProperty("QIDExpiryDate");
                var fullNameArProperty = entity.GetType().GetProperty("FullNameAr");
                var fullNameEnProperty = entity.GetType().GetProperty("FullNameEn");
                var nationalityCodeProperty = entity.GetType().GetProperty("NationalityCode");
                var userGenderIdProperty = entity.GetType().GetProperty("UserGenderId");
                var dobProperty = entity.GetType().GetProperty("DOB");

                if (qidProperty == null || qidExpiryDateProperty == null)
                    continue;

                var qid = qidProperty.GetValue(entity) as string;
                if (string.IsNullOrWhiteSpace(qid)) continue;

                var qidExpiryDate = qidExpiryDateProperty.GetValue(entity) as DateOnly?;
                string message = $"MOIData other fields , ExpiryDate:{qidExpiryDate!.Value}";
                loggingServices.WriteLineToFile(true, message);
                var moiMessageRequest = new MOIMessageRequest { QID = qid, QIDExpiryDate = qidExpiryDate.Value };
                var moiData = await mOIServices._GetPersonalInfo(moiMessageRequest);
                var GenderList = await GenderListTask;
                if (moiData != null)
                {
                    fullNameArProperty?.SetValue(entity, string.Join(" ", new List<string> { moiData.ArabicName1, moiData.ArabicName2, moiData.ArabicName3, moiData.ArabicName4, moiData.ArabicName5 }.Where(n => !string.IsNullOrEmpty(n))));
                    fullNameEnProperty?.SetValue(entity, string.Join(" ", new List<string> { moiData.EnglishName1, moiData.EnglishName2, moiData.EnglishName3, moiData.EnglishName4, moiData.EnglishName5 }.Where(n => !string.IsNullOrEmpty(n))));
                    nationalityCodeProperty?.SetValue(entity, moiData.NationalityCode);
                    var genderId = moiData.Gender?.ToUpper() == "FEMALE" ? GenderList?.FirstOrDefault(x=>x.TitleEn== "Female")?.Id : GenderList?.FirstOrDefault(x => x.TitleEn == "Male")?.Id;
                    userGenderIdProperty?.SetValue(entity, genderId);

                    if (!string.IsNullOrWhiteSpace(moiData.DateOfBirth) && DateTime.TryParseExact(moiData.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                    {
                        if (dobProperty?.PropertyType == typeof(DateOnly))
                        {
                            dobProperty.SetValue(entity, DateOnly.FromDateTime(dob));
                        }
                        else
                        {
                            dobProperty?.SetValue(entity, dob);
                        }
                    }
                    
                }
                else
                {
                    var name = fullNameArProperty?.GetValue(entity) ?? fullNameEnProperty?.GetValue(entity);
                    var errorMessage = lang == "ar"
                        ? $"{name} غير موجود في سجل وزارة الداخلية."
                        : $"{name} not found in MOI records.";

                    errors.Add(errorMessage);
                }
            }

            return errors;
        }
        public async Task<List<string>> ValidateMinAndMaxAgeAsync(List<SchCompanion> companions, int? minAge, int? maxAge, string lang)
        {
            var errors = new List<string>();

            foreach (var companion in companions)
            {
                if (companion.DOB == default)
                    continue;

                var age = CalculateAge(companion.DOB);

                // Validate Min Age
                if (minAge.HasValue && age < minAge.Value)
                {
                    var error = lang == "ar"
                        ? $"المرافق {companion.FullNameAr} أصغر من الحد الأدنى للعمر المسموح به ({minAge.Value} سنة)."
                        : $"Companion {companion.FullNameEn} is younger than the minimum allowed age ({minAge.Value} years).";
                    errors.Add(error);
                }

                // Validate Max Age
                if (maxAge.HasValue && age > maxAge.Value)
                {
                    var error = lang == "ar"
                        ? $"المرافق {companion.FullNameAr} أكبر من الحد الأقصى للعمر المسموح به ({maxAge.Value} سنة)."
                        : $"Companion {companion.FullNameEn} is older than the maximum allowed age ({maxAge.Value} years).";
                    errors.Add(error);
                }
            }

            return errors;
        }
        private int CalculateAge(DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dob.Year;

            if (today < dob.AddYears(age))
                age--;

            return age;
        }
        public async Task<List<string>> ValidateActiveScholarshipAsync(List<SchCompanion> companions, string lang)
        {
            var errors = new List<string>();

            foreach (var companion in companions)
            {
                int? activeCount = await ActiveScholarshipCountAsync(companion.QID);

                if (activeCount > 0)
                {
                    var error = lang == "ar"
                        ? $"المرافق {companion.FullNameAr}  يملك منحة نشطة."
                        : $"Companion {companion.FullNameEn}  have an active scholarship.";
                    errors.Add(error);
                }
            }

            return errors;
        }
        public async Task<int?> ActiveScholarshipCountAsync(string QID)
        {
            var restrictedStatuses = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.SchRestrictedStatuses);
            var restrictedStatusList = restrictedStatuses?
                                     .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => x.Trim())
                                     .ToList() ?? new List<string>();

            return await uow.GetRepository<ScholarshipData>()
                .GetAllActiveNonDeleted()
                .Include(c => c.StudentUser)
                .Include(c => c.SchStatus)
                .AsSplitQuery()
                .Where(c => c.StudentUser!.QID == QID)
                .Where(c => !restrictedStatusList.Contains(c.SchStatus!.BackEndName)) 
                .CountAsync();
        }
        private async Task<List<string>> ValidateUniqueQidAsync<TEntity>(List<TEntity> entities, string lang) where TEntity : EntityBase, new()
        {
            var errors = new List<string>();
            var qidSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entity in entities)
            {
                var qidProperty = entity.GetType().GetProperty("QID");
                if (qidProperty == null) continue;

                var qid = qidProperty.GetValue(entity) as string;
                qid = qid?.Trim();

                if (string.IsNullOrEmpty(qid)) continue;

                if (!qidSet.Add(qid))
                {
                    var errorMsg = lang == "ar"
                        ? $"رقم البطاقة الشخصية مكرر: {qid}"
                        : $"Duplicate QID found: {qid}";

                    errors.Add(errorMsg);
                }
            }

            return errors;
        }
        public async Task<bool> CheckQIDInMOIAsync(string qid, DateOnly qidExpiryDate)
        {
            if (string.IsNullOrEmpty(qid)) return false;

            var moiMessageRequest = new MOIMessageRequest
            {
                QID = qid,
                QIDExpiryDate = qidExpiryDate,
            };

            var moiData = await mOIServices._GetPersonalInfo(moiMessageRequest);

            if (moiData == null) return false;

            return true;
        }
        public async Task<string> CheckQIDInMOIAndGetDOBAsync(string qid, DateOnly qidExpiryDate)
        {
            if (string.IsNullOrEmpty(qid)) return string.Empty;

            var moiMessageRequest = new MOIMessageRequest
            {
                QID = qid,
                QIDExpiryDate = qidExpiryDate,
            };

            var moiData = await mOIServices._GetPersonalInfo(moiMessageRequest);

            if (moiData == null) return string.Empty;

            if (DateOnly.TryParse(moiData.DateOfBirth, out DateOnly dob))
            {
                return CalculateAge(dob).ToString();
            }
            return string.Empty;
        }
        public async Task<List<string>> ValidateCompanionListAsync(List<SchCompanion> companions, string lang)
        {
            var errors = new List<string>();

            if (companions == null || !companions.Any())
                return errors;

            var companionCountByType = companions
                .GroupBy(c => c.CompanionTypeId)
                .ToDictionary(g => g.Key, g => new
                {
                    Count = g.Count(),
                    TypeId = g.Key,
                    Type = g.FirstOrDefault()?.CompanionType
                });

            foreach (var entry in companionCountByType)
            {
                int maxCount = await GetMaxCountByCompanionTypeIdAsync(entry.Key);
                if (maxCount > 0 && entry.Value.Count > maxCount)
                {
                    string typeNameAr = entry.Value.Type?.NameAr ?? "غير معروف";
                    string typeNameEn = entry.Value.Type?.NameEn ?? "Unknown";

                    var errorMessage = lang == "ar"
                        ? $"لا يمكن أن يكون هناك أكثر من {maxCount} من {typeNameAr}."
                        : $"Cannot have more than {maxCount} {typeNameEn}.";

                    errors.Add(errorMessage);
                }
            }

            return errors;
        }
        public async Task<List<string>> validateDefaultBankAccountAsync(List<BankAccount> bankAccounts, string lang)
        {
            var errors = new List<string>();

            // Filter accounts marked as default
            var defaultAccounts = bankAccounts
                .Where(acc => acc.IsDefault == true)
                .ToList();

            if (defaultAccounts.Count != 1)
            {
                var errorMessage = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblMustHaveOneDefaultAccount, requestInfo.Lang);

                errors.Add(errorMessage);
            }

            return errors;
        }
     
        private async Task<List<string>> validateUniqueIbanNumberAsync(List<BankAccount> list,Guid? UserId, string lang)
        {
            var errors = new List<string>();
            var ibans = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in list)
            {
                var iban = item.IBANNumber?.Trim();
                if (string.IsNullOrEmpty(iban))
                    continue;

                if (!ibans.Add(iban))
                {
                    var errorMessage = await cacheDataProvider
                        .GetExceptionMessage(ConstantKeys.ExceptionMessage.lblUniqueIbanRequired, requestInfo.Lang);

                    errors.Add(errorMessage);
                    continue;
                }

                bool ibanExists =  await serviceScopeFactory.CreateScopedUow().GetRepository<StudentBankAccount>()
                    .GetAllActiveNonDeleted()
                    .AnyAsync(x => x.IBANNumber!.ToLower() == iban.ToLower() && x.StudentUserId != UserId);

                if (ibanExists)
                {
                    var errorMessage = await cacheDataProvider
                        .GetExceptionMessage(ConstantKeys.ExceptionMessage.lblIbanAlreadyExists, requestInfo.Lang);

                    errors.Add(errorMessage);
                }
            }

            return errors;
        }

        private async Task<List<string>> validateUniqueSemesterYearAsync(List<SchSemester> semesters, string lang)
        {
            var errors = new List<string>();
            var semesterYearSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var semester in semesters)
            {
                var academicYear = semester.SchAcademicYearId;
                var semesterId = semester.SemesterId;

                var key = $"{academicYear}-{semesterId}";

                if (!semesterYearSet.Add(key))
                {
                    var errorMessage = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblUniqueSemesterYearRequired, requestInfo.Lang);

                    errors.Add(errorMessage);
                }
            }

            return errors;
        }
		private async Task<List<string>> validateDeletedSemestersAsync(Guid? scholarshipId, string jsonValue, string lang)
		{
			var errors = new List<string>();
            if (scholarshipId != null || scholarshipId != Guid.Empty)
            {


                var incomingIds = ExtractIncomingIdsFromJson(jsonValue, "Index");

                var semesterRepo = serviceScopeFactory.CreateScopedUow().GetRepository<SchSemester>();
                var existingSemesters = await semesterRepo
                    .GetAllQueryFiltered(s => s.ScholarshipId == scholarshipId)
                    .Select(s => new { s.Id, s.SchAwardId })
                    .AsNoTracking()
                    .ToListAsync();

                var deleted = existingSemesters
                    .Where(s => !incomingIds.Contains(s.Id))
                    .ToList();

                var linked = deleted.Where(s => s.SchAwardId != null).ToList();

                if (linked.Any())
                {
                    var errorMessage = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblCantDeleteSemester, requestInfo.Lang);

                    errors.Add(errorMessage);
                }
            }

			return errors;
		}
		private async Task<List<string>> validateDeletedAwardsAsync(Guid? scholarshipId, string jsonValue, string lang)
		{
			var errors = new List<string>();
            if (scholarshipId != null || scholarshipId != Guid.Empty)
            {

                var incomingIds = ExtractIncomingIdsFromJson(jsonValue, "Index");

                var awardRepo = serviceScopeFactory.CreateScopedUow().GetRepository<SchAward>();
                var existingAwards = await awardRepo
                    .GetAllQueryFiltered(a => a.ScholarshipId == scholarshipId)
                    .Select(s => new { s.Id, s.StudentPaymentTransactionId })
                    .AsNoTracking()
                    .ToListAsync();

                var deleted = existingAwards.Where(a => !incomingIds.Contains(a.Id)).ToList();
                if (deleted.Count == 0)
                    return errors;

                bool hasPayments = deleted.Any(a => a.StudentPaymentTransactionId != null);

                if (hasPayments)
                {
                    var errorMessage = await cacheDataProvider.GetExceptionMessage(ConstantKeys.ExceptionMessage.lblCantDeleteSemester, requestInfo.Lang);

                    errors.Add(errorMessage);
                }
            }
			return errors;
		
		}

		private static HashSet<Guid> ExtractIncomingIdsFromJson(string? json, string idKey = "Index")
		{
			var ids = new HashSet<Guid>();
			if (string.IsNullOrWhiteSpace(json)) return ids;

			
				var list = Newtonsoft.Json.JsonConvert
					.DeserializeObject<List<Dictionary<string, object>>>(json) ?? new();

				foreach (var dic in list)
				{
					if (dic.TryGetValue(idKey, out var idObj))
					{
						var g = ToGuid(idObj);
						if (g.HasValue && g.Value != Guid.Empty)
							ids.Add(g.Value);
					}
				}
			

			return ids;
		}

		private static Guid? ToGuid(object? v)
		{
			if (v is null) return null;
			if (v is Guid g) return g;
			var s = v.ToString();
			return Guid.TryParse(s, out var parsed) ? parsed : (Guid?)null;
		}

		private async Task<int> GetMaxCountByCompanionTypeIdAsync(Guid companionTypeId)
        {
            var companionType = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<CompanionType>()
                                        .GetAll(ct => ct.Id == companionTypeId)
                                        .Select(ct => new { ct.MaxCountPerScholarship })
                                        .FirstOrDefaultAsync();

            return companionType?.MaxCountPerScholarship ?? 0;
        }
        public async Task<string> UpdateCompanionListJsonAsync(List<SchCompanion> updatedCompanions, Guid fieldId, string jsonData)
        {
            if (updatedCompanions == null || !updatedCompanions.Any() || string.IsNullOrEmpty(jsonData))
                return jsonData;

            // Check if any companion has missing required MOI data
            bool hasMissingMoiData = updatedCompanions.Any(c =>
                (string.IsNullOrWhiteSpace(c.FullNameEn) && string.IsNullOrWhiteSpace(c.FullNameAr)) ||
                string.IsNullOrWhiteSpace(c.NationalityCode)
            );

            if (hasMissingMoiData)
                throw new BusinessException(ExceptionMessage.MOIDataMissing);
            var fieldList = await SrvField.GetFieldListByFieldId(fieldId);
            var fieldMapping = fieldList
                .ToDictionary(
                    f => f.BackendName.Replace("Companion", ""),
                    f => f.Id
                );
            var jsonList = JArray.Parse(jsonData);// JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);

            foreach (var companion in updatedCompanions)
            {
                foreach (var jsonEntry in jsonList.Children<JObject>())
                {
                    if (jsonEntry.TryGetValue("Index", out var indexToken) && indexToken.ToString() == companion.Id.ToString())
                    {
                        var nationalityId =await SrvDropdown.GetCountryBycode(companion.NationalityCode);
                        UpdateField(jsonEntry, fieldMapping, "FullNameEn", companion.FullNameEn);
                        UpdateField(jsonEntry, fieldMapping, "FullNameAr", companion.FullNameAr);
                        UpdateField(jsonEntry, fieldMapping, "DOB", companion.DOB.ToString("dd/MM/yyyy"));
                        UpdateField(jsonEntry, fieldMapping, "NationalityCode", nationalityId.Id.ToString());
                        UpdateField(jsonEntry, fieldMapping, "UserGenderId", companion.UserGenderId.ToString());
                    }
                }
            }

            return JsonConvert.SerializeObject(jsonList, Formatting.Indented);
        }
        public async Task<string> UpdateGuarantorListJsonAsync(List<SchGuarantor> updatedGuarantors, Guid fieldId, string jsonData)
        {
            if (updatedGuarantors == null || !updatedGuarantors.Any() || string.IsNullOrEmpty(jsonData))
                return jsonData;
            var fieldList = await SrvField.GetFieldListByFieldId(fieldId);
            var fieldMapping = fieldList.ToDictionary(
                                         f => f.BackendName.Replace("Guarantor", ""),
                                         f => f.Id
                                     );

            var jsonList = JArray.Parse(jsonData);// JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);

            foreach (var Guarantor in updatedGuarantors)
            {
                foreach (var jsonEntry in jsonList.Children<JObject>())
                {
                    if (jsonEntry.TryGetValue("Index", out var indexToken) && indexToken.ToString() == Guarantor.Id.ToString())
                    {
                        var nationalityId = await SrvDropdown.GetCountryBycode(Guarantor.NationalityCode);

                        UpdateField(jsonEntry, fieldMapping, "EnglishFullName", Guarantor.FullNameEn);
                        UpdateField(jsonEntry, fieldMapping, "ArabicFullName", Guarantor.FullNameAr);
                        UpdateField(jsonEntry, fieldMapping, "DateOfBirth", Guarantor.DOB.ToString("dd/MM/yyyy"));
                        UpdateField(jsonEntry, fieldMapping, "Nationality", nationalityId!.Id.ToString());
                        UpdateField(jsonEntry, fieldMapping, "Gender", Guarantor.UserGenderId.ToString());
                    }
                }
            }
            return JsonConvert.SerializeObject(jsonList, Formatting.Indented);
        }
        private void UpdateField(JObject jsonEntry, Dictionary<string, Guid> fieldMapping, string backendName, string newValue)
        {
            if (fieldMapping.TryGetValue(backendName, out var fieldId) && !string.IsNullOrEmpty(newValue))
            {
                string key = fieldId.ToString();

                if (jsonEntry.ContainsKey(key))
                {
                    jsonEntry[key] = JToken.FromObject(newValue);
                }
                else
                {
                    jsonEntry.Add(new JProperty(key, newValue));
                }
            }
        }
        public async Task<bool> ValidatedConditionAsync(object? fieldValue, string operatorType, string comparisonValue)
        {
            switch (operatorType.ToLowerInvariant())
            {
                case "equal":
                    return EqualsLoose(fieldValue, comparisonValue);

                case "not equal":
                    return !EqualsLoose(fieldValue, comparisonValue);

                case "lessthan":
                    return Comparer.Default.Compare(fieldValue, comparisonValue) < 0;

                case "greaterthan":
                    return Comparer.Default.Compare(fieldValue, comparisonValue) > 0;

                case "in":
                    return InCsv(fieldValue, comparisonValue);

                case "not in":
                    return !InCsv(fieldValue, comparisonValue);

                case "like":
                    {
                        var left = fieldValue?.ToString() ?? "";
                        var right = comparisonValue ?? "";
                        return left.IndexOf(right, StringComparison.OrdinalIgnoreCase) >= 0;
                    }

                case "not null":
                    return fieldValue != null;

                default:
                    throw new ArgumentException($"Unsupported operator: {operatorType}");
            }
        }

        private static bool EqualsLoose(object? left, string right)
        {
            if (Guid.TryParse(Convert.ToString(left), out var g1) &&
                Guid.TryParse(right, out var g2))
                return g1.Equals(g2);

            return string.Equals(Convert.ToString(left)?.Trim(),
                                 right?.Trim(),
                                 StringComparison.OrdinalIgnoreCase);
        }

        private static bool InCsv(object? value, string csv)
        {
            var items = csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var item in items)
                if (EqualsLoose(value, item)) return true;

            return false;
        }

        private Task ValidateRemaksAndOtherAttachementAsync(ActionStatusConfiguration actionStatusConfig, string remarks, List<IFormFile> attachments)
        {
            if (actionStatusConfig.IsRemark && actionStatusConfig.IsRemarkRequired && (string.IsNullOrWhiteSpace(remarks) || string.IsNullOrEmpty(remarks)))
            {
                throw new BusinessException(ExceptionMessage.lblRemarksRequired );
            }
            if (actionStatusConfig.IsOtherAttachment && actionStatusConfig.IsOtherAttachmentRequired && (attachments == null || !attachments.Any()))
            {
                throw new BusinessException(ExceptionMessage.lblOtherAttachmentsRequired);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region MapDTO
        private SharedHelper.Models.Api.PartyTypeDTOs.PartyTypeDTO MapToPartyTypeDTO(PartyType partyType)
        {
            return new SharedHelper.Models.Api.PartyTypeDTOs.PartyTypeDTO
            {
                Id = partyType.Id,
                Name = partyType.NameAr,
            };
        }

        #endregion

    }

}
