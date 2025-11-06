using Evaluation.DAL.UnitOfWork;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Scholarship.Services.BusinessLayer.API.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Scholarship.Services.Models.API
{

    public class RequestsBL(
        IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvNotification SrvNotification, SrvUser SrvUser, 
        LoggingServices loggingServices,  UserInfo userInfo, SrvField SrvField, SrvAction SrvAction, 
        SrvStatus SrvStatus, SrvAssignment SrvAssignment, SrvDropdown SrvDropdown, SrvActionTransactionsLog SrvActionTransactionsLog, 
        SrvService SrvService, SrvServiceRequest SrvServiceRequest, SrvAttachments SrvAttachments, IServiceProvider serviceProvider,RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
    {

        public async Task<ServiceRequestDTO> HandleServiceRequestAsync(ActionFormDTO? actionFormDTO, Guid? scholarshipId,
            Guid serviceId, string actionName, string fieldValuesJson, List<AssignUserDTO?> assignUsers,
            IFormFileCollection files, string remarks, bool saveAsDraft = false)
        {
            string lang = _requestInfo.Lang;
            var requestId = actionFormDTO?.RequestId;

            var serviceObj = await SrvService.GetActiveAndOpenServiceById(serviceId);

            if (serviceObj == null)
            {
                throw new BusinessException(ExceptionMessage.IncompleteRequest);
            }

            var action = await SrvAction.GetActionByBackendNameAsync(serviceObj.Id, actionName) ??
                         await SrvAction.GetInitialActionAsync(serviceId, actionName);

            if (action == null || action.ActionType == null)
            {
                throw new BusinessException(ExceptionMessage.IncompleteRequest);
            }

            var (filesWithFieldId, othersAttachement) = SrvAttachments.SeparateFilesByFieldId(files);

            var fileFields = filesWithFieldId.Select(file =>
            {
                var (fieldId, childFieldId, index) = GetFieldDetailsFromFile(file.ContentDisposition);

                return new FileFieldDTO
                {
                    File = file,
                    FieldId = fieldId,
                    childFieldId = childFieldId,
                    Index = index
                };
            }).ToList();

            // Step 3: Create or update the request
            ServiceRequestDTO resultRequest = new ServiceRequestDTO();
            if (requestId == null || requestId == Guid.Empty)
            {
                var status = await SrvStatus.GetInitialStatusByServiceId(serviceId);
                if (status == null)
                {
                    throw new BusinessException(ExceptionMessage.IncompleteRequest);
                }
                Guid studentId;
                Guid? CountryId;
                Guid? UniversityId;
                Guid? InitialHistoryId = null;

                if (scholarshipId != null)
                {
                    var scholarship = await srvScholarship.GetScholarshipByIdAsync(scholarshipId.Value);
                    var SchHistory = await srvFinShared.GetSchHistoryByIdAsync(scholarshipId.Value);


                    if (scholarship == null)
                        throw new BusinessException(ExceptionMessage.missingscholarship);

                    studentId = scholarship.StudentUserId;
                    InitialHistoryId = SchHistory?.Id;
                    CountryId = scholarship.CountryId;
                    UniversityId = scholarship.UniversityId;
                }
                else
                {
                    studentId = userInfo.UserId!.Value;
                    (CountryId, UniversityId) = await GetRequestCountryAndUniversityAsync(null,actionFormDTO?.FieldValues!);
                }

                var request = new ServiceRequest
                {
                    Id = Guid.NewGuid(),
                    StatusId = status.Id,
                    ServiceId = serviceId,
                    StudentId = studentId,
                    SystemModuleId = serviceObj.SystemModuleId,
                    ScholarshipId = scholarshipId,
                    InitialHistoryId = InitialHistoryId,
                    CountryId = CountryId,
                    UniversityId = UniversityId,

                };
                var GetMappedConditionFieldTask = SrvAction.GetActionMappedFieldsAsync(action.Id, RelatedFieldsGroupConstant.ScholarshipDetails);

                var validateRequestTask = ValidateCanCreateRequest(scholarshipId, serviceObj);
                if (actionFormDTO?.FieldValues != null)
                {
                    var validateActionTask = SrvAction.ValidateActionAndActionFieldAsync(null, studentId, actionFormDTO?.FieldValues!, remarks, othersAttachement, serviceObj, status.Id, action, fileFields, saveAsDraft);
                    await Task.WhenAll(validateRequestTask, validateActionTask);

                    var validatedFields = await validateActionTask;

                    var MappedConditionField = await GetMappedConditionFieldTask;
                    if (MappedConditionField.Any() && serviceObj.Initialservice && !saveAsDraft)
                    {
                        var vaildatePlanCondition = await CheckPlanConditionAsync(MappedConditionField!, actionFormDTO?.FieldValues!);
                        if (!vaildatePlanCondition)
                        {
                            throw new BusinessException(ExceptionMessage.lblNotCompliantPlanCondition);
                        }

                    }
                    actionFormDTO!.FieldValues = (await SrvAttachments.UploadAndInsertAttachments(validatedFields.ToList(), request.Id, request.ScholarshipId, fileFields, filesWithFieldId)).Cast<FieldValueDTO?>().ToList();
                }




                resultRequest!.Id = request.Id;

                var actionResult = await PerformAction(request, serviceObj, actionFormDTO!.FieldValues!, action.BackendName, assignUsers.Where(c => c!.IsSelected).ToList()!, remarks, saveAsDraft);

                var studentTask = SrvUser.GetStudentByIdAsync(request.StudentId);

                var otherAttachmentsTask = SrvAttachments.UploadAndInsertOtherAttachments(othersAttachement, actionResult.actionlog, scholarshipId);
                var sequence = request.Sequence;
                var requestNumber = DateTime.Now.ToString(serviceObj.ReqNumberDef ?? "", new CultureInfo("en-US")) + sequence;

                request.RequestNumber = requestNumber;
                resultRequest.RequestNumber = requestNumber;

                SrvServiceRequest.InsertRequest(request);
                var StudentData = await studentTask;
                await srvScholarshipPlan.SetExceptionUsed(StudentData!.QID, request.Id);

                await otherAttachmentsTask;

                await uow.CommitAsync();


                if (!saveAsDraft)
                {
                    // Handle main action notifications
                    await SrvNotification.HandleNotification(actionResult.notifications, request, actionResult.actiondb.Id, lang, remarks, otherAttachmentsTask.Result);

                    // Check if auto-assign logic is applicable
                    bool shouldAutoAssign = request != null &&
                                            serviceObj.IsAutoAssignEnabled == true &&
                                            status.IsInitial &&
                                            actionResult.actiondb.ActionType!.BackendName == ActionTypeKeys.Info;

                    if (shouldAutoAssign)
                    {
                        var assignAction = await serviceScopeFactory.CreateScopedUow()
                            .GetRepository<ActionStatusConfiguration>()
                            .GetAllQueryFiltered()
                            .Include(x => x.Notifications)
                            .Include(c => c.ServiceAction)
                                .ThenInclude(c => c!.ActionType)
                                .AsSplitQuery()
                            .FirstOrDefaultAsync(c =>
                                (c.ServiceAction!.ActionType!.BackendName == ActionTypeKeys.Assign || c.ServiceAction!.ActionType!.BackendName == ActionTypeKeys.Info)
                                && c.CurrentStatusId == request!.StatusId
                               && c.ServiceAction.IsAutoAssign);

                        if (assignAction != null)
                        {
                            await SrvNotification.HandleNotification(assignAction.Notifications, request!, assignAction.Id, lang, string.Empty);
                        }
                    }
                }


            }
            else
            {
                // Handle  action for existing request
                var application = await SrvServiceRequest.GetSrvServiceRequestByIdAsync(requestId!.Value, true);

                var GetMappedConditionFieldTask = SrvAction.GetActionMappedFieldsAsync(action.Id, RelatedFieldsGroupConstant.ScholarshipDetails);

                var allFields = JsonConvert.DeserializeObject<List<FieldValueDTO?>>(fieldValuesJson);
                var validatedFields = await SrvAction.ValidateActionAndActionFieldAsync(application, application!.StudentId, allFields!, remarks, othersAttachement, serviceObj, application.StatusId, action, fileFields, saveAsDraft);

                if (application.Status!.IsInitial && !saveAsDraft)
                {
                    var MappedConditionField = await GetMappedConditionFieldTask;
                    if (MappedConditionField.Any() && serviceObj.Initialservice)
                    {
                        var vaildatePlanCondition = await CheckPlanConditionAsync(MappedConditionField!, actionFormDTO?.FieldValues!);
                        if (!vaildatePlanCondition)
                        {
                            throw new BusinessException(ExceptionMessage.lblNotCompliantPlanCondition);
                        }
                    }
                }

                actionFormDTO!.FieldValues = (await SrvAttachments.UploadAndInsertAttachments(validatedFields.ToList(), requestId, application.ScholarshipId, fileFields, filesWithFieldId)).Cast<FieldValueDTO?>().ToList();

                var actionResult = await PerformAction(application, serviceObj, actionFormDTO.FieldValues!, actionName, assignUsers.Where(c => c!.IsSelected).ToList()!, remarks, saveAsDraft);

                var otherAttachments = await SrvAttachments.UploadAndInsertOtherAttachments(othersAttachement, actionResult.actionlog, application.ScholarshipId);

                if (application.Service!.Initialservice) { 
                var (CountryId, UniversityId) = await GetRequestCountryAndUniversityAsync(application.Id, actionFormDTO.FieldValues!);
                application.CountryId = CountryId;
                application.UniversityId = UniversityId;
            }

                await uow.CommitAsync();

                if (actionResult.notifications != null && !saveAsDraft) { 
                await SrvNotification.HandleNotification(actionResult.notifications, application, actionResult.actiondb.Id, lang, remarks, otherAttachments);
                }
                resultRequest = new ServiceRequestDTO { Id = requestId, RequestNumber = application.RequestNumber };
            }

            return resultRequest;
        }

        public async Task<(Guid? CountryId, Guid? UniversityId)> GetRequestCountryAndUniversityAsync( Guid? requestId,IEnumerable<FieldValueDTO>? fieldValues = null)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();
            List<(string BackendName, string? Value)> values = new();

            if (fieldValues != null)
            {
                var fields = await uow.GetRepository<Field>()
                    .GetAllActiveNonDeleted()
                    .Include(x => x.Service)
                    .Include(x => x.MappingSystemField)
                    .Where(c => c.Service!.Initialservice &&
                                (c.MappingSystemField!.BackendName == "CountryId" ||
                                 c.MappingSystemField.BackendName == "UniversityId"))
                    .ToListAsync();

                values = fields
                    .Select(f => (
                        f.MappingSystemField!.BackendName,
                        fieldValues.FirstOrDefault(v => v.FieldId == f.Id)?.Value
                    ))
                    .Where(v => !string.IsNullOrWhiteSpace(v.Value))
                    .ToList();
            }

            if (!values.Any() && requestId.HasValue)
            {
                var dbFieldValues = await uow.GetRepository<ServiceRequestFieldsValue>()
                        .GetAllQueryFiltered()
                        .Include(x => x.Field)!.ThenInclude(f => f.Service)
                        .Include(x => x.Field)!.ThenInclude(f => f.MappingSystemField)
                        .Where(x => x.ServiceRequestId == requestId.Value &&
                                    x.Field!.Service!.Initialservice &&
                                    (x.Field.MappingSystemField!.BackendName == "CountryId" ||
                                     x.Field.MappingSystemField.BackendName == "UniversityId"))
                        .Select(x => new
                        {
                            BackendName = x.Field!.MappingSystemField!.BackendName,
                            Value = x.Value
                        })
                        .ToListAsync();

                 values = dbFieldValues.Select(x => (x.BackendName, x.Value)).ToList();
            }

            var countryFieldValue = values.FirstOrDefault(v => v.BackendName == "CountryId").Value;
            var universityFieldValue = values.FirstOrDefault(v => v.BackendName == "UniversityId").Value;

            Guid.TryParse(countryFieldValue, out Guid parsedCountryId);
            Guid.TryParse(universityFieldValue, out Guid parsedUniversityId);

            return (
                parsedCountryId == Guid.Empty ? null : parsedCountryId,
                parsedUniversityId == Guid.Empty ? null : parsedUniversityId
            );
        }
        public (Guid? fieldId, Guid? childFieldId, Guid? index) GetFieldDetailsFromFile(string contentDisposition)
        {
            Match fieldIdMatch = Regex.Match(contentDisposition, @"Files\[(.*?)\]");
            Match filenameMatch = Regex.Match(contentDisposition, @"filename=""(.*?)""");

            if (fieldIdMatch.Success && filenameMatch.Success)
            {
                if (!Guid.TryParse(fieldIdMatch.Groups[1].Value, out Guid parsedFieldId))
                    return (null, null, null);

                string filename = filenameMatch.Groups[1].Value;
                string[] filenameParts = filename.Split('_');

                if (filenameParts.Length >= 3)
                {
                    Guid? childFieldId = Guid.TryParse(filenameParts[0], out Guid parsedChildFieldId) ? parsedChildFieldId : null;
                    Guid? index = Guid.TryParse(filenameParts[1], out Guid parsedIndex) ? parsedIndex : null;

                    return (parsedFieldId, childFieldId, index);
                }

                return (parsedFieldId, null, null);
            }

            return (null, null, null);
        }
        public async Task<bool> CheckPlanConditionAsync(List<Field> mappedConditionFields, List<FieldValueDTO> fieldValues)
        {
            var userId = userInfo.UserId;
            var user = await SrvUser.GetStudentByIdAsync(userId!.Value);
            var userGenderId = user?.UserGenderId;
            if (user == null ||userGenderId == null || userGenderId == Guid.Empty)
                throw new BusinessException(ExceptionMessage.lblUserGenderIDMissing);


            string GetFieldValue(string backendName)
            {
                var field = mappedConditionFields?.FirstOrDefault(f => f.MappingSystemField?.BackendName == backendName);
                return fieldValues?.FirstOrDefault(v => v.FieldId == field?.Id)?.Value??string.Empty;
            }


            var majorIdStr = GetFieldValue("MajorId");
            var degreeIdStr = GetFieldValue("AcademicDegreeId");
            var enrollmentTypeIdStr = GetFieldValue("EnrollmentTypeId");
            var countryIdStr = GetFieldValue("CountryId");
            var universityIdStr = GetFieldValue("UniversityId");
            var trackIdStr = GetFieldValue("ScholarshipTrack");
            var previousDegreeValue = GetFieldValue("PreviousDegree");

            if (string.IsNullOrEmpty(majorIdStr) || string.IsNullOrEmpty(degreeIdStr))
                throw new BusinessException(ExceptionMessage.lblMajorOrDegreeIdMissing);

            if (!Guid.TryParse(majorIdStr, out Guid majorId) || !Guid.TryParse(degreeIdStr, out Guid academicDegreeId))
                throw new BusinessException(ExceptionMessage.lbl_Request_is_not_Valid);

            if (!Guid.TryParse(enrollmentTypeIdStr, out Guid enrollmentTypeId) ||
                !Guid.TryParse(countryIdStr, out Guid countryId) ||
                !Guid.TryParse(universityIdStr, out Guid universityId) ||
                !Guid.TryParse(trackIdStr, out Guid trackId))
            {
                throw new BusinessException(ExceptionMessage.lbl_Request_is_not_Valid); // Adjust message if needed
            }

            DegreeGPA? previousDegree = null;
            if (!string.IsNullOrWhiteSpace(previousDegreeValue))
            {
                if (decimal.TryParse(previousDegreeValue, out var gpa))
                {
                    previousDegree = new DegreeGPA { GPA = gpa };
                }
                else
                {
                    throw new BusinessException(ExceptionMessage.NotValidpreviousDegreeValue);
                }
            }

            var requestDetailsDTO = new SchRequestDetails
            {
                StudentUserId = userId.Value,
                MajorId = majorId,
                AcademicDegreeId = academicDegreeId,
                EnrollmentTypeId = enrollmentTypeId,
                CountryId = countryId,
                UniversityId = universityId,
                TrackId = trackId,
                QID = user!.QID,
                PreviousDegree = previousDegree!
            };


            // 1. Validate program eligibility
            var validationResult = await srvScholarshipPlan.ValidateScholarshipRequest(requestDetailsDTO);
            bool hasValidProgram = validationResult?.IsValidRequest ?? false;

            // 2. Check vacancy availability
            var vacancies = await SrvVacancy.SearchVacanciesHaveSeatAvailable(userGenderId.Value, academicDegreeId, majorId);
            bool hasVacancy = vacancies != null && vacancies.Any();

            if (!hasValidProgram || !hasVacancy)
            {
                string reason = !hasValidProgram
                    ? $"Conflicts: {string.Join(", ", validationResult?.ProgramConditionConflicts?.Select(c => c.ProgramId) ?? Enumerable.Empty<Guid>())}"
                    : "No vacancies available for the selected criteria.";

                var requestFieldsJson = JsonConvert.SerializeObject(fieldValues ?? new List<FieldValueDTO>());
                var conditionIds = string.Join(",", mappedConditionFields?.Select(f => f.Id) ?? Enumerable.Empty<Guid>());

                await InsertFailedRequestAsync(userId.Value, requestFieldsJson, conditionIds, reason);
                return false;
            }

            return true;
        }
        private async Task InsertFailedRequestAsync(Guid studentUserId, string requestFieldJason, string conditionsId, string errorMessage)
        {
            using (var newScope= serviceScopeFactory.CreateScopedUow())
            {
                var serviceRequestFailed = new ServiceRequestFailed
                {
                    StudentUserId = studentUserId,
                    RequestFieldJason = requestFieldJason,
                    ConditionsId = conditionsId,
                    ErrorMessage = errorMessage,
                    IsActive=true,
                    CreateById=userInfo!.UserId!.Value
                };
                await newScope.GetRepository<ServiceRequestFailed>().InsertAsync(serviceRequestFailed);

                await newScope.CommitAsync();
            }

        }
        public async Task<bool> ValidateCanCreateRequest(Guid? scholarshipId, Service serviceObj)
        {
            if (serviceObj == null)
            {
                throw new BusinessException(ExceptionMessage.IncompleteRequest);
            }

				#region maxCountOpen

				int defaultMaxCountOpen = int.Parse(ServiceSettings.MaxCountOpen);
            int maxCountOpen = defaultMaxCountOpen;

            var serviceSettingsJson = serviceObj.ServiceSettings ?? "{}";
            var serviceSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(serviceSettingsJson);

            if (!TryGetSettingValue(serviceSettings!, ServiceSettingsKey.MaxCountOpen, out maxCountOpen))
            {
                var systemSettingsJson = await cacheDataProvider.GetSystemSettingValue(SystemSettings.ServiceSettings);
                if (!string.IsNullOrEmpty(systemSettingsJson))
                {
                    var systemSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(systemSettingsJson);
                    if (systemSettings == null || !TryGetSettingValue(systemSettings, ServiceSettingsKey.MaxCountOpen, out maxCountOpen))
                    {
                        SrvService.UpdateServiceSettingsAsync(serviceObj, ServiceSettingsKey.MaxCountOpen, maxCountOpen);
                    }
                }
            }

            await ValidateIfThereIsOpenedRequestForServiceAsync(scholarshipId, serviceObj, maxCountOpen);

            #endregion

            return true;
        }

        private bool TryGetSettingValue(Dictionary<string, object> settings, string settingKey, out int result)
        {
            result = 0;

            if (settings != null && settings.TryGetValue(settingKey, out var settingValue))
            {
                return int.TryParse(settingValue?.ToString(), out result);
            }
            return false;
        }
        private async Task ValidateIfThereIsOpenedRequestForServiceAsync(Guid? scholarshipId, Service serviceObj, int maxCountOpen)
        {

			var user = await SrvUser.GetByIDActiveNonDeleted(userInfo!.UserId!.Value);


			var isMinistry = user is MinistryUser;

           
			var openRequests =await serviceScopeFactory.CreateScopedUow()
                                    .GetRepository<ServiceRequest>()
                                        .GetAllQueryFiltered()
                                        .Include(c => c.Status)
                                        .Where(c => c.ScholarshipId == scholarshipId || serviceObj.Initialservice)
                                        .Where(c => c.ServiceId == serviceObj.Id && c.Status!.IsOpen && !c.IsDeleted!.Value )
                                        .Where(c=>c.StudentId== userInfo!.UserId || isMinistry)
                                        .CountAsync();


            if (openRequests >= maxCountOpen)
            {
                throw new BusinessException(ExceptionMessage.lblRequestAlreadyOpened);
            }
        }
        public async Task<PerforActionResponseDTO> PerformAction(ServiceRequest application, Service serviceObj, IList<FieldValueDTO> Fields, string actionname, List<AssignUserDTO> users, string Remarks, bool saveAsDraft = false)
        {
            string lang = _requestInfo.Lang;
            var result = new PerforActionResponseDTO();

            var userId = userInfo.UserId;
            if (application == null)
                throw new BusinessException(ExceptionMessage.lblRequestFieldsMissing);

            var actiondb = await SrvAction.GetActionByBackendNameAsync(serviceObj.Id, actionname,true);

            var actionConfigurationTask = SrvAction.GetActionConfigurationAsync(actiondb!.Id, application.StatusId);
            var existingFieldsTask = SrvServiceRequest.GetRequestFieldsValueAsync(application.Id);

            Fields = await UpdateCustomFieldJsonSchemaValue(application.Id, Fields);


            var currentStatus = application.StatusId;





            var existingFields = await existingFieldsTask;
            var approvedFieldIds = existingFields
                                    .Where(f => f.IsApproved == true)
                                    .Select(f => f.FieldId)
                                    .ToHashSet();

            var editableFields = actiondb.ActionStepsFields!
                                     .Where(f =>
                                         ( f.IsEditable == true && !approvedFieldIds.Contains(f.FieldId)) || actiondb.IsInitialAction||
                                         (f.IsEditable == true &&
                                          new[] {
                                             ActionTypeKeys.INFO_Override_Approve,
                                             ActionTypeKeys.RequestDataChange
                                          }.Contains(actiondb.ActionType!.BackendName))
                                     )
                                     .Select(f => f.FieldId)
                                     .ToList();
            var actionConfiguration = await actionConfigurationTask;
            result.notifications = actionConfiguration!.Notifications;
            var actionTransactionlog =await SrvActionTransactionsLog.UpdateStatusAndLogAction(application, actiondb.Id, actionConfiguration.NextStatusId, Remarks, saveAsDraft);


            var integrationFieldsDtos = actiondb.ActionStepsFields!
                                   .Where(x => x.Field!.FieldAttributeValues != null &&
                                               x.Field.FieldAttributeValues.Any(attr => attr.AttributeKey.StartsWith("Integration_")))
                                   .Select(x => new FieldValueDTO
                                   {
                                       FieldId = x.Field!.Id,
                                       Value = "",
                                       Type = x.Field.FieldType!.BackendName,
                                       FormGroupId = x.Field.FormGroupId,
                                       FormGroupListId = x.Field.FormGroupListId,
                                       Attributes = (x.Field.FieldAttributeValues?.Select(attr => new AttributeDTO
                                       {
                                           Name = attr.AttributeKey,
                                           Value = attr.AttributeValue,
                                           Message = lang == "ar" ? attr.MessageAr : attr.MessageEn
                                       }).ToList())!,
                                   })
                                   .ToList();



            var missingIntegrationFields = integrationFieldsDtos.Where(f => !Fields.Any(existing => existing.FieldId == f.FieldId)).ToList();

            foreach (var field in missingIntegrationFields)
            {
                Fields.Add(field);
            }
            var (allFields, approvedIntegrationFields) = await UpdateIntegrationFieldValuesAsync(application.Id, actiondb.ActionType!.BackendName, integrationFieldsDtos, Fields, existingFields);

            Fields = allFields;

           
            var FieldsToUpdates1 = Fields
                                  .Where(c => editableFields.Contains(c.FieldId!.Value))
                                  .ToList();
            Fields = await HandleVacancyIfRequired(application, Fields.ToList(), FieldsToUpdates1.ToList(), lang);
            var FieldsToUpdates = Fields
                                  .Where(c => editableFields.Contains(c.FieldId!.Value) || c.BackendName == "EntityContractId" || c.BackendName == "SectorId")
                                  .ToList();
            switch (actiondb.ActionType.BackendName)
            {
                case ActionTypeKeys.Approve:
                    await ApproveUneditedFieldsAsync(application.ServiceId, application.Id, FieldsToUpdates, existingFields, Fields, lang, actiondb.Id);
                    break;

                case ActionTypeKeys.Info:
                case ActionTypeKeys.INFO_WITH_DRAFT:
                    var updatedfields= await PrepareAndUpdateFields(application.ServiceId, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
                    break;
                case ActionTypeKeys.INFO_Override_Approve:
                    await UnApproveFields(existingFields, Fields);
                    await PrepareAndUpdateFields(application.ServiceId, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
                    break;
                case ActionTypeKeys.EDIT:
                    await PrepareAndUpdateFields(application.ServiceId, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
                    break;
                case ActionTypeKeys.Assign:
                    await SrvAssignment.PerformAssignAction(application.Id, users!);
                    await AddOrUpdateFields(Fields, existingFields, application.Id, lang, actiondb.Id);
                    break;
               
                case ActionTypeKeys.Approve_And_Assign:
                    await SrvAssignment.PerformAssignAction(application.Id, users!);
                    await ApproveUneditedFieldsAsync(application.ServiceId, application.Id, FieldsToUpdates, existingFields, Fields, lang, actiondb.Id);
                    break;

                case ActionTypeKeys.Reject:
                case ActionTypeKeys.RETURNBACK:
                    await HandleRejectAction(existingFields, Fields, actiondb.Id);
                    break;
                case ActionTypeKeys.RequestDataChange:
                    await HandleRequestMissingAction(existingFields, Fields, actiondb.Id);
                    break;
                case ActionTypeKeys.CreateScholarship:
                    if (FieldsToUpdates.Count > 0)
                    {

                        var updatedfiels = await PrepareAndUpdateFields(application.ServiceId, application.Id, FieldsToUpdates, existingFields, lang, actiondb.Id);
                        existingFields.AddRange(updatedfiels);
                    }
                    var ApprovedOrActionField = existingFields.Where(x => x.IsApproved == true || Fields.Any(f => f.FieldId == x.FieldId) || x.Field?.MappingSystemField?.BackendName== "EntityContractId" || x.Field?.MappingSystemField?.BackendName  == "SectorId").ToList();

                    await srvScholarship.CreateScholarship(serviceObj,application, application.Id, actiondb, ApprovedOrActionField, lang);
                    break;
                case ActionTypeKeys.CloseAndUpdate:
                    if (FieldsToUpdates.Count > 0)
                    {
                        var updatedfiels = await AddOrUpdateFields(FieldsToUpdates, existingFields, application.Id, lang, actiondb.Id);
                        existingFields.AddRange(updatedfiels);
                    }
                    //await ApproveFields(existingFields.Where(x => Fields.Any(f => f.FieldId == x.FieldId)).ToList(), actiondb.Id);
                    var ApprovedOrActionFields = existingFields.Where(x => x.IsApproved == true || Fields.Any(f => f.FieldId == x.FieldId) || x.Field?.MappingSystemField?.BackendName == "EntityContractId" || x.Field?.MappingSystemField?.BackendName  == "SectorId").ToList();

                    await srvScholarship.UpdateScholarship(serviceObj,application, actiondb, ApprovedOrActionFields, lang);
                    break;
                case ActionTypeKeys.Close:
                    await AddOrUpdateFields(FieldsToUpdates, existingFields, application.Id, lang, actiondb.Id);
                    await ReleaseVacancySeatIfExists(existingFields);
                    break;
                case ActionTypeKeys.UPDATE_ITEGRATION_FIELDS:
                    await PrepareAndUpdateFields(application.ServiceId, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
                    break;
                default:
                    await PrepareAndUpdateFields(application.ServiceId, application.Id, Fields.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
                    break;
            }


            result.request = application;
            result.actiondb = actiondb;
            result.actionlog =  actionTransactionlog;

            var status = SrvStatus.GetStatusById(currentStatus);

            if (serviceObj.IsAutoAssignEnabled == true && !saveAsDraft)
            {
                // Retrieve field IDs for Country and University
                var countryField = actiondb.ActionStepsFields!
                    .FirstOrDefault(x => x.Field!.MappingSystemField?.BackendName == "CountryId")?.FieldId;
                var universityField = actiondb.ActionStepsFields!
                    .FirstOrDefault(x => x.Field!.MappingSystemField?.BackendName == "UniversityId")?.FieldId;

                // Fetch values only if the field IDs are valid
                Guid? countryValue = countryField.HasValue ? TryGetGuidFieldValue(countryField.Value, FieldsToUpdates, existingFields) : null;

                Guid? universityValue = universityField.HasValue ? TryGetGuidFieldValue(universityField.Value, FieldsToUpdates, existingFields) : null;

                var Assignaction = await serviceScopeFactory.CreateScopedUow()
                    .GetRepository<ActionStatusConfiguration>()
                    .GetAllQueryFiltered()
                    .Include(c => c.ServiceAction)
                    .ThenInclude(c => c!.ActionType)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(c => c.ServiceAction!.ActionType!.BackendName == ActionTypeKeys.Assign
                        && c.CurrentStatusId == application.StatusId && c.IsAuto);

                if (Assignaction != null && Assignaction.IsAuto)
                {
                    bool AutoAssign = await SrvAssignment.PerformAutoAssign(application, Assignaction.ServiceActionId, countryValue, universityValue);
                    if (AutoAssign)
                    {
                        await SrvActionTransactionsLog.UpdateStatusAndLogAction(
                            application, Assignaction.ServiceActionId, Assignaction.NextStatusId, Remarks, saveAsDraft);
                    }
                }
            }

            if (application!=null && actiondb.SchStatusId!=null&& actiondb.ActionType.BackendName!=ActionTypeKeys.CloseAndUpdate&& actiondb.ActionType.BackendName != ActionTypeKeys.CreateScholarship)
            {
               await srvScholarship.UpdateStatusScholarship(application, actiondb.SchStatusId.Value);
            }


            return result;
        }
        private Guid? TryGetGuidFieldValue(Guid fieldId, IList<FieldValueDTO> primaryList, IList<ServiceRequestFieldsValue> fallbackList)
        {
            var valueStr = primaryList.FirstOrDefault(x => x.FieldId == fieldId)?.Value
                        ?? fallbackList.FirstOrDefault(x => x.FieldId == fieldId)?.Value;

            return Guid.TryParse(valueStr, out var result) ? result : (Guid?)null;
        }
        private async Task<List<ServiceRequestFieldsValue>> ApproveUneditedFieldsAsync( Guid serviceId, Guid applicationId,List<FieldValueDTO> FieldsToUpdate, List<ServiceRequestFieldsValue> existingFields,IList<FieldValueDTO> allFields, string lang,Guid actionId)
        {
            List<ServiceRequestFieldsValue> updatedFields = new();

            if (FieldsToUpdate.Count > 0)
            {
                updatedFields = await PrepareAndUpdateFields(serviceId, applicationId, FieldsToUpdate, existingFields, lang, actionId);
                existingFields.AddRange(updatedFields);
            }

            var editedFieldIds = new HashSet<Guid>(FieldsToUpdate.Select(f => f.FieldId!.Value));

            var fieldsToApprove = existingFields
                .Where(x =>
                    allFields.Any(f => f.FieldId == x.FieldId) &&
                    !editedFieldIds.Contains(x.FieldId))
                .ToList();

            await ApproveFields(fieldsToApprove, actionId);

            return updatedFields;
        }
        private async Task<(IList<FieldValueDTO> AllFields, List<FieldValueDTO> ApprovedFields)> UpdateIntegrationFieldValuesAsync( Guid requestId,string actionTypeBackendName, List<FieldValueDTO>? integrationFieldsDtos,IList<FieldValueDTO>? allFields, List<ServiceRequestFieldsValue>? existingFields)
        {
            integrationFieldsDtos ??= new List<FieldValueDTO>();
            allFields ??= new List<FieldValueDTO>();
            existingFields ??= new List<ServiceRequestFieldsValue>();

            List<FieldValueDTO> fieldsToUpdate;

            if (actionTypeBackendName == ActionTypeKeys.UPDATE_ITEGRATION_FIELDS)
            {
                fieldsToUpdate = integrationFieldsDtos;
            }
            else
            {
                fieldsToUpdate = integrationFieldsDtos
                    .Where(f => f.FieldId.HasValue && !existingFields.Any(e => e.FieldId == f.FieldId))
                    .ToList();
            }

            if (fieldsToUpdate.Any())
            {
                var userId = requestId != Guid.Empty ? await SrvServiceRequest.GetUserIdByRequestIdAsync(requestId) : userInfo.UserId;
                if (userId == null)
                    userId = userInfo.UserId;

                if (userId != null)
                {
                    var student = await SrvUser.GetStudentByIdAsync(userId.Value);

                    var studentQID = TempQIDForTesting; // Replace with student.QID if available    // "30663401929";

                    var updatedIntegrationFields = await SrvField.ProcessIntegrationFieldsAsync(fieldsToUpdate, studentQID, requestId, true);

                    foreach (var updated in updatedIntegrationFields)
                    {
                        if (!updated.FieldId.HasValue) continue;

                        var target = allFields.FirstOrDefault(f => f.FieldId == updated.FieldId);
                        if (target != null && !string.IsNullOrWhiteSpace(updated.Value?.ToString()))
                        {
                            target.Value = updated.Value;
                            target.IsApproved = true;
                        }
                    }
                }
            }

            var approvedFields = allFields.Where(f => f.IsApproved == true).ToList();

            return (allFields, approvedFields);
        }
        private async Task<List<ServiceRequestFieldsValue>> PrepareAndUpdateFields(Guid serviceId, Guid requestId, List<FieldValueDTO> fields, List<ServiceRequestFieldsValue> existingFields, string lang,Guid actionId, List<FieldValueDTO>? approvedIntegrationFields=null)
        {
            if (fields == null || fields.Count == 0)
                return existingFields;

           
            var updatedFiels = await AddOrUpdateFields(fields, existingFields, requestId, lang, actionId, approvedIntegrationFields!);
            return updatedFiels;
        }
        private async Task<List<FieldValueDTO>> HandleVacancyIfRequired(ServiceRequest serviceRequest, List<FieldValueDTO> Allfields, List<FieldValueDTO> fields, string lang)
        {
            var serviceId = serviceRequest.ServiceId;

            var fieldIds = fields.Select(f => f.FieldId).ToList();

            var mappedFields = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<Field>()
                .GetAllQueryFiltered()
                .Include(x => x.MappingSystemField)
                .Include(x => x.FieldType)
            .AsSplitQuery()
                .Where(c => c.ServiceId == serviceId && (fieldIds.Contains(c.Id) || c.MappingSystemField!.BackendName == "EntityContractId" || c.MappingSystemField!.BackendName == "SectorId"))
                .ToListAsync();

            var vacancySeatField = mappedFields.FirstOrDefault(c => c.MappingSystemField?.BackendName == "VacancySeatId");
            var EntityContractField = mappedFields.FirstOrDefault(c => c.MappingSystemField?.BackendName == "EntityContractId");
            var SectorIdField = mappedFields.FirstOrDefault(c => c.MappingSystemField?.BackendName == "SectorId");
            if (vacancySeatField == null)
                return Allfields;

            var seatFieldValue = fields.FirstOrDefault(x => x.FieldId == vacancySeatField.Id);

            if (string.IsNullOrEmpty(seatFieldValue!.Value))
                return Allfields;
            else
            {
                var seat = await SrvVacancy.GetSeatDetails(Guid.Parse(seatFieldValue.Value));
                if (seat == null)
                {
                    return await srvScholarship.HandleVacancyReservation(serviceRequest, Allfields, EntityContractField, SectorIdField, lang);
                }
            }

            return Allfields;
        }
        private async Task HandleRejectAction(List<ServiceRequestFieldsValue> existingField, IList<FieldValueDTO> fields,Guid actionId)
        {
            var approveddtos = fields.Where(c => c.IsApproved == true).Select(c => c.FieldId!.Value).ToList();
            var rejecteddtos = fields.Where(c => c.IsApproved == false).Select(c => c.FieldId!.Value).ToList();

            var fieldIdsToShiftedToRejected = await SrvDropdown.GetDropdownFieldsGroup(rejecteddtos);

            approveddtos = approveddtos.Except(fieldIdsToShiftedToRejected).ToList();
            rejecteddtos.AddRange(fieldIdsToShiftedToRejected);
            rejecteddtos = rejecteddtos.Distinct().ToList();


            var toApprove = existingField.Where(c => approveddtos.Contains(c.FieldId)).ToList();
            var toReject = existingField.Where(c => rejecteddtos.Contains(c.FieldId)).ToList();
            var dependentRejectedFields = new List<ServiceRequestFieldsValue>();


            foreach (var item in toReject)
            {
                var relatedFieldViewConditionsFieldIds = await uow.GetRepository<FieldViewCondition>().GetAllQueryFiltered(x => x.ParentFieldId == item.FieldId).AsNoTracking().Select(x => x.FieldId).ToListAsync();

                foreach (var fieldId in relatedFieldViewConditionsFieldIds)
                {
                    var relatedFieldToBeRejected = await uow.GetRepository<ServiceRequestFieldsValue>()
                        .GetAllQueryFiltered(x => x.FieldId == fieldId && x.ServiceRequestId == item.ServiceRequestId)
                        .AsNoTracking().ToListAsync();

                    dependentRejectedFields.AddRange(relatedFieldToBeRejected);
                }
            }

            foreach (var dependentRejectedField in dependentRejectedFields)
            {
                var exist = toReject.Any(x => x.FieldId == dependentRejectedField.FieldId);
                if (!exist)
                    toReject.Add(dependentRejectedField);
            }
            toApprove = toApprove.Where(c => !toReject.Any(reject => reject.FieldId == c.FieldId)).ToList();
            await ApproveFields(toApprove, actionId);
            await RejectFields(toReject, actionId);
        }
        private async Task HandleRequestMissingAction(List<ServiceRequestFieldsValue> existingField, IList<FieldValueDTO> fields, Guid actionId)
        {
            var missingdtos = fields.Where(c => c.IsApproved == false && c.FieldId.HasValue).Select(c => c.FieldId!.Value).ToList();

            var fieldIdsToShiftedToRejected = await SrvDropdown.GetDropdownFieldsGroup(missingdtos);
            missingdtos.AddRange(fieldIdsToShiftedToRejected);
            missingdtos = missingdtos.Distinct().ToList();

            var toBeMissing = existingField.Where(c => missingdtos.Contains(c.FieldId)).ToList();


            await MissingFields(toBeMissing, actionId);
        }
        private async Task RejectFields(List<ServiceRequestFieldsValue> toReject,Guid actionId)
        {

            foreach (var item in toReject)
            {
                item.IsApproved = false;
            }

            uow.GetRepository<ServiceRequestFieldsValue>().UpdateRange(toReject);

            var transactionlogs = toReject.Select(c => new FieldValueTransactionsLog()
            {
                ServiceRequestFieldsValueId = c.Id,
                NewValue = c.Value,
                OldValue = c.Value,
                TransactionsTypeId = TransactionTypeGuidIds.REJECT,
                ServiceActionId = actionId,
            });

            await uow.GetRepository<FieldValueTransactionsLog>().InsertRange(transactionlogs);




        }
        private async Task MissingFields(List<ServiceRequestFieldsValue> toReject,Guid actionId)
        {
            foreach (var item in toReject)
            {
                item.IsMissing = true;
                item.IsApproved = false;
            }
            uow.GetRepository<ServiceRequestFieldsValue>().UpdateRange(toReject);

            var transactionlogs = toReject.Select(c => new FieldValueTransactionsLog()
            {
                ServiceRequestFieldsValueId = c.Id,
                NewValue = c.Value,
                OldValue = c.Value,
                TransactionsTypeId = TransactionTypeGuidIds.REQUESTMISSING,
                ServiceActionId = actionId,

            });

            await uow.GetRepository<FieldValueTransactionsLog>().InsertRange(transactionlogs);
        }
        private async Task ApproveFields(List<ServiceRequestFieldsValue> toapprove,Guid actionId)
        {
            foreach (var item in toapprove)
            {
                item.IsApproved = true;
                item.IsMissing = null;
            }
            uow.GetRepository<ServiceRequestFieldsValue>().UpdateRange(toapprove);
            var transactionlogs = toapprove.Select(c => new FieldValueTransactionsLog()
            {
                ServiceRequestFieldsValueId = c.Id,
                NewValue = c.Value,
                OldValue = c.Value,
                TransactionsTypeId = TransactionTypeGuidIds.APPROVE,
                ServiceActionId = actionId,
            }).ToList();

            await uow.GetRepository<FieldValueTransactionsLog>().InsertRange(transactionlogs);
        }
        private async Task UnApproveFields(List<ServiceRequestFieldsValue> existingField, IList<FieldValueDTO> fields)
        {
            await Task.Run(() =>
            {
                var Unapproveddtos = fields.Select(c => c.FieldId);

                var toUnApprove =existingField.Where(c => Unapproveddtos.Contains(c.FieldId)).ToList();

                foreach (var item in toUnApprove)
                {
                    item.IsApproved = false;
                    item.IsMissing = null;
                }
                uow.GetRepository<ServiceRequestFieldsValue>().UpdateRange(toUnApprove);
            });
        }
        private async Task<IList<FieldValueDTO>> UpdateCustomFieldJsonSchemaValue(Guid requestId, IList<FieldValueDTO> fields)
        {

            var fieldIdsList = fields.Select(x => x.FieldId).Distinct().ToList();

            var list = await serviceScopeFactory.CreateScopedUow()
                                                .GetRepository<ServiceRequestFieldsValue>()
                                                .GetAllQueryFiltered(x => x.ServiceRequestId == requestId && fieldIdsList.Contains(x.FieldId))
                                                .Include(x => x.Field).AsNoTracking()
                                                .Where(x => x.Field!.FormGroupListId != null)
                                                .ToListAsync();


            var ServiceRequestFieldsValues = list.GroupBy(x => x.Field!.FieldTypeId)
                                                  .Select(x => new { FieldTypeId = x.Key, ServiceRequestFieldsValue = x.OrderByDescending(y => y.CreateDate.Date).FirstOrDefault() })
                                                  .Select(x => x.ServiceRequestFieldsValue)
                                                  .ToList();

            foreach (var ServiceRequestFieldsValue in ServiceRequestFieldsValues)
            {
                var field = fields.Where(x => x.FieldId == ServiceRequestFieldsValue!.FieldId).FirstOrDefault();

                if (field != null && !string.IsNullOrEmpty(field.Value) && !string.IsNullOrEmpty(ServiceRequestFieldsValue!.Value))
                {
                    var mergedData = SoftMerge(ServiceRequestFieldsValue.Value, field.Value);
                    field.Value = mergedData;
                }
            }

            return fields;
        }
        private async Task<List<ServiceRequestFieldsValue>> AddOrUpdateFields(IList<FieldValueDTO> fields, List<ServiceRequestFieldsValue> requestFieldsValues, Guid requestId, string lang,Guid actionId, List<FieldValueDTO>? approvedIntegrationFields=null)
        {
           
                var transactionLogs = new List<FieldValueTransactionsLog>();

                var existingField = requestFieldsValues.Where(c => fields.Select(c => c.FieldId).ToList().Contains(c.FieldId)).ToList();

                var UnApprovedFields = existingField.Where(c => !c.IsApproved).ToList();

                transactionLogs.AddRange(UnApprovedFields.Where(c => !c.IsApproved).Select(c => new FieldValueTransactionsLog()
                {
                    ServiceRequestFieldsValueId = c.Id,
                    NewValue = fields.First(y => y.FieldId == c.FieldId)?.Value,
                    OldValue = c.Value,
                    TransactionsTypeId = TransactionTypeGuidIds.UPDATE,
                    ServiceActionId = actionId,
                }).ToList());

                foreach (var field in UnApprovedFields)
                {
                    field.Value = fields.First(c => c.FieldId == field.FieldId).Value;
                    field.IsMissing = false;
                    if (approvedIntegrationFields != null && approvedIntegrationFields.Any(x => x.FieldId == field.FieldId))
                    {
                        field.IsApproved = true;
                    }
                    uow.GetRepository<ServiceRequestFieldsValue>().Update(field);
                }

                var fieldsIDs = requestFieldsValues.Select(c => c.FieldId).ToList();
                var newFields = fields.Where(c => c.FieldId.HasValue && !fieldsIDs.Contains(c.FieldId.Value)).ToList();

                var newDBFields = newFields.Select(field =>
            {
                bool isApproved = approvedIntegrationFields != null && approvedIntegrationFields.Any(x => x.FieldId == field.FieldId);

                return new ServiceRequestFieldsValue
                {
                    FieldId = field.FieldId!.Value,
                    Value = field.Value,
                    IsActive = true,
                    ServiceRequestId = requestId,
                    IsApproved = isApproved
                };
            }).ToList();

                if (newDBFields.Any())
                {
                    foreach (var newDBField in newDBFields)
                    {
                        uow.GetRepository<ServiceRequestFieldsValue>().Insert(newDBField);
                    }

                }

                transactionLogs.AddRange(
                    newDBFields.Select(c => new FieldValueTransactionsLog()
                    {
                        ServiceRequestFieldsValueId = c.Id,
                        NewValue = fields.First(y => y.FieldId == c.FieldId)?.Value,
                        OldValue = c.Value,
                        TransactionsTypeId = TransactionTypeGuidIds.INSERT,
                        ServiceActionId = actionId,
                    }).ToList()
                );
            await Task.Run(() =>
            {
                if (transactionLogs.Any())
                {
                    foreach (var transactionLog in transactionLogs)
                    {
                        uow.GetRepository<FieldValueTransactionsLog>().Insert(transactionLog);
                    }
                }
            });
            return newDBFields;
           
        }
        public async Task ReleaseVacancySeatIfExists(List<ServiceRequestFieldsValue> requestFields)
        {
            if ((requestFields == null || !requestFields.Any()))
            {
                return;
            }

            var vacancySeatField = requestFields!.FirstOrDefault(c =>
                c.Field?.MappingSystemField?.BackendName == "VacancySeatId");

            if ((vacancySeatField == null || string.IsNullOrWhiteSpace(vacancySeatField.Value)))
            {
                //throw new BusinessException(ExceptionMessage.lblNoAvailableVacancySeat);
                return;
            }

            if (!Guid.TryParse(vacancySeatField.Value, out Guid vacancySeatId) || vacancySeatId == Guid.Empty)
            {
                throw new BusinessException(ExceptionMessage.lblNoAvailableVacancySeat);
            }
            var ClearFieldValue = await uow.GetRepository<ServiceRequestFieldsValue>().GetByIDNonDeleted(vacancySeatField.Id);
            if (null==ClearFieldValue)
            {
                throw new BusinessException(ExceptionMessage.lblNoAvailableVacancySeat);
            }
            else
            {
                if (null!= ClearFieldValue)
                {
                    ClearFieldValue.Value = null;
                    ClearFieldValue.IsApproved = false;
                    uow.GetRepository<ServiceRequestFieldsValue>().Update(ClearFieldValue);
                }
            }
            


            await SrvVacancy.ReleaseVacancySeat(vacancySeatId);
        }
        private string SoftMerge(string oldJson, string newJson)
        {

            var mergedPartners = new List<Dictionary<string, object>>();

            var oldPartners = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(oldJson);

            var newPartners = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(newJson);

            foreach (var newPartner in newPartners!)
            {
                var index = newPartner["Index"].ToString();
                var oldPartner = oldPartners!.Find(p => p["Index"].ToString() == index);

                if (oldPartner != null)
                {
                    foreach (var kvp in newPartner)
                    {
                        oldPartner[kvp.Key] = kvp.Value;
                    }
                    mergedPartners.Add(oldPartner);
                }
                else
                {
                    mergedPartners.Add(newPartner);
                }
            }

            return JsonConvert.SerializeObject(mergedPartners);
        }

    }
}
