using AutoMapper;
using Azure;
using Azure.Core;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.Services.Extensions;
using Evaluation.Services.Models.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Reflection.PortableExecutable;
using static Evaluation.SharedHelper.Enums.ConstantKeys;



namespace Evaluation.Services.BusinessLayer.API
{
    public class  FormRenderBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, SrvUser _srvUser, UnitOfWork uow, LoggingServices loggingServices, SrvAction _srvAction,
				  IMapper mapper,UserInfo userInfo,   RequestInfo _requestInfo, SrvServiceRequest _srvServiceRequest, EvaluationRequestService _evaluationRequestService, SrvService _srvService, SrvActionStatusConfiguration _srvActionStatusConfiguration
			, SrvAttachments _srvAttachments, SrvDropdown _srvDropdown, SystemModuleSrv systemModuleSrv, SrvStatus _srvStatus, SrvAssignment _srvAssignment,
				SrvField _srvField, IServiceProvider serviceProvider , ServiceRequestBL serviceRequestBL, PlanServiceRequestServices planServiceRequestServices)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
        {
		public async Task<ActionCustomDTO> GetActionField(ServiceAction action,Guid serviceId,RequestType requestType,Guid? requestId = null,Guid? PlanId = null)
		{
			string lang = _requestInfo.Lang;
			var result = mapper.Map<ActionCustomDTO>(action);

			var integrationFieldsToProcess = new ConcurrentBag<FieldValueDTO>();
			var schAttachmentIds = new ConcurrentBag<string>();

			var hiddenFieldsTask = _srvField.GetHiddenFields(action.ServiceId);
			var actionFieldsTask = _srvField.GetFieldsByActionId(action.Id, action.ServiceId);
			var actionFieldListsTask = _srvField.GetFieldsListByActionIdAsync(action.ServiceId);
			var requestFieldValuesTask = _srvServiceRequest.GetRequestFieldsValueAsync(requestId);

			await Task.WhenAll(hiddenFieldsTask, actionFieldsTask, actionFieldListsTask, requestFieldValuesTask);

			var hiddenFieldIds = await hiddenFieldsTask;
			var actionFields = await actionFieldsTask;
			var actionFieldsList = await actionFieldListsTask;
			var requestFieldValues = await requestFieldValuesTask ?? new List<ServiceRequestFieldsValue>();

			var visibleFieldsForAction = actionFields;

			var grouped = visibleFieldsForAction
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
					var fieldAttributesTask = GetFieldAttributes(field, action, lang);
					Task<string?>? jsonSchemaTask = null;

					if (field.FormGroupListId.HasValue)
					{
						jsonSchemaTask = _srvField.GenerateJsonSchemaForFormGroupList(
							field.FormGroupListId.Value,
							actionFieldsList);
					}

					var isEditable = action.ActionFields!
						.FirstOrDefault(x => x.FieldId == field.Id)?.IsEditable ?? true;

					var fieldValue = requestFieldValues.FirstOrDefault(x => x.FieldId == field.Id);
					var value = fieldValue?.Value;

					string fieldType = field.FieldType!.NameEn;
					var attributes = (await fieldAttributesTask ?? [])!;

					var fieldDto = new FieldValueDTO
					{
						FieldId = field.Id,
						formId = field.EvalFormId,
						Value = value,
						IsApproved = fieldValue?.IsApproved,
						Type = fieldType,
						FormGroupId = field.FormGroupId,
						FormGroupListId = field.FormGroupListId,
						FormGroupName = group.Key.Title,
						Row = field.Row,
						Column = field.Column,
						FieldName = lang == "ar" ? field.TitleAr : field.TitleEn,
						FieldTooltip = lang == "ar" ? field.InfoAr : field.InfoEn,
						ReadFromFieldId = field.ReadFieldId,
						DropDownTypeId = field.DropDownTypeId,
						DropDownParentFieldId = field.DropDownParentFieldId,
						ClassName = field.ClassName,
						IsEditable = isEditable,
						Attributes = attributes,
						Conditions = (field.FieldViewConditions?.Select(c => new FieldViewConditionDTO
						{
							operators = c.operators,
							FieldValue = c.FieldValue,
							IsSufficient = c.IsSufficient,
							ParentFieldId = c.ParentFieldId
						}).ToList() ?? [])!,
						JsonSchema = jsonSchemaTask != null ? await jsonSchemaTask : null
					};

					if (string.IsNullOrWhiteSpace(value) &&
						fieldDto.Attributes?.Any(attr => attr!.Name != null && attr.Name.StartsWith("Integration_")) == true)
					{
						integrationFieldsToProcess.Add(fieldDto);
					}

					fieldDtos.Add(fieldDto);
				}

				if (fieldDtos.Any(x => x.ReadFromFieldId != null))
				{
					var attachmentsFromReadFields = await HandleFieldsWithReadFromFieldIdAsync(
						fieldDtos.Where(x => x.ReadFromFieldId != null).ToList(),
						requestId,
						PlanId);

					foreach (var att in attachmentsFromReadFields)
						schAttachmentIds.Add(att);
				}

				formGroups.Add(new FormGroupDTO
				{
					FormGroupName = group.Key.Title,
					Order = group.Key.Order,
					Fields = fieldDtos.Where(f => f.Visible != false).ToList()
				});
			}

			if (integrationFieldsToProcess.Any() && requestId.HasValue && requestId.Value != Guid.Empty)
			{
				var SchoolID = "";

				if (!string.IsNullOrWhiteSpace(SchoolID))
				{
					var updatedFields = await _srvField.ProcessIntegrationFieldsAsync(
						integrationFieldsToProcess.ToList(),
						SchoolID,
						requestId);

					var allFields = formGroups.SelectMany(g => g.Fields).ToList();

					foreach (var updated in updatedFields)
					{
						var target = allFields.FirstOrDefault(f => f.FieldId == updated.FieldId);
						if (target != null)
						{
							target.Value = updated.Value;
							target.IsApproved = updated.IsApproved;
						}
					}
				}
			}

			result.FormGroups = formGroups.OrderBy(fg => fg.Order).ToList();
			result.SchAttachmentIds = schAttachmentIds.ToList();

			return result;
		}
		private async Task<List<string>> HandleFieldsWithReadFromFieldIdAsync(List<FieldValueDTO> fields, Guid? requestId, Guid? PlanId)
		{
			var attachmentList = new List<string>();

			if (fields == null || fields.Count == 0)
				return attachmentList;

			var fieldsWithReadFrom = fields
				.Where(f =>f != null && f.ReadFromFieldId.HasValue && string.IsNullOrWhiteSpace(f.Value))
				.ToList();

			if (fieldsWithReadFrom.Count == 0)
				return attachmentList;

			foreach (var field in fieldsWithReadFrom)
			{
				if (!field.ReadFromFieldId.HasValue)
					continue;

				var sourceField = await _srvField.GetFieldsByIdsAsync(field.ReadFromFieldId.Value);
				if (sourceField?.FieldType?.BackendName == null)
					continue;

				if (sourceField.FieldType.BackendName==FieldTypeConstant.EvaluationPlan)
				{
					if (!PlanId.HasValue || PlanId.Value == Guid.Empty)
						return attachmentList;

					var planJsonResult =
						await planServiceRequestServices.GetPlanJsonById(PlanId!.Value);

					if (!planJsonResult.IsSuccess || string.IsNullOrWhiteSpace(planJsonResult.Value))
						continue;

					field.Value = planJsonResult.Value;
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

					var matchingField = fieldList.FirstOrDefault(f => f.ReadFieldId == fieldId);
					if (matchingField == null || matchingField.ReadField?.Id == null || matchingField.ReadField?.Id == Guid.Empty)
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
		//private async Task<string> GetSchoolProfileFieldValue(SchoolUser School, string backendName)
		//{
		//	return backendName switch
		//	{
		//		"QID" => School.QID!,
		//		"FullName" => School.FullNameAr!,
		//		"GenderId" => School.UserGenderId.ToString()!,
		//		"AccountMobile" => School.Mobile!,
		//		"NationalityId" => (await SrvAccreditedUniversity.GetCountryBycode(School!.NationalityCode!))?.Id.ToString()!,
		//		"AccountEmail" => School.Email!,
		//		"DOB" => School.DOB?.ToString("dd-MM-yyyy")!,
		//		"Age" => School.DOB.HasValue ? CalculateAge(School.DOB.Value).ToString()! : null!,
		//		_ => null!,
		//	};
		//}
	
		public async Task<ActionCustomDTO> GetApprovedAndMissingFields(ServiceRequest request, ServiceAction action)
		{
			string lang = _requestInfo.Lang;

			var result = action.Adapt<ActionCustomDTO>();

			// Fetch necessary data in parallel to improve performance
			var hiddenFieldsTask = _srvField.GetHiddenFields(request.ServiceId);
			var ActionFieldsListTask = _srvField.GetFieldsListByActionIdAsync(action.ServiceId);

			await Task.WhenAll(hiddenFieldsTask, ActionFieldsListTask);

			var hiddenFields = hiddenFieldsTask.Result ?? new List<Guid>();
			var stepFieldsList = ActionFieldsListTask.Result;

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
								.Where(f => f.RefId == request.Id &&
											(f.IsApproved || f.IsMissing == false) &&
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
					 ? await _srvField.GenerateJsonSchemaForFormGroupList(f.Field.FormGroupListId, stepFieldsList)
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

			result.FormGroups = groupedFields;
				

			return result;
		}
		public async Task<ActionCustomDTO> GetMissingFields(ServiceRequest request, ServiceAction action)
		{
			string lang = _requestInfo.Lang;
			var result = action.Adapt<ActionCustomDTO>();

			var hiddenFieldsTask = _srvField.GetHiddenFields(request.ServiceId);
			var ActionFieldsListTask = _srvField.GetFieldsListByActionIdAsync(request.ServiceId);

			await Task.WhenAll( hiddenFieldsTask, ActionFieldsListTask);

			var hiddenFieldsIds = hiddenFieldsTask.Result ?? new List<Guid>();
			var ActionFieldsList = ActionFieldsListTask.Result;

			var fieldValuesRaw = await serviceScopeFactory.CreateScopedUow()
				.GetRepository<ServiceRequestFieldsValue>()
				.GetAllQueryFiltered()
				.Include(f => f.Field)
					.ThenInclude(f => f!.FieldType)
				.Include(f => f.Field!.FieldAttributeValues)
				.Include(f => f.Field!.FieldViewConditions)
				.Include(f => f.Field!.FormGroup)
				.AsSplitQuery()
				.Where(f => f.RefId == request.Id && f.IsMissing == true && !hiddenFieldsIds.Contains(f.FieldId))
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
				}).ToList() ?? [])!,
				IsApproved = f.IsApproved,
				JsonSchema = f.Field.FormGroupListId is not null
							 ? await _srvField.GenerateJsonSchemaForFormGroupList(f.Field.FormGroupListId, ActionFieldsList)
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

			result.FormGroups = groupedFields;

			return result;
		}
		private async Task<List<AttributeDTO>> GetFieldAttributes(Field field, ServiceAction action, string lang)
		{
			var actionStepAttributes = (action?.ActionFields ?? new List<ActionField>())
				.Where(x => x.FieldId == field.Id)
				.SelectMany(x => x.ActionFieldAttribute ?? new List<ActionFieldAttribute>())
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
					var hasActionStepAttribute = (action?.ActionFields ?? new List<ActionField>())
						.SelectMany(x => x.ActionFieldAttribute! ?? new List<ActionFieldAttribute>())
						.Any(a => a.AttributeKey == g.Key)!;

					return hasActionStepAttribute
						? g.FirstOrDefault()
						: g.First();
				})
				.ToList();
		}

		public async Task<IList<CssClassesDTO>> GetCssClassesAsync()
		{
			return await _srvField.GetCssClasses();
		}
		public async Task<ServiceDTO> GetServiceAsync(Guid serviceId)
		{
			var lang = _requestInfo.Lang;
			var userId = userInfo.UserId;

			var service = await _srvService.GetServiceDetailsAsync(serviceId, lang);

			if (service.Initialservice &&
				service.EligableScholarShips == null &&
				service.Actions != null &&
				service.Actions.Any() &&
				service.Actions.Count == 1)
			{
				var action = service.Actions.First();
				service.ServiceRequestDTO = await GetActionFieldAsync(serviceId, action.BakendName, null, null);
			}

			return service;
		}
		public async Task<ServiceDTO> GetCreatePlanService()
		{
			var lang = _requestInfo.Lang;
			var DepartementId = requestInfo.DepId!.Value;// Guid.Parse("1B8F5ADE-37D0-4D77-A780-CA3FF3EC0F43");
			//var service = await _srvService.GetCreatePlanServiceDetailsAsync(DepartementId, lang);
			var service = await _srvService.GetServiceDetailsByModuleTypeAsync(DepartementId, ModuleType.EvaluationPlan, lang, initialService: true);


			if (service.Actions != null &&service.Actions.Any() &&service.Actions.Count == 1)
			{
				var action = service.Actions.First();
				service.ServiceRequestDTO = await GetActionFieldAsync(service.Id!.Value, action.BakendName, null, null);
			}

			return service;
		}
		public async Task<ServiceDTO> GetPlanService( Guid serviceId,Guid planId)
		{
			var lang = _requestInfo.Lang;
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");
			var DepartementId = requestInfo.DepId!.Value;// Guid.Parse("1B8F5ADE-37D0-4D77-A780-CA3FF3EC0F43");
			var service = await _srvService.GetServiceDetailsByModuleTypeAsync(DepartementId, ModuleType.EvaluationPlan, lang, serviceId: serviceId, initialService: false, planId);
			if (service.Actions != null && service.Actions.Any() && service.Actions.Count == 1)
			{
				var action = service.Actions.First();
				service.ServiceRequestDTO = await GetActionFieldAsync(service.Id!.Value, action.BakendName, null, planId);
			}

			return service;
		}
		public async Task<ServiceDTO> GetCreateEvaluationPartyService( Guid serviceId)
		{
			var lang = _requestInfo.Lang;
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");
			var DepartementId = requestInfo.DepId!.Value;// Guid.Parse("1B8F5ADE-37D0-4D77-A780-CA3FF3EC0F43");
			var service = await _srvService.GetServiceDetailsByModuleTypeAsync(DepartementId, ModuleType.EvaluationParty, lang, serviceId: serviceId);

			if (service.Actions != null && service.Actions.Any() && service.Actions.Count == 1)
			{
				var action = service.Actions.First();
				service.ServiceRequestDTO = await GetActionFieldAsync(service.Id!.Value, action.BakendName, null, null);
			}

			return service;
		}
		public async Task<List<ServiceDTO>> GetServicesWebAppAsync(string? moduleName)
		{
			var userId = userInfo.UserId;
			return await _srvService.GetServicesbyDepartementAndPartyType(moduleName, userId);
		}

		public async Task<List<DropDownValueDTO>> GetDropDownValuesByTypeIdAsync(
			Guid? requestId,
			Guid? schId,
			Guid dropDownTypeId)
		{
			ServiceRequest? requestObj = null;
			if (requestId.HasValue)
			{
				requestObj = await _srvServiceRequest.GetRequestByIdAsync(requestId.Value)
							 ?? throw new BusinessException($"Service request with ID {requestId} not found.");
			}

			var OrgTreetId = _srvServiceRequest.GetOrgTreeRequestId(requestObj)
							?? throw new BusinessException(ExceptionMessage.UserNotFound);

			var lang = _requestInfo.Lang;

			var values = await _srvDropdown.GetDropDownValuesByDropDownTypeId(
				dropDownTypeId,
				OrgTreetId,
				schId);

			return values;
		}

		public async Task<List<DropDownValueDTO>> GetDropDownValuesByIdAsync(
			Guid? requestId,
			Guid? schId,
			Guid dropDownTypeId,
			Guid value)
		{
			ServiceRequest? requestObj = null;
			if (requestId.HasValue)
			{
				requestObj = await _srvServiceRequest.GetRequestByIdAsync(requestId.Value)
							 ?? throw new BusinessException($"Service request with ID {requestId} not found.");
			}

			var OrgTreetId = _srvServiceRequest.GetOrgTreeRequestId(requestObj)
							?? throw new BusinessException(ExceptionMessage.UserNotFound);

			var lang = _requestInfo.Lang;

			var values = await _srvDropdown.GetDropDownValuesByDropDownTypeId(
				dropDownTypeId,
				OrgTreetId,
				schId,
				null,
				value);

			return values;
		}

		

		public async Task<IEnumerable<TempLateDocDTO>> GetActionTemplatesByStatusAsync(Guid requestId)
		{
			var userId = userInfo.UserId;

			if (!await _srvServiceRequest.HasAccessToRequestAsync(requestId, userId!.Value))
			{
				throw new UnauthorizedAccessException("You do not have permission to view this request.");
			}

			return await _srvActionStatusConfiguration.GetActionTemplatesByStatus(requestId);
		}

		public async Task<ServiceRequestDTO> GetActionFieldAsync(Guid serviceId,string actionBackendKey,Guid? requestId = null,Guid? PlanId = null)
		{
			string lang = _requestInfo.Lang;
			var userId = userInfo.UserId ?? throw new BusinessException(ExceptionMessage.UserNotFound); 

			var serviceTask = _srvService.GetServiceById(serviceId);
			var actionTask = _srvAction.GetActionByBackendNameAsync(serviceId, actionBackendKey);
			var statusIdTask = requestId is null
				? _srvStatus.GetInitialStatusIdByServiceId(serviceId)
				: Task.FromResult<Guid?>(null);
			

			await Task.WhenAll(serviceTask, actionTask, statusIdTask );

			var service = await serviceTask ?? throw new BusinessException(ExceptionMessage.InvalidRequest);
			var action = await actionTask
						 ?? throw new BusinessException($"Action with key {actionBackendKey} not found for service {service.NameEn}.");
			ServiceRequest? requestObj = null;

			var requestType = systemModuleSrv.GetRequestType(service);
			if (requestId is not null && requestId != Guid.Empty)
			{
				requestObj = await serviceRequestBL.GetRequestUnifiedAsync(requestId.Value, requestType)
							?? throw new BusinessException(ExceptionMessage.lblRequestNotValid);

				if (requestType == RequestType.Evaluation)
				{
					var moduleId = service.SystemModuleId;
					var canAccess = await _evaluationRequestService.ValidateMinistryUserAccessAsync(userId, moduleId, requestId.Value);
					//if (!canAccess)
					//	throw new UnauthorizedAccessException("You do not have permission to view this request.");
				}
				else
				{
					var canAccess = await _srvServiceRequest.HasAccessToRequestAsync(requestId.Value, userId);
					//if (!canAccess)
					//	throw new UnauthorizedAccessException("You do not have permission to view this request.");
				}

				PlanId = requestObj.PlanId ?? PlanId;
			}

		
			if (!service.Initialservice && action.IsInitialAction && PlanId == null)
				//throw new BusinessException(ExceptionMessage.MissingPlan);

			if (requestId is null && !action.IsInitialAction)
				throw new BusinessException(ExceptionMessage.InvalidRequest);

			var isValidActionConditionsTask =
				_srvActionStatusConfiguration.ValidateActionConditions(action.Id, requestId, PlanId);

			PlanId = requestId is not null ? requestObj!.PlanId : PlanId;

			var actionCustomTask = action.ActionType!.BackendName switch
			{
				ActionTypeKeys.RequestDataChange => GetApprovedAndMissingFields(requestObj!, action),
				ActionTypeKeys.SubmitMissingData => GetMissingFields(requestObj!, action),
				_ => GetActionField(action, service.Id, requestType, requestId, PlanId)
			};

			var statusId = requestId is not null
				? requestObj?.StatusId ?? throw new BusinessException(ExceptionMessage.InvalidRequest)
				: await statusIdTask ?? throw new BusinessException(ExceptionMessage.lblNoServiceStatusFound);

			var OrgTreeId = requestId != null ? requestObj!.OrgTreeId : userId;

			if (OrgTreeId == Guid.Empty)
				throw new BusinessException(ExceptionMessage.UserNotFound);

			var dropDownTask = _srvDropdown.GetDropDownValuesForAction(
				PlanId ?? requestObj?.PlanId,
				OrgTreeId,
				action.Id,
				null,
				lang,
				requestId);

			var actionConfigTask =
				_srvActionStatusConfiguration.GetActionConfigurationDetails(action.Id, statusId);

			var isValidActionConditions = await isValidActionConditionsTask;
			if (!isValidActionConditions)
				throw new BusinessException(ExceptionMessage.lblActionConditionsNotMet);

			var actionCustom = await actionCustomTask;

			Task<List<AttachementDTO?>> schAttachmentsTask =
				(actionCustom.SchAttachmentIds is { Count: > 0 })
					? _srvAttachments.GetAttachmentsByIdsAsync(actionCustom.SchAttachmentIds)
					: Task.FromResult<List<AttachementDTO?>>(null!);

			var actionConfig = await actionConfigTask
							   ?? throw new BusinessException(ExceptionMessage.lblNoActionStatusConfiguration);

			var assignUsers = new List<AssignUserDTO>();
			if (action.ActionType.BackendName is ActionTypeKeys.Assign or ActionTypeKeys.Approve_And_Assign)
			{
				assignUsers = await _srvAssignment.GetAssignedUsers(
					requestObj!,
					action.Id,
					actionConfig.ShowIsDefaultAssigner);
			}

			actionCustom.BakendName = action.BackendName;
			actionCustom.IsRemark = actionConfig.IsRemark;
			actionCustom.IsOtherAttachment = actionConfig.IsOtherAttachment;
			actionCustom.IsRemarkRequired = actionConfig.IsRemarkRequired;
			actionCustom.IsOtherAttachmentRequired = actionConfig.IsOtherAttachmentRequired;
			actionCustom.RemarkLabel = lang == "ar" ? actionConfig.RemarkLabelAr : actionConfig.RemarkLabelEn;
			actionCustom.AttachmentLabel = lang == "ar" ? actionConfig.AttachmentLabelAr : actionConfig.AttachmentLabelEn;
			actionCustom.AssignUsers = assignUsers;

			var requestDto = new ServiceRequestDTO
			{
				ActionCustom = actionCustom,
				//DropDownValues = await dropDownTask,
				//SchAttachments = schAttachmentsTask != null ? await schAttachmentsTask : null
			};

			return requestDto;
		}
	}
}
