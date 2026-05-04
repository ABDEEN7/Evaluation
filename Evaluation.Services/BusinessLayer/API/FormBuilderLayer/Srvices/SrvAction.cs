using AutoMapper;
using AutoMapper.Internal;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
#pragma warning disable CS8620

    public class SrvAction (SrvField SrvField, IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
    {

		public async Task<ServiceAction?> GetActionByBackendNameAsync(Guid serviceId, string backendName, bool includeActionFields = false)
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
			using var scope = serviceScopeFactory.CreateScopedUow();
			var action = await scope
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
			using var scope = serviceScopeFactory.CreateScopedUow();

			var fields = await scope
				.GetRepository<ActionField>()
				.GetAllQueryFiltered(x => x.ServiceActionId == actionId)
				.Include(x => x.Field)
				.AsNoTracking()
				.ToListAsync();

			return fields;
		}
		public async Task<List<ActionTemplateDoc>> GetActionDocumentsAsync(Guid actionId)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();
			var documents = await scope
				.GetRepository<ActionTemplateDoc>()
				.GetAllQueryFiltered(x => x.ServiceActionId == actionId)
				.AsNoTracking()
				.ToListAsync();

			return documents;
		}
		public async Task<List<Evaluation.SharedHelper.Models.Api.PartyTypeDTOs.PartyTypeDTO>> GetActionPartyTypesAsync(Guid actionId)
		{

			using var scope = serviceScopeFactory.CreateScopedUow();

			var partyTypes = await scope
				.GetRepository<ActionPartyType>()
				.GetAllQueryFiltered(x => x.ServiceActionId == actionId)
				.Include(x => x.PartyType)
				.AsNoTracking()
				.ToListAsync();

			return partyTypes.Select(pt => MapToPartyTypeDTO(pt.PartyType!)).ToList();
		}

		#region ValidateActionAndActionField
		public async Task<IList<FieldValueDTO>> ValidateActionAndActionFieldAsync(ServiceRequest? application,  IList<FieldValueDTO> allFields, string ActionRemarks, List<IFormFile> otherAttachement, Service serviceObj, Guid applicationStatusId, ServiceAction actionObj, List<FileFieldDTO> fileFields, bool saveAsDraft = false)
		{
			string Lang = requestInfo.Lang;
			if (null == serviceObj && null == actionObj)
			{
				throw new BusinessException(ConstantKeys.ExceptionMessage.IncompleteRequest);
			}

			//var studentTask = SrvUser.GetByStudentIDActiveNonDeleted(StudentId);

			var actionTask = GetActionByBackendNameAsync(serviceObj!.Id, actionObj.BackendName);
			using var scope = serviceScopeFactory.CreateScopedUow();
			using var scope1 = serviceScopeFactory.CreateScopedUow();

			var attributesTask = scope
													.GetRepository<FieldAttributeValue>()
													.GetAllQueryFiltered()
													.Include(c => c.Field!.FieldType)
													.Include(c => c.Field!.FieldViewConditions)
													.AsSplitQuery()
													.Where(c => allFields.Select(f => f.FieldId).ToList().Contains(c.FieldId)).AsNoTracking()
													.ToListAsync();

			var ActionFieldAttributesTask = scope1
												.GetRepository<ActionField>()
												.GetAllQueryFiltered()
												.Where(c => allFields.Select(f => f.FieldId).Contains(c.FieldId) && c.ServiceActionId == actionObj.Id)
												.SelectMany(c => c.ActionFieldAttribute!)
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

				//var student = await studentTask;
				var validationErrorsTask = ValidateFieldsAttributesAsync(fieldsToValidate, allFields, fileFields, fieldAttributesToValidate, application?.OrgTreeId, Lang, application?.PlanId);

				await Task.WhenAll(validationErrorsTask, validateRemarksTask);

				var (validationErrors, updatedFields) = await validationErrorsTask;
				if (validationErrors.Any(e => !string.IsNullOrWhiteSpace(e.error)))
					throw new BusinessException(validationErrors);

				ApplyUpdatedFields(allFields, updatedFields);
			}
			else if (saveAsDraft)
			{
				var filledFields = allFields.Where(f => f.FieldId.HasValue && !string.IsNullOrWhiteSpace(f.Value) && f.Value != "[]").ToList();

				if (filledFields.Any())
				{
					var mergedAttributes = MergeAttributes(attributesTask.Result, ActionFieldAttributesTask.Result, Lang);
					var fieldAttributesToValidate = mergedAttributes
						.Where(attr => filledFields.Select(f => f.FieldId).Contains(attr.FieldId))
						.ToList();

					//var student = await studentTask;
					var validationErrorsTask = ValidateFieldsAttributesAsync(filledFields, allFields, fileFields, fieldAttributesToValidate, application?.OrgTreeId, Lang, application?.PlanId);

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
			using var scope = serviceScopeFactory.CreateScopedUow();

			var ActionConfiguration = await scope
				  .GetRepository<ActionStatusConfiguration>().GetAllQueryFiltered().Include(x => x.Notifications)
				  .FirstOrDefaultAsync(c => c.ServiceActionId == actionId && c.CurrentStatusId == currentStatusId);

			return ActionConfiguration;
		}
		private async Task<bool> ValidateActionFieldsAsync(IList<FieldValueDTO> fields, Guid actionId, Guid? ActionTypeId, Guid? applicationId)
		{
			if (ActionTypeId ==ActionTypeIds.RequestDataChange || ActionTypeId == ActionTypeIds.SubmitMissingData) { return false; }
			else
			{
				using var scope = serviceScopeFactory.CreateScopedUow();

				using var scope1 = serviceScopeFactory.CreateScopedUow();

				var MissingField =await scope.GetRepository<ServiceRequestFieldsValue>()
										.GetAllQueryFiltered()
										.AsNoTracking()
										.Where(x => x.RefId == applicationId && x.IsMissing == false)
										.Select(c => c.FieldId).ToListAsync();



				var actionFieldIdsList = scope1.GetRepository<ActionField>()
										.GetAllQueryFiltered()
										.AsNoTracking()
										.Where(c => c.ServiceActionId == actionId)
										.Select(c => c.FieldId);

				var invalidFieldsList = fields.Where(c => !actionFieldIdsList.Contains(c.FieldId!.Value) && !MissingField.Contains(c.FieldId.Value)).ToList();

				return invalidFieldsList.Any();
			}
		}
		public async Task<List<Field?>> GetActionMappedFieldsAsync(Guid actionId, string RelatedFieldsGroupBackendName)
		{
			using var scope = serviceScopeFactory.CreateScopedUow();

			var actionMappedFieldIdsList = await scope
											.GetRepository<ActionField>()
											.GetAllQueryFiltered()
											.Include(c => c.Field)
											.Include(c => c.Field!.MappingField)
											.AsSplitQuery()
											.AsNoTracking()
											.Where(c => c.ServiceActionId == actionId)
											.Select(c => c.Field)
											.ToListAsync();

			return actionMappedFieldIdsList;

		}
		public async Task<(List<SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO> errors, IList<FieldValueDTO> updatedFields)> ValidateFieldsAttributesAsync(IList<FieldValueDTO> fields, IList<FieldValueDTO> allFields, IList<FileFieldDTO> files, List<FieldAttributeValue> fullAttributes, Guid? OrgTreeId, string lang, Guid? PlanId)
		{
			var listFieldsTask = ValidateListFieldsAsync(fields, allFields, files, OrgTreeId, lang, PlanId);
			var simpleFieldsTask = ValidateSimpleFieldsAsync(fields, allFields, files, fullAttributes, lang);

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
		private async Task<(List<SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO> errors, IList<FieldValueDTO> updatedFields)> ValidateListFieldsAsync(IList<FieldValueDTO> fields, IList<FieldValueDTO> allFields, IList<FileFieldDTO> files, Guid? OrgTreeId, string lang, Guid? PlanId)
		{
			var validator = new FieldValidatorBL(cacheDataProvider);
			var errors = new List<SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO>();
			var fieldsIds = fields.Select(x => x.FieldId).ToList();
			using var scope = serviceScopeFactory.CreateScopedUow();

			var FieldwithJasonSchema = await scope
				.GetRepository<Field>()
				.GetAllQueryFiltered(c => fieldsIds.Contains(c.Id))
				.Include(c => c.FieldAttributeValues)
				.Include(c => c.MappingField)
				.AsSplitQuery()
				.Where(c => c.FormGroupListId != null && c.MappingFieldId != null)
				.ToListAsync();

			if (FieldwithJasonSchema.Any())
			{
				foreach (var item in FieldwithJasonSchema)
				{
					var fildList = await SrvField.GetFieldListByFieldId(item.Id);
					var FieldValue = fields.FirstOrDefault(x => x.FieldId == item.Id)?.Value;
					var listDicts = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(FieldValue!) ?? new();


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

				
				}
			}

			return (errors, fields);
		}
		private async Task<List<SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO>> ValidateSimpleFieldsAsync(IList<FieldValueDTO> fields, IList<FieldValueDTO> allFields, IList<FileFieldDTO> files, List<FieldAttributeValue> fullAttributes, string lang)
		{
			var validator = new FieldValidatorBL(cacheDataProvider);
			var errors = new List<SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO>();

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
								errors.AddRange(WrapErrors(item.FieldId, await validator.ValidateList(item, files, attributes, lang)));
								break;
						}
					}
				}

			}


			return errors;
		}
		private List<SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO> WrapErrors(Guid? fieldId, IList<string> messages)
		{
			return messages.Select(msg => new SharedHelper.Models.Api.FormBuilderDTO.FieldErrorDTO { fieldId = fieldId, error = msg }).ToList();
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
				throw new BusinessException(ExceptionMessage.lblRemarksRequired);
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
