using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.SystemLog;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.EvaluationForm;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Evaluation.Services.BusinessLayer.API.TeamMemberBL;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Evaluation.DAL.ConstantKeys;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Models.API
{

    public class PerformActionBL(
        IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvNotification SrvNotification, SrvUser SrvUser, 
        LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, SrvField SrvField, SrvAction SrvAction, 
        SrvStatus SrvStatus, SrvAssignment SrvAssignment, SrvDropdown SrvDropdown, SrvActionTransactionsLog SrvActionTransactionsLog, 
        SrvService SrvService, AssignmentBL _assignmentBL, EvaluationFormBL _EvaluationFormBL ,SrvServiceRequest SrvServiceRequest, PlanServiceRequestServices planServiceRequestServices, SrvAttachments SrvAttachments, IServiceProvider serviceProvider,RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
    {

		public async Task<PerforActionResponseDTO> PerformAction(ServiceRequest application, RequestType RequestType, Service serviceObj, IList<FieldValueDTO> Fields, string actionname, List<AssignUserDTO> users, List<EvalTeamRequestDto> teamUsers, string Remarks, bool saveAsDraft = false)
		{
			string lang = _requestInfo.Lang;
			var result = new PerforActionResponseDTO();

			var userId = userInfo.UserId;
			if (application == null)
				throw new BusinessException(ExceptionMessage.lblRequestFieldsMissing);

			var actiondb = await SrvAction.GetActionByBackendNameAsync(serviceObj.Id, actionname, true);

			var actionConfigurationTask = SrvAction.GetActionConfigurationAsync(actiondb!.Id, application.StatusId);
			var existingFieldsTask = SrvServiceRequest.GetRequestFieldsValueAsync(application.Id);

			//Fields = await UpdateCustomFieldJsonSchemaValue(application.Id, Fields);

			var currentStatus = application.StatusId;

			var existingFields = await existingFieldsTask;
			var approvedFieldIds = existingFields
									.Where(f => f.IsApproved == true)
									.Select(f => f.FieldId)
									.ToList();

			var editableFields = actiondb.ActionFields!
									 .Where(f =>
										 (f.IsEditable == true && !approvedFieldIds.Contains(f.FieldId)) || actiondb.IsInitialAction ||
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
			var actionTransactionlog = await SrvActionTransactionsLog.UpdateStatusAndLogAction(RequestType,application, actiondb.Id, actionConfiguration.NextStatusId, Remarks, saveAsDraft);


			var integrationFieldsDtos = actiondb.ActionFields!
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


			
			var FieldsToUpdates = Fields.Where(c => editableFields.Contains(c.FieldId!.Value)).ToList();
			switch (actiondb.ActionType.BackendName)
			{
				case ActionTypeKeys.Approve:
					await ApproveUneditedFieldsAsync(application.ServiceId, RequestType, application.Id, FieldsToUpdates, existingFields, Fields, lang, actiondb.Id);
					break;

				case ActionTypeKeys.Info:
				case ActionTypeKeys.INFO_WITH_DRAFT:
					var updatedfields = await PrepareAndUpdateFields(application.ServiceId, RequestType, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
					break;
				case ActionTypeKeys.INFO_Override_Approve:
					await UnApproveFields(existingFields, Fields);
					await PrepareAndUpdateFields(application.ServiceId, RequestType, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
					break;
				case ActionTypeKeys.EDIT:
					await PrepareAndUpdateFields(application.ServiceId, RequestType, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
					break;
				case ActionTypeKeys.Assign:
					await SrvAssignment.PerformAssignAction(application.Id, users!);
					await AddOrUpdateFields(Fields, RequestType, existingFields, application.Id, lang, actiondb.Id);
					break;
				case ActionTypeKeys.ASSIGNT_TEAM:
					await _assignmentBL.AddedRequestAssignment(
						application.Id,
						teamUsers
					);
					break;
				case ActionTypeKeys.Approve_And_Assign:
					await SrvAssignment.PerformAssignAction(application.Id, users!);
					await ApproveUneditedFieldsAsync(application.ServiceId, RequestType, application.Id, FieldsToUpdates, existingFields, Fields, lang, actiondb.Id);
					break;

				case ActionTypeKeys.Reject:
				case ActionTypeKeys.RETURNBACK:
					await HandleRejectAction(existingFields, Fields, actiondb.Id);
					break;
				case ActionTypeKeys.RequestDataChange:
					await HandleRequestMissingAction(existingFields, Fields, actiondb.Id);
					break;
				case ActionTypeKeys.CreateEvaluationPlan:
					{
						{
							var updatedFields = await PrepareAndUpdateFields(application.ServiceId,RequestType,application.Id,FieldsToUpdates,existingFields,lang,actiondb.Id);

							existingFields.AddRange(updatedFields);
						}

						var planField = existingFields
							.Where(x => x.IsApproved)
							.FirstOrDefault(x =>
								x.Field?.FieldType?.BackendName == FieldTypeConstant.EvaluationPlan &&
								!string.IsNullOrWhiteSpace(x.Value)
							);

						if (planField == null)
							break;

						try
						{
							var dto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planField.Value);
							if (dto == null) throw new BusinessException("Invalid Evaluation Plan data");

							await planServiceRequestServices.InsertOrUpdatePlan(dto);

						}

						catch (Exception ex)
						{

						}
						
						break;
					}
				case ActionTypeKeys.CLOSE_AND_UPDATE_PLAN:
					{
						if (FieldsToUpdates.Count > 0)
						{
							var updatedFields = await PrepareAndUpdateFields(application.ServiceId,RequestType,application.Id,FieldsToUpdates,existingFields,lang,actiondb.Id);

							existingFields.AddRange(updatedFields);
						}

						var planField = existingFields
							.Where(x => x.IsApproved)
							.FirstOrDefault(x =>
								x.Field?.FieldType?.BackendName == FieldTypeConstant.EvaluationPlan &&
								!string.IsNullOrWhiteSpace(x.Value)
							);

						if (planField == null)
							break;


						try
						{
							var dto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planField.Value);
							if (dto == null) throw new BusinessException("Invalid Evaluation Plan data");

							await planServiceRequestServices.InsertOrUpdatePlan(dto);

							if (dto == null) throw new BusinessException("Invalid Evaluation Plan data");

							await planServiceRequestServices.InsertOrUpdatePlan(dto);

						}

						catch (Exception ex)
						{

						}
						//						var dto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(
						//	planField.Value.ToString()
						//);
						

						break;
					}
				case ActionTypeKeys.CLOSE_AND_UPDATE_FORM:
					{
						if (FieldsToUpdates.Count > 0)
						{
							var updatedFields = await PrepareAndUpdateFields(application.ServiceId,RequestType,application.Id,FieldsToUpdates,existingFields,lang,actiondb.Id);

							existingFields.AddRange(updatedFields);
						}

						var FormField = existingFields
							.Where(x => x.IsApproved)
							.FirstOrDefault(x =>
								x.Field?.FieldType?.BackendName == FieldTypeConstant.Evl_Form &&
								!string.IsNullOrWhiteSpace(x.Value)
							);

						if (FormField == null)
							break;


						var dto = JsonConvert.DeserializeObject<EvaluationFormDto>(FormField.Value);

						if (dto == null) throw new BusinessException("Invalid Evaluation Plan data");

						await _EvaluationFormBL.SaveEvaluationForm(dto);

						break;
					}
				case ActionTypeKeys.CLOSE_AND_DELETE_PLAN:
					{
						if (FieldsToUpdates.Count > 0)
						{
							var updatedFields = await PrepareAndUpdateFields(application.ServiceId,RequestType,application.Id,FieldsToUpdates,existingFields,lang,actiondb.Id);

							existingFields.AddRange(updatedFields);
						}

						var planField = existingFields
							.Where(x => x.IsApproved)
							.FirstOrDefault(x =>
								x.Field?.FieldType?.BackendName == FieldTypeConstant.EvaluationPlan &&
								!string.IsNullOrWhiteSpace(x.Value)
							);

						if (planField == null)
							break;


						//var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planField.Value.ToString());
						var dto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planField.Value);

						//						var dto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(
						//	planField.Value.ToString()
						//);
						if (dto == null) throw new BusinessException("Invalid Evaluation Plan data");

						await planServiceRequestServices.DeletePlanDraft(dto.Id);

						break;
					}

				//case ActionTypeKeys.CloseAndUpdate:
				//	if (FieldsToUpdates.Count > 0)
				//	{
				//		var updatedfiels = await AddOrUpdateFields(FieldsToUpdates, existingFields, application.Id, lang, actiondb.Id);
				//		existingFields.AddRange(updatedfiels);
				//	}
				//	//await ApproveFields(existingFields.Where(x => Fields.Any(f => f.FieldId == x.FieldId)).ToList(), actiondb.Id);
				//	var ApprovedOrActionFields = existingFields.Where(x => x.IsApproved == true || Fields.Any(f => f.FieldId == x.FieldId) || x.Field?.MappingSystemField?.BackendName == "EntityContractId" || x.Field?.MappingSystemField?.BackendName == "SectorId").ToList();

				//	await srvScholarship.UpdateScholarship(serviceObj, application, actiondb, ApprovedOrActionFields, lang);
				//	break;
				case ActionTypeKeys.Close:
					await AddOrUpdateFields(FieldsToUpdates, RequestType, existingFields, application.Id, lang, actiondb.Id);
					break;
				case ActionTypeKeys.UPDATE_ITEGRATION_FIELDS:
					await PrepareAndUpdateFields(application.ServiceId, RequestType, application.Id, FieldsToUpdates.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
					break;
				default:
					await PrepareAndUpdateFields(application.ServiceId, RequestType, application.Id, Fields.ToList(), existingFields, lang, actiondb.Id, approvedIntegrationFields);
					break;
			}


			result.request = application;
			result.actiondb = actiondb;
			result.actionlog = actionTransactionlog;

			var status = SrvStatus.GetStatusById(currentStatus);

			if (serviceObj.IsAutoAssignEnabled == true && !saveAsDraft)
			{

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
					//bool AutoAssign = await SrvAssignment.PerformAutoAssign(application, Assignaction.ServiceActionId);
					//if (AutoAssign)
					//{
					//	await SrvActionTransactionsLog.UpdateStatusAndLogAction(
					//		application, Assignaction.ServiceActionId, Assignaction.NextStatusId, Remarks, saveAsDraft);
					//}
				}
			}

			//if (application != null && actiondb.SchStatusId != null && actiondb.ActionType.BackendName != ActionTypeKeys.CloseAndUpdate && actiondb.ActionType.BackendName != ActionTypeKeys.CreateScholarship)
			//{
			//	await srvScholarship.UpdateStatusScholarship(application, actiondb.SchStatusId.Value);
			//}


			return result;
		}
		private Guid? TryGetGuidFieldValue(Guid fieldId, IList<FieldValueDTO> primaryList, IList<ServiceRequestFieldsValue> fallbackList)
		{
			var valueStr = primaryList.FirstOrDefault(x => x.FieldId == fieldId)?.Value
						?? fallbackList.FirstOrDefault(x => x.FieldId == fieldId)?.Value;

			return Guid.TryParse(valueStr, out var result) ? result : (Guid?)null;
		}
		private async Task<List<ServiceRequestFieldsValue>> ApproveUneditedFieldsAsync(Guid serviceId, RequestType RequestType, Guid applicationId, List<FieldValueDTO> FieldsToUpdate, List<ServiceRequestFieldsValue> existingFields, IList<FieldValueDTO> allFields, string lang, Guid actionId)
		{
			List<ServiceRequestFieldsValue> updatedFields = new();

			if (FieldsToUpdate.Count > 0)
			{
				updatedFields = await PrepareAndUpdateFields(serviceId, RequestType, applicationId, FieldsToUpdate, existingFields, lang, actionId);
				existingFields.AddRange(updatedFields);
			}

			var editedFieldIds = new List<Guid>(FieldsToUpdate.Select(f => f.FieldId!.Value));

			var fieldsToApprove = existingFields
				.Where(x =>
					allFields.Any(f => f.FieldId == x.FieldId) &&
					!editedFieldIds.Contains(x.FieldId))
				.ToList();

			await ApproveFields(fieldsToApprove, actionId);

			return updatedFields;
		}
		private async Task<(IList<FieldValueDTO> AllFields, List<FieldValueDTO> ApprovedFields)> UpdateIntegrationFieldValuesAsync(Guid requestId, string actionTypeBackendName, List<FieldValueDTO>? integrationFieldsDtos, IList<FieldValueDTO>? allFields, List<ServiceRequestFieldsValue>? existingFields)
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
				var userId = requestId != Guid.Empty ? await SrvServiceRequest.GetOrgTreeIdByRequestIdAsync(requestId) : userInfo.UserId;
				if (userId == null)
					userId = userInfo.UserId;

				if (userId != null)
				{
					var student = await SrvUser.GetStudentByIdAsync(userId.Value);

					var studentQID = ""; // Replace with student.QID if available    // "30663401929";

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
		private async Task<List<ServiceRequestFieldsValue>> PrepareAndUpdateFields(Guid serviceId, RequestType RequestType, Guid requestId, List<FieldValueDTO> fields, List<ServiceRequestFieldsValue> existingFields, string lang, Guid actionId, List<FieldValueDTO>? approvedIntegrationFields = null)
		{
			if (fields == null || fields.Count == 0)
				return existingFields;


			var updatedFiels = await AddOrUpdateFields(fields, RequestType, existingFields, requestId, lang, actionId, approvedIntegrationFields!);
			return updatedFiels;
		}
		private async Task HandleRejectAction(List<ServiceRequestFieldsValue> existingField, IList<FieldValueDTO> fields, Guid actionId)
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
						.GetAllQueryFiltered(x => x.FieldId == fieldId && x.RefId == item.RefId)
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
		private async Task RejectFields(List<ServiceRequestFieldsValue> toReject, Guid actionId)
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
		private async Task MissingFields(List<ServiceRequestFieldsValue> toReject, Guid actionId)
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
		private async Task ApproveFields(List<ServiceRequestFieldsValue> toapprove, Guid actionId)
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

				var toUnApprove = existingField.Where(c => Unapproveddtos.Contains(c.FieldId)).ToList();

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
												.GetAllQueryFiltered(x => x.RefId == requestId && fieldIdsList.Contains(x.FieldId))
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
		private async Task<List<ServiceRequestFieldsValue>> AddOrUpdateFields(IList<FieldValueDTO> fields,RequestType RequestType, List<ServiceRequestFieldsValue> requestFieldsValues, Guid requestId, string lang, Guid actionId, List<FieldValueDTO>? approvedIntegrationFields = null)
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
					RefId = requestId,
					RequestType = RequestType.ToString(),
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
