using System;
using System.Collections.Concurrent;
using System.Globalization;
using Azure.Core;
using Evaluation.DAL.Entities.ActionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Scholarship.Services.BusinessLayer.API.Services;


namespace Scholarship.Services.BusinessLayer.API
{
    public class  FormRenderBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, SrvUser SrvUser, UnitOfWork uow, LoggingServices loggingServices, SrvAccreditedUniversity SrvAccreditedUniversity, SrvSystemField SrvSystemField,
            IMapper mapper,  UserInfo userInfo,   RequestInfo _requestInfo, SrvServiceRequest SrvServiceRequest, SrvScholarship SrvScholarship
            , SrvAttachments SrvAttachments, SrvDropdown SrvDropdown,
                SrvField srvField, SrvStep srvStep, IServiceProvider serviceProvider)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
        {

        public async Task<ActionCustomDTO> GetActionSteps(ServiceAction action, Guid serviceId, Guid? requestId = null, Guid? scholarshipId = null)
        {
            string lang = _requestInfo.Lang;
            var result = mapper.Map<ActionCustomDTO>(action);

            var integrationFieldsToProcess = new ConcurrentBag<FieldValueDTO>();
            var schAttachmentIds = new ConcurrentBag<string>();
            var actionStepsIds = action.ActionStepsFields!.Select(c => c.StepId).Distinct().ToList();

            var stepsTask = srvStep.GetStepsByStepsIds(actionStepsIds);
            var hiddenFieldsTask = srvField.GetHiddenFields(action.ServiceId);
            var stepFieldsTask = srvField.GetFieldsByActionId(action.Id, action.ServiceId);
            var stepFieldListsTask = srvField.GetFieldsListByActionIdAsync(action.ServiceId);
            var requestFieldValuesTask = SrvServiceRequest.GetRequestFieldsValueAsync(requestId);

            Guid? studentUserId = requestId.HasValue
                ? await SrvServiceRequest.GetUserIdByRequestIdAsync(requestId.Value)
                : userInfo.UserId;

            await Task.WhenAll( hiddenFieldsTask, stepFieldsTask, stepFieldListsTask, requestFieldValuesTask);

            var steps = await stepsTask;//action.ActionStepsFields!.Select(x=>x.Step).Distinct().ToList();
            var hiddenFieldIds = await hiddenFieldsTask;
            var stepFields = await stepFieldsTask;
            var stepFieldsList = await stepFieldListsTask;
            var requestFieldValues = await requestFieldValuesTask;  //requestFieldValuesRslt ??  new List<ServiceRequestFieldsValue>();

            StudentUser? student = null;
            string studentQID = string.Empty;

            if (studentUserId.HasValue)
            {
                student = await SrvUser.GetStudentByIdAsync(studentUserId.Value);
                studentQID = ConstantKeys.TempQIDForTesting;  //student?.QID ?? string.Empty;  //"30663401929";
            }

            var stepDtoList = new List<StepDTO>();

            foreach (var step in steps)
            {
                // 1) Filter fields for this step
                var stepFieldsForStep = stepFields
                    .Where(f => action.ActionStepsFields!.Any(x =>
                                x.StepId == step.Id &&
                                x.FieldId == f.Id &&
                                !hiddenFieldIds.Contains(f.Id)))
                    .ToList();

                // 2) Group by form group
                var grouped = stepFieldsForStep
                    .GroupBy(f => new
                    {
                        f.FormGroupId,
                        Title = lang == "ar" ? f.FormGroup!.TitleAr : f.FormGroup!.TitleEn,
                        f.FormGroup.Order
                    })
                    .OrderBy(g => g.Key.Order);

                var formGroups = new List<FormGroupDTO>();

                foreach (var group in grouped)
                {
                    var fieldDtos = new List<FieldValueDTO>();

                    foreach (var field in group)
                    {
                        
                        var FieldAttributesTask = GetFieldAttributes(field, action, lang);
                        Task<string?>? jsonSchemaTask = null;

                        if (field.FormGroupListId.HasValue)
                        {
                            jsonSchemaTask =  srvField.GenerateJsonSchemaForFormGroupList(field.FormGroupListId.Value, stepFieldsList);
                               
                        }
                        var isEditable = action.ActionStepsFields!
                            .FirstOrDefault(x => x.FieldId == field.Id)?.IsEditable ?? true;

                        var fieldValue = requestFieldValues.FirstOrDefault(x => x.FieldId == field.Id);
                        var value = fieldValue?.Value;

                       
                        string fieldType = field.FieldType!.NameEn;


                        // If you ever need the RETURNBACK text mapping again, uncomment and adjust:
                        // if (action.ActionType.BackendName == ConstantKeys.ActionTypeKeys.RETURNBACK &&
                        //     field.DropDownTypeId != null &&
                        //     Guid.TryParse(value, out var parsedGuid))
                        // {
                        //     var dropdownValue = await SrvDropdown.GetDropDownValue(lang, parsedGuid, field.DropDownTypeId.Value, null);
                        //     value = dropdownValue != null
                        //         ? (lang == "ar" ? dropdownValue.TitleAr : dropdownValue.TitleEn)
                        //         : value;
                        // }

                        var fieldDto = new FieldValueDTO
                        {
                            FieldId = field.Id,
                            Value = value,
                            IsApproved = fieldValue?.IsApproved,
                            //Type = field.DropDownTypeId != null && action.ActionType.BackendName == ConstantKeys.ActionTypeKeys.RETURNBACK ? "text" : field.FieldType.NameEn,
                            Type = fieldType,
                            FormGroupId = field.FormGroupId,
                            FormGroupListId = field.FormGroupListId,
                            FormGroupName = group.Key.Title,
                            Row = field.Row,
                            Column = field.Column,
                            FieldName = lang == "ar" ? field.TitleAr : field.TitleEn,
                            FieldTooltip = lang == "ar" ? field.InfoAr : field.InfoEn,
                            ReadFromFieldId = field.ReadSystemFieldId,
                            DropDownTypeId = field.DropDownTypeId,
                            DropDownParentFieldId = field.DropDownParentFieldId,
                            ClassName = field.ClassName,
                            IsEditable = isEditable,
                            Attributes = (await FieldAttributesTask ?? [])!,
                            Conditions = (field.FieldViewConditions?.Select(c => new FieldViewConditionDTO
                            {
                                operators = c.operators,
                                FieldValue = c.FieldValue,
                                IsSufficient = c.IsSufficient,
                                ParentFieldId = c.ParentFieldId
                            }).ToList()  ?? [])!,
                            JsonSchema = jsonSchemaTask != null ? await jsonSchemaTask : null


                        };

                        if (string.IsNullOrWhiteSpace(value) &&
                            field.FieldAttributeValues?.Any(attr => attr.AttributeKey.StartsWith("Integration_")) == true)
                        {
                            integrationFieldsToProcess.Add(fieldDto);
                        }

                        fieldDtos.Add(fieldDto);
                    }

                    // Handle fields that read from another field (attachments)
                    if (fieldDtos.Any(x => x.ReadFromFieldId != null))
                    {
                        var attachmentsFromReadFields =
                            await HandleFieldsWithReadFromFieldIdAsync(
                                fieldDtos.Where(x => x.ReadFromFieldId != null).ToList(),
                                requestId,
                                scholarshipId);

                        foreach (var att in attachmentsFromReadFields)
                        {
                            schAttachmentIds.Add(att);
                        }
                    }

                    formGroups.Add(new FormGroupDTO
                    {
                        FormGroupName = group.Key.Title,
                        Order = group.Key.Order,
                        Fields = fieldDtos.Where(f => f.Visible != false).ToList()
                    });
                }

                var stepDto = mapper.Map<StepDTO>(step);
                stepDto.Title = lang == "ar" ? step.TitleAr : step.TitleEn;
                stepDto.FormGroups = formGroups;

                stepDtoList.Add(stepDto);
            }

            var allStepDtos = stepDtoList.ToArray();
            if (requestId != null && requestId != Guid.Empty)
            { 
            await ApplyContractSignatureIfNeededAsync(action, allStepDtos.ToList(), requestFieldValues, requestId.Value);
         
            }

            if (integrationFieldsToProcess.Any() && !string.IsNullOrEmpty(studentQID))
            {
                var updatedFields = await srvField.ProcessIntegrationFieldsAsync(integrationFieldsToProcess.ToList(), studentQID, requestId);

                foreach (var updated in updatedFields)
                {
                    var target = allStepDtos
                        .SelectMany(s => s.FormGroups)
                        .SelectMany(g => g.Fields)
                        .FirstOrDefault(f => f.FieldId == updated.FieldId);

                    if (target != null)
                    {
                        target.Value = updated.Value;
                        target.IsApproved = updated.IsApproved;
                        if (target.Type == "file")
                            target.Visible = false;
                    }
                }
            }

            result.Steps = allStepDtos.OrderBy(s => s.OrderNo).ToList();
            
            result.SchAttachmentIds = schAttachmentIds.ToList();

            return result;
        }
        private async Task ApplyContractSignatureIfNeededAsync(ServiceAction action,List<StepDTO> steps,List<ServiceRequestFieldsValue> requestFieldValues, Guid serviceRequestId) // ← was "requestId" missing; now passed in
        {
            var asf = action.ActionStepsFields!
                .FirstOrDefault(s => s.ActionStepFieldAttribute?.Any(a => a.AttributeKey == "ContractSignature") == true);
    
            if (asf == null  ) return ;
            var RequestFieldValue = requestFieldValues.Where(x => x.FieldId == asf.FieldId).FirstOrDefault();
            if (RequestFieldValue?.Value != null) return;
            // Parse system setting: "{{StudentSign}},120,80"
            var setting = await cacheDataProvider.GetSystemSettingValue(ConstantKeys.SystemSettings.ContractSignature);
            if (string.IsNullOrWhiteSpace(setting))
                throw new BusinessException("Missing system setting: ContractSignature");

            var parts = setting.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
                throw new BusinessException("Invalid ContractSignature format. Expected '{{Sign}},X,Y'");

            var placeholder = parts[0].Trim();
            if (!float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var x))
                throw new BusinessException("Invalid X coordinate in ContractSignature setting.");
            if (!float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
                throw new BusinessException("Invalid Y coordinate in ContractSignature setting.");

            var uow = serviceProvider.CreateScopedUow();
            var signatureImageBytes = await uow.GetRepository<UserPartyTypeSignature>()
                .GetAllQueryFiltered()
                .Include(s => s.UserPartyType)
                .Where(s => s.UserPartyType!.SignaturePlaceHolder == placeholder)
                .Select(s => s.Signature)
                .FirstOrDefaultAsync();

            if (signatureImageBytes == null || signatureImageBytes.Length == 0)
                throw new BusinessException($"Signature image for placeholder '{placeholder}' not found or empty.");

            var pdfFieldIdStr = asf.ActionStepFieldAttribute!
                .First(a => a.AttributeKey == "ContractSignature").AttributeValue;

                  var field=  await   srvField.GetFieldsByBackendNameAsync(pdfFieldIdStr!);
            if (null == field)
                throw new BusinessException($"ContractSignature attribute on field {asf.FieldId} must be a GUID (PDF field id).");

            var originalPdfAttachmentId = requestFieldValues.FirstOrDefault(v => v.FieldId == field.Id)?.Value;
            if (string.IsNullOrWhiteSpace(originalPdfAttachmentId)) return ; 

            var pdfBytes = await SrvAttachments.GetBytesByAttachmentIdAsync(originalPdfAttachmentId);

            var stampedPdfBytes = StampSignatureOnLastPage(pdfBytes, signatureImageBytes, x, y);
           
            string p46 =  Convert.ToBase64String(stampedPdfBytes); 

            var targetField = steps
                .SelectMany(s => s.FormGroups)
                .SelectMany(g => g.Fields)
                .FirstOrDefault(f => f.FieldId == asf.FieldId);

            if (targetField != null)
            {
                targetField!.Value = p46;
            }
         
        }
        public static byte[] StampSignatureOnLastPage( byte[] pdfBytes, byte[] imageBytes, float x,float y,float? width = null, float? height = null)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentException("pdfBytes is null or empty.", nameof(pdfBytes));
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("imageBytes is null or empty.", nameof(imageBytes));

            using var src = new MemoryStream(pdfBytes, writable: false);
            using var dst = new MemoryStream();

            using var reader = new PdfReader(src);
            // Keeping streams open is optional for MemoryStream, but harmless:
            reader.SetCloseStream(false);
 // ensures dst is usable after closing

            try
            {

                using var writer = new PdfWriter(dst);
                writer.SetCloseStream(false);

                using var pdfDoc = new PdfDocument(reader, writer);

                int last = pdfDoc.GetNumberOfPages();
                if (last < 1)
                    throw new InvalidOperationException("Source PDF has no pages.");

                var page = pdfDoc.GetPage(last);

                // Decode the image
                var imgData = ImageDataFactory.Create(imageBytes);

                // Decide target size
                float targetW = width ?? imgData.GetWidth();
                float targetH = height ?? imgData.GetHeight();

                // Draw directly with PdfCanvas (kernel only)
                var rect = new Rectangle(x, y, targetW, targetH);
                var pdfCanvas = new PdfCanvas(page, true);
                // false = don't preserve aspect ratio (fill the rect). Set true if you want to keep ratio.
                pdfCanvas.AddImageFittedIntoRectangle(imgData, rect, false);

                pdfDoc.Close();
                return dst.ToArray();
            }
            catch (PdfException ex)
            {
                throw new InvalidOperationException($"iText7 PdfException while stamping: {ex.Message}", ex);
            }
        }


      
        private async Task<List<string>> HandleFieldsWithReadFromFieldIdAsync( List<FieldValueDTO> fields,Guid? requestId, Guid? ScholarshipId)
        {
          List<string> attachmentList = new List<string>();
            if (fields == null || !fields.Any())
                return attachmentList;

        
            var fieldsWithReadFrom = fields.Where(f => f.ReadFromFieldId.HasValue && string.IsNullOrWhiteSpace(f.Value)).ToList();

            if (!fieldsWithReadFrom.Any())
                return attachmentList;

            var readFromFieldIds = fieldsWithReadFrom
                                    .Select(f => f.ReadFromFieldId!.Value)
                                    .Distinct()
                                    .ToList();

            // 2. Get SystemFields
            var systemFields = await SrvSystemField.GetSystemFieldsByIds(readFromFieldIds);

            if (systemFields == null || !systemFields.Any())
                return attachmentList;
            Guid? userId = requestId != null
                            ? await SrvServiceRequest.GetUserIdByRequestIdAsync(requestId.Value)
                            : userInfo.UserId;

            if (userId == null)
                return attachmentList;

            var userProfile = await SrvUser.GetStudentByIdAsync(userId.Value);

            var schFieldValues = (ScholarshipId!=Guid.Empty && ScholarshipId !=null)
                ? await SrvScholarship.GetSchFieldValues(ScholarshipId.Value)
                : new List<SchFieldValue>();

            // 3. Process each field
            foreach (var field in fieldsWithReadFrom)
            {
                var sysField = systemFields.FirstOrDefault(sf => sf.Id == field.ReadFromFieldId);
                if (sysField == null)
                    continue;

                if (sysField.SystemTable?.BackendName?.ToLower() == "scholarship")
                {
                    var FieldValue = schFieldValues?.FirstOrDefault(x=>x.SystemFieldId== sysField.Id);
                    if (FieldValue !=null)
                    {
                        if(sysField.FieldType.BackendName=="file" || sysField.FieldType.BackendName == "fileV2")
                        {
                            attachmentList.Add(FieldValue.Value!);
                        }
                        if(sysField.FieldType.BackendName=="list")
                        {
                            var fieldList = await srvField.GetFieldListByFieldId(field.FieldId!.Value);
                            var (extractedJson, attachments) = ExtractFieldValues(fieldList, FieldValue.Value!);
                            field.Value = extractedJson;
                            attachmentList.AddRange(attachments);

                        }
                      
                        else
                        {
                            field.Value = FieldValue.Value;
                        }
                       
                    }
                }
                else if (sysField.SystemTable?.BackendName?.ToLower() == "studentprofile" && userProfile != null)
                {
                    var userProfileValue =await GetStudentProfileFieldValue(userProfile, sysField.BackendName);
                    if (!string.IsNullOrWhiteSpace(userProfileValue))
                    {
                        field.Value = userProfileValue;
                        field.IsApproved = true;
                    }
                }
            }
            return attachmentList;
        }

        public (string extractedJson, List<string> attachmentList) ExtractFieldValues(List<Field> fieldList, string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || fieldList == null || !fieldList.Any())
                return ("[]", new List<string>());

            var jsonDataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData)
                                ?? new List<Dictionary<string, object>>();

            var mergedList = new List<Dictionary<string, object>>();
            var attachmentList = new List<string>();

            foreach (var entry in jsonDataList)
            {
                var entryDict = new Dictionary<string, object>();

                foreach (var item in entry)
                {
                    if (item.Key.Trim() == "Index")
                    {
                        entryDict["Index"] = item.Value?.ToString() ?? "[]";
                        continue;
                    }

                    if (item.Key.Trim() == "IsOld")
                    {
                        entryDict["IsOld"] = true;
                        continue;
                    }
                    if (!Guid.TryParse(item.Key.ToString(), out var fieldId))
                        continue;

                    var matchingField = fieldList.FirstOrDefault(f => f.ReadSystemFieldId == fieldId);
                    if (matchingField == null || matchingField.ReadSystemField?.Id == null || matchingField.ReadSystemField?.Id == Guid.Empty)
                        continue;

                    var key = matchingField.Id.ToString();
                    var valueString = item.Value?.ToString();
                    var value = string.IsNullOrEmpty(valueString) ? "[]" : valueString;

                    if (matchingField.FieldType?.BackendName == "file" || matchingField.FieldType?.BackendName == "fileV2")
                    {
                        try
                        {
                            var fileList = JsonConvert.DeserializeObject<List<string>>(valueString!);
                            if (fileList != null && fileList.Any())
                            {
                                attachmentList.AddRange(fileList);
                            }
                            else
                            {
                                attachmentList.Add(valueString!);
                            }
                        }
                        catch
                        {
                            attachmentList.Add(valueString!);
                        }
                    }

                    entryDict[key] = value;
                }
                if (!entryDict.ContainsKey("IsOld"))
                {
                    entryDict["IsOld"] = true;
                }

                mergedList.Add(entryDict);
            }
          
        
            return (JsonConvert.SerializeObject(mergedList), attachmentList);
        }

        private async Task<string> GetStudentProfileFieldValue(StudentUser student, string backendName)
        {
            return backendName switch
            {
                "QID" => student.QID!,
                "FullName" => student.FullNameAr!,
                "GenderId" => student.UserGenderId.ToString()!,
                "AccountMobile" => student.Mobile!,
                "NationalityId" => (await SrvAccreditedUniversity.GetCountryBycode(student!.NationalityCode!))?.Id.ToString()!,
                "AccountEmail" => student.Email!,
                "DOB" => student.DOB?.ToString("dd-MM-yyyy")!,
                "Age" => student.DOB.HasValue ? CalculateAge(student.DOB.Value).ToString()! : null!,
                _ => null!,
            };
        }


        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
        public async Task<ActionCustomDTO> GetApprovedAndMissingFields(ServiceRequest request, ServiceAction action)
        {
            string lang = _requestInfo.Lang;

            var result = mapper.Map<ActionCustomDTO>(action);

            // Fetch necessary data in parallel to improve performance
            var stepsTask = srvStep.GetDefaultStepByServiceId(request.ServiceId);
            var hiddenFieldsTask = srvField.GetHiddenFields(request.ServiceId);
            var stepFieldsListTask = srvField.GetFieldsListByActionIdAsync(action.ServiceId);

            await Task.WhenAll(stepsTask, hiddenFieldsTask, stepFieldsListTask);

            var hiddenFields = hiddenFieldsTask.Result ?? new List<Guid>();
            var stepFieldsList = stepFieldsListTask.Result;
            var actionStep = await stepsTask ?? throw new BusinessException(ConstantKeys.ExceptionMessage.lblNoDefaultStepFound);

            // Fetch approved and non-missing fields in a single query
            var fieldValuesRaw = await serviceScopeFactory.CreateScopedUow()
                                .GetRepository<ServiceRequestFieldsValue>()
                                .GetAllQueryFiltered()
                                .Include(f => f.Field)
                                    .ThenInclude(f => f!.FieldType)
                                .Include(f => f.Field!.FieldAttributeValues)
                                .Include(f => f.Field!.FieldViewConditions)
                                .Include(f => f.Field!.FormGroup)
                                .AsSplitQuery()
                                .Where(f => f.ServiceRequestId == request.Id &&
                                            (f.IsApproved || f.IsMissing==false) &&
                                            !hiddenFields.Contains(f.FieldId))
                                .ToListAsync();

            var fieldValueTasks = fieldValuesRaw.Select(async f => new FieldValueDTO
            {
                FieldId = f.FieldId,
                Value = f.Value,
                Type = f.Field!.FieldType?.BackendName ?? "Unknown",
                FormGroupId = f.Field.FormGroupId,
                FormGroupName = lang == "ar" ? f.Field.FormGroup?.TitleAr : f.Field.FormGroup?.TitleEn,
                FormGroupOrderNo = f.Field.FormGroup?.Order ?? 0,
                Row = f.Field.Row,
                Column = f.Field.Column,
                FieldName = lang == "ar" ? f.Field.TitleAr : f.Field.TitleEn,
                FieldTooltip = lang == "ar" ? f.Field.InfoAr : f.Field.InfoEn,
                ClassName = f.Field.ClassName,
                DropDownTypeId = f.Field.DropDownTypeId,
                DropDownParentFieldId = f.Field.DropDownParentFieldId,
                Attributes = (f.Field!.FieldAttributeValues!.Select(attr => new AttributeDTO
                {
                    Name = attr.AttributeKey,
                    Value = attr.AttributeValue,
                    Message = lang == "ar" ? attr.MessageAr : attr.MessageEn
                }).ToList() ?? [])!,
                Conditions = (f.Field.FieldViewConditions!.Select(cond => new FieldViewConditionDTO
                {
                    operators = cond.operators,
                    FieldValue = cond.FieldValue,
                    IsSufficient = cond.IsSufficient,
                    ParentFieldId = cond.ParentFieldId
                }).ToList() ?? [])!,
                IsApproved = f.IsApproved,
                JsonSchema = f.Field.FormGroupListId is not null
                     ? await srvField.GenerateJsonSchemaForFormGroupList(f.Field.FormGroupListId, stepFieldsList)
                     : null
            }).ToList();

            // Wait for all tasks to complete
            var fieldValues = await Task.WhenAll(fieldValueTasks);


            var groupedFields = fieldValues
                .GroupBy(f => new { f.FormGroupId, f.FormGroupName, f.FormGroupOrderNo })
                .Select(g => new FormGroupDTO
                {
                    FormGroupName = g.Key.FormGroupName ?? "Uncategorized",
                    Order = g.Key.FormGroupOrderNo,
                    Fields = g.ToList()
                })
                .OrderBy(g => g.Order)
                .ToList();

            result.Steps = new List<StepDTO>
            {
                new StepDTO
                {
                    FormGroups = groupedFields,
                    Title = lang == "ar" ? actionStep.TitleAr : actionStep.TitleEn
                }
            };

            return result;
        }


        public async Task<ActionCustomDTO> GetMissingFields(ServiceRequest request, ServiceAction action)
        {
            string lang = _requestInfo.Lang;
            var result = mapper.Map<ActionCustomDTO>(action);

            var stepsTask = srvStep.GetDefaultStepByServiceId(request.ServiceId);
            var hiddenFieldsTask = srvField.GetHiddenFields(request.ServiceId);
            var stepFieldsListTask = srvField.GetFieldsListByActionIdAsync(request.ServiceId);

            await Task.WhenAll(stepsTask, hiddenFieldsTask, stepFieldsListTask);

            var hiddenFieldsIds = hiddenFieldsTask.Result ?? new List<Guid>();
            var stepFieldsList = stepFieldsListTask.Result;
            var actionStep = await stepsTask ?? throw new BusinessException(ConstantKeys.ExceptionMessage.lblNoDefaultStepFound);

            var fieldValuesRaw = await serviceScopeFactory.CreateScopedUow()
                .GetRepository<ServiceRequestFieldsValue>()
                .GetAllQueryFiltered()
                .Include(f => f.Field)
                    .ThenInclude(f => f!.FieldType)
                .Include(f => f.Field!.FieldAttributeValues)
                .Include(f => f.Field!.FieldViewConditions)
                .Include(f => f.Field!.FormGroup)
                .AsSplitQuery()
                .Where(f => f.ServiceRequestId == request.Id && f.IsMissing == true && !hiddenFieldsIds.Contains(f.FieldId))
                .ToListAsync();

            var fieldValueTasks = fieldValuesRaw.Select(async f => new FieldValueDTO
            {
                FieldId = f.FieldId,
                Value = f.Value,
                Type = f.Field!.FieldType?.BackendName ?? "Unknown",
                FormGroupId = f.Field.FormGroupId,
                FormGroupName = lang == "ar" ? f.Field.FormGroup?.TitleAr : f.Field.FormGroup?.TitleEn,
                FormGroupOrderNo = f.Field.FormGroup?.Order ?? 0,
                Row = f.Field.Row,
                Column = f.Field.Column,
                FieldName = lang == "ar" ? f.Field.TitleAr : f.Field.TitleEn,
                FieldTooltip = lang == "ar" ? f.Field.InfoAr : f.Field.InfoEn,
                ClassName = f.Field.ClassName,
                DropDownTypeId = f.Field.DropDownTypeId,
                DropDownParentFieldId = f.Field.DropDownParentFieldId,
                Attributes = (f.Field.FieldAttributeValues!.Select(attr => new AttributeDTO
                {
                    Name = attr.AttributeKey,
                    Value = attr.AttributeValue,
                    Message = lang == "ar" ? attr.MessageAr : attr.MessageEn
                }).ToList() ?? [])!,
                Conditions = (f.Field.FieldViewConditions!.Select(cond => new FieldViewConditionDTO
                {
                    operators = cond.operators,
                    FieldValue = cond.FieldValue,
                    IsSufficient = cond.IsSufficient,
                    ParentFieldId = cond.ParentFieldId
                }).ToList()??[])!,
                IsApproved = f.IsApproved,
                JsonSchema = f.Field.FormGroupListId is not null
                             ? await srvField.GenerateJsonSchemaForFormGroupList(f.Field.FormGroupListId, stepFieldsList)
                             : null
            }).ToList();

            var fieldValues = await Task.WhenAll(fieldValueTasks);

            var groupedFields = fieldValues
                .GroupBy(f => new { f.FormGroupId, f.FormGroupName, f.FormGroupOrderNo })
                .Select(g => new FormGroupDTO
                {
                    FormGroupName = g.Key.FormGroupName ?? "Uncategorized",
                    Order = g.Key.FormGroupOrderNo,
                    Fields = g.ToList()
                })
                .OrderBy(g => g.Order)
                .ToList();

            result.Steps = new List<StepDTO>
                {
                    new StepDTO
                    {
                        FormGroups = groupedFields,
                        Title = lang == "ar" ? actionStep.TitleAr : actionStep.TitleEn
                    }
                };

            return result;
        }

      
        private async Task<List<AttributeDTO>> GetFieldAttributes(Field field, ServiceAction action, string lang)
        {
            var actionStepAttributes = (action?.ActionStepsFields ?? new List<ActionStepField>())
                .Where(x => x.FieldId == field.Id)
                .SelectMany(x => x.ActionStepFieldAttribute ?? new List<ActionStepFieldAttribute>())
                .Select(a => new AttributeDTO
                {
                    Name = a.AttributeKey,
                    Value = a.AttributeValue,
                    Message = lang == "ar" ? a.MessageAr : a.MessageEn
                });

            var fieldAttributes = (field.FieldAttributeValues ?? new List<FieldAttributeValue>())
                .Select(f => new AttributeDTO
                {
                    Name = f.AttributeKey,
                    Value = f.AttributeValue,
                    Message = lang == "ar" ? f.MessageAr : f.MessageEn
                });

            return actionStepAttributes!
                .Union(fieldAttributes)
                .GroupBy(attr => attr.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .Select(g =>
                {
                    var hasActionStepAttribute = (action?.ActionStepsFields ?? new List<ActionStepField>())
                        .SelectMany(x => x.ActionStepFieldAttribute! ?? new List<ActionStepFieldAttribute>())
                        .Any(a => a.AttributeKey == g.Key)!;

                    return hasActionStepAttribute
                        ? g.FirstOrDefault()  
                        : g.First();
                })
                .ToList();
        }


    }
}
