using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.EvaluationRequestLayer;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Globalization;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.BusinessLayer.API;

public class EvaluationRequestService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo,
	SrvField SrvField,
	SrvAction SrvAction,
	SrvUser srvUser,
	SrvActionStatusConfiguration srvActionStatusConfiguration,
	SystemModuleSrv SrvSystemModule,
	SrvDropdown srvDropdown,
	SchoolRepository schoolRepository,
	SrvPartyType SrvPartyType,
	SrvActionTransactionsLog SrvActionTransactionsLog,
	SrvStatus SrvStatus, SrvEvaluationParty srvEvaluationParty


	) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<EvaluationRequest>> GetEvaluationRequests()
    {
        return unitOfWork.GetRepository<EvaluationRequest>()
                    .GetAllActiveNonDeleted()
                    .Include(d => d.Plan)
                    .Include(d => d.OrgTree)
                    .Include(d => d.DepEvaluationType)
                    .ToList();
    }


	public async Task<WebAppEvaluationRequestsDTO> GetEvaluationRequestsAsync(Guid userId, FilterRequestsDTO model)
	{
		string lang = requestInfo!.Lang;
		model.ModuleName = "/evaluation-plan-request";

		using var uow = serviceScopeFactory.CreateScopedUow();

		var moduleTask = SrvSystemModule.GetSystemModuleByRoutingAsync(model.ModuleName);
		var userTask = srvUser.GetByIDActiveNonDeleted(userId);
		var timeFormatTask = cacheDataProvider.GetSystemSettingValue(SystemSettings.ShortTimeFormat);
		var dateFormatTask = cacheDataProvider.GetSystemSettingValue(SystemSettings.DateFormat);

		await Task.WhenAll(moduleTask, userTask, timeFormatTask, dateFormatTask);

		var module = moduleTask.Result;
		var user = userTask.Result;
		var timeFormat = timeFormatTask.Result;
		var dateFormat = dateFormatTask.Result;

		if (user == null || module == null)
			throw new BusinessException(ExceptionMessage.UserInfoNotFound);

		var isMinistry = user is MinistryUser;

		var query = await GetDepEvaluationRequestsAsync(
			uow, userId, module, lang, timeFormat, dateFormat);

		var result = await FilteredEvaluationRequestsAsync(uow, isMinistry, query, model);

		//await UpdateRequestStatusesAsync(result.Data, module.Id);

		return result;
	}
	private async Task<IQueryable<EvaluationRequestDTO>> GetDepEvaluationRequestsAsync(UnitOfWork uow, Guid userId, SystemModule module, string lang, string timeFormat, string dateFormat)
	{
		var permissionTasks = new
		{
			IsAllowedToViewAllRequests =
				SrvPartyType.IsAllowedToViewAllRequestsAsync(userId, module.Id),

			IsAllowedToViewAllRequestsWithoutFiltration =
				SrvPartyType.IsAllowedToViewAllRequestsWitoutFilterationAsync(userId, module.Id),

			UserPartyTypeData =
				SrvPartyType.GetUserPartyTypeData(userInfo.UserId!, module.Id)
		};

		IQueryable<EvaluationRequest> baseQuery = uow
			.GetRepository<EvaluationRequest>()
			.GetAllActiveNonDeleted()
			.Include(x => x.Service)
			.Include(x => x.ServiceStatus)
			.Include(x => x.OrgTree)
			.Include(x => x.DepEvaluationType)
			.Include(x => x.Plan)
				.ThenInclude(p => p!.PlanStatus);

		baseQuery = baseQuery.AsSplitQuery()
			.Where(x => x.Service!.SystemModuleId == module.Id);

		var permissions = new
		{
			IsAllowedToViewAllRequests = await permissionTasks.IsAllowedToViewAllRequests,
			IsAllowedToViewAllRequestsWithoutFiltration = await permissionTasks.IsAllowedToViewAllRequestsWithoutFiltration,
			UserPartyTypeData = await permissionTasks.UserPartyTypeData
		};

		//if (!permissions.IsAllowedToViewAllRequestsWithoutFiltration)
		//{
		//	baseQuery = ApplyUserAccessFiltersForEvaluationRequests(
		//		baseQuery,
		//		permissions.UserPartyTypeData,
		//		userId,
		//		permissions.IsAllowedToViewAllRequests
		//	);
		//}

		return baseQuery.Select(x => new EvaluationRequestDTO
		{
			Id = x.Id,
			ServiceId = x.ServiceId,
			Service = lang == "ar" ? x.Service!.NameAr : x.Service!.NameEn,
			icon = x.Service!.Icon,
			RequestNumber = "1234",
			StatusId = x.ServiceStatusId,
			Status = lang == "ar" ? x.ServiceStatus!.NameAr : x.ServiceStatus!.NameEn,
			StatusColor = x.ServiceStatus!.ColorCode,
			StatusISOPen = x.ServiceStatus!.IsOpen,

			CreateDate = x.CreateDate,
			CreateOn = x.CreateDate.ToString(dateFormat),
			CreateOnTime = x.CreateDate.ToString(timeFormat),

			planId = x.PlanId,
			PlanName = x.Plan != null ? x.Plan.PlanName : "",
			EvaluationType = x.DepEvaluationType != null ? (lang == "ar" ? x.DepEvaluationType.NameAr : x.DepEvaluationType.NameAr) : "",
			OrgTreeId = x.OrgTreeId,
			OrgTreeName = x.OrgTree != null ? (lang == "ar" ? x.OrgTree.NameAr : x.OrgTree.NameEn) : ""
		});
	}

	public async Task<EvaluationRequest?> GetEvaluationRequestByIdAsync(Guid RequestId, bool UseMainUow = false)
	{
		UnitOfWork Scope;
		if (UseMainUow)
		{
			Scope = uow;
		}
		else
		{
			Scope = serviceScopeFactory.CreateScopedUow();

		}
		var Request = await Scope.GetRepository<EvaluationRequest>()
									.GetAllActiveNonDeleted(x => x.Id == RequestId)
									.Include(c => c.Plan)
									.Include(x => x.ServiceStatus)
									.ThenInclude(x => x!.StatusPreventPartyTypes)
									.Include(c => c.Service)
									.Include(c => c.OrgTree)
									.AsSplitQuery()
									.AsNoTracking()
									.FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false);

		return Request;

	}

	public async Task<EvaluationRequestDTO> GetEvaluationDetailsAsync(Guid id, CancellationToken ct = default)
	{
		var lang = requestInfo.Lang;
		var userId = userInfo.UserId ??  Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");// throw new BusinessException(ExceptionMessage.UserNotFound);

		var request = await GetEvaluationRequestByIdAsync(id);
		if (request == null)
			throw new BusinessException(ExceptionMessage.lblRequestNotValid);

		if (request.Service == null || request.ServiceStatus == null)
			throw new BusinessException(ExceptionMessage.lblRequestNotValid);

		var userTask = srvUser.GetByIDActiveNonDeleted(userId);
		var moduleTask = SrvSystemModule.GetSystemModuleByIdAsync(request.Service.SystemModuleId);
		var fieldsTask = GetEvaluationRequestFieldsValueAsync(request);

		await Task.WhenAll(userTask, moduleTask, fieldsTask);

		var user = await userTask;
		if (user == null)
			throw new BusinessException(ExceptionMessage.UserNotFound);

		var module = await moduleTask;
		var formGroups = await fieldsTask;

		var preventPartyTypes = request.ServiceStatus.StatusPreventPartyTypes;
		if (preventPartyTypes != null
			&& user.UserPartTypes != null
			&& preventPartyTypes.Any(x => user.UserPartTypes.Any(c => c.PartyTypeId == x.PartyTypeId)))
		{
			throw new BusinessException(ExceptionMessage.lblNoPermissionForRequestStatus);
		}

		if (user is MinistryUser)
		{
			if (user.Id != request.CreateById)
			{
				var allowed = await ValidateMinistryUserAccessAsync(userId, module?.Id, request.Id);
				//if (!allowed)
					//throw new UnauthorizedAccessException(ExceptionMessage.lblNoPermissionForViewRequest);
			}
		}

		// bool hasFieldHistoryPermission = false;
		// bool hasAllFieldHistoryPermission = false;

		var evaluationPartiesTask = srvEvaluationParty.GetPartiesWithServicesAndRequestsAsync(request.Id, module.DepartmentId);
		var attachmentsTask = GetAllEvaluationRequestAttachmentsAsync(request.Id, lang);
		var actionTransactionsTask = SrvActionTransactionsLog.GetActionLog(request.Id, request.ServiceId, module?.Id, user);
		var schoolTask = schoolRepository.GetSchoolDetails(request.OrgTreeId);
		var actionsTask = srvActionStatusConfiguration.GetActionsByStatus(request.ServiceId,request.ServiceStatusId,request.Id,request.PlanId,lang);
		var Status = request.ServiceStatus.NameAr;//SrvStatus.GetStatusDisplayName(request.ServiceStatusId, module?.Id);
		await Task.WhenAll(attachmentsTask, actionTransactionsTask, schoolTask, actionsTask, evaluationPartiesTask);

		var school = await schoolTask;
		var attachments = await attachmentsTask;
		var actionTransactions = await actionTransactionsTask;
		var actions = await actionsTask;
		var evaluationParties = await evaluationPartiesTask;
		var requestDetails= new EvaluationRequestDTO
		{
			formGroups = formGroups,
			Attachments =  attachments,
			ActionTransactions =  actionTransactions,
			School = mapper.Map<ResponseSchools>(school),
			Actions =  actions,

			planNo = request.Plan?.PlanName,
			planId = request.Plan?.Id,

			Status = Status,
			Service = lang == "ar" ? request.Service.NameAr : request.Service.NameEn,
			EvaluationParties=await evaluationPartiesTask,
			//CanViewFieldHistory = hasFieldHistoryPermission,
			//CanViewAllFieldHistory = hasAllFieldHistoryPermission
		};
		return requestDetails;
	}
	private async Task<bool> ValidateMinistryUserAccessAsync(Guid userId, Guid? moduleId, Guid? requestId)
	{
		if (moduleId is null)
		{
			return false;
		}

		var uow = serviceScopeFactory.CreateScopedUow();

		var userPartyDataTask = SrvPartyType.GetUserPartyTypeData(userInfo.UserId!, moduleId);



		var hasRequestAccessTask = uow.GetRepository<EvaluationRequestAssignment>()
								   .GetAllQueryFiltered()
								   .AnyAsync(a => a.EvaluationRequestId == requestId &&
												  a.MinistryUserId == userId &&
												  userInfo.PartyTypes.Contains(a.PartyTypeId));

		var userPartyData = await userPartyDataTask;

		if (userPartyData.Any(pt => pt.CanViewAllRequests))
		{
			return true;
		}


		else
		{
			var hasRequestAccess = await hasRequestAccessTask;

			if (hasRequestAccess)
			{
				return true;
			}
		}

		return false;
	}

	public async Task<List<FormGroupDTO>> GetEvaluationRequestFieldsValueAsync(EvaluationRequest request)
	{
		string lang = requestInfo.Lang;
		var stepFieldsListTask = SrvField.GetFieldsListByActionIdAsync(request.ServiceId);
		var hiddenFieldsIds = await SrvField.GetHiddenFields(request.ServiceId);

		var requestFieldsValue = await serviceScopeFactory.CreateScopedUow()
			.GetRepository<EvaluationRequestFieldsValue>()
			.GetAllActiveNonDeleted()
			.Include(x => x.Field)
			.ThenInclude(x => x!.FieldViewConditions)
			.Where(c => c.EvaluationRequestId == request.Id && !hiddenFieldsIds.Contains(c.FieldId))
			.Select(c => new
			{
				c.FieldId,
				c.Value,
				Type = c.Field!.DropDownTypeId != null ? "text" : c.Field!.FieldType!.NameEn, // Convert type to "text" if dropdown
				c.Field.FormGroupId,
				FormGroupName = lang == "ar" ? c.Field!.FormGroup!.TitleAr : c.Field!.FormGroup!.TitleEn,
				FormGroupOrderNo = c.Field.FormGroup.Order,
				c.Field.Row,
				c.Field.Column,
				FieldName = lang == "ar" ? c.Field.TitleAr : c.Field.TitleEn,
				FieldTooltip = lang == "ar" ? c.Field.InfoAr : c.Field.InfoEn,
				c.Field.BackendName,
				c.Field.ClassName,
				c.Field.DropDownTypeId,
				Attributes = c.Field!.FieldAttributeValues!
					.Where(x => x.IsDeleted == false && x.IsActive == true)
					.Select(m => new AttributeDTO
					{
						Name = m.AttributeKey,
						Value = m.AttributeValue,
						Message = lang == "ar" ? m.MessageAr : m.MessageEn,
					}).ToList(),
				Conditions = c.Field!.FieldViewConditions!
						 .Where(x => x.IsDeleted == false && x.IsActive == true)
						 .Select(cond => new FieldViewConditionDTO
						 {
							 operators = cond.operators,
							 FieldValue = cond.FieldValue,
							 IsSufficient = cond.IsSufficient,
							 ParentFieldId = cond.ParentFieldId
						 }).ToList(),
				c.Field.FormGroupListId
			})
			.ToListAsync();


		var stepFieldsList = await stepFieldsListTask;

		var fieldTasks = requestFieldsValue.Select(async c =>
		{
			string fieldValue = c.Value!;
			Guid valueGuid = Guid.TryParse(c.Value, out var parsedGuid) ? parsedGuid : Guid.Empty;
			bool isVisible = true;

			foreach (var cond in c.Conditions)
			{
				var parentFieldValue = requestFieldsValue.FirstOrDefault(f => f.FieldId == cond.ParentFieldId)?.Value;
				bool isValid = await SrvAction.ValidatedConditionAsync(parentFieldValue, cond.operators, cond.FieldValue);

				if (isValid)
					isVisible = true;

				if (!isValid)
				{
					isVisible = false;
					break;
				}
			}
			if (!isVisible)
				return null;

			if (c.DropDownTypeId != null && !string.IsNullOrWhiteSpace(c.Value))
			{
				fieldValue = await srvDropdown.ResolveDropDownTextAsync(
					lang,
					c.Value!,
					c.DropDownTypeId.Value,
					request.PlanId
				);
			}

			if (c!.Type == "list" && c.FormGroupListId != null && c.Value != null)
			{

				var listValues = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(c.Value);

				if (listValues != null)
				{
					var updatedList = new List<Dictionary<string, string>>();

					foreach (var item in listValues)
					{
						var updatedItem = new Dictionary<string, string>();

						foreach (var kvp in item)
						{
							if (kvp.Key != "Index" && kvp.Key != "IsOld")
							{
								string updatedValue = kvp.Value?.ToString() ?? string.Empty;

								var field = await SrvField.GetFieldsByIdsAsync(Guid.Parse(kvp.Key));

								if (field?.DropDownTypeId != null && !string.IsNullOrWhiteSpace(updatedValue))
								{
									updatedValue = await srvDropdown.ResolveDropDownTextAsync(
										lang,
										updatedValue,
										field.DropDownTypeId.Value,
										request.PlanId
									);
								}
								updatedItem[kvp.Key] = updatedValue;
							}
						}

						updatedList.Add(updatedItem);
					}

					fieldValue = JsonConvert.SerializeObject(updatedList);
				}

			}

			return new FieldValueDTO
			{
				FieldId = c.FieldId,
				Value = fieldValue,
				Type = c.Type,
				FormGroupId = c.FormGroupId,
				FormGroupName = c.FormGroupName,
				FormGroupOrderNo = c.FormGroupOrderNo,
				Row = c.Row,
				Column = c.Column,
				FieldName = c.FieldName,
				FieldTooltip = c.FieldTooltip,
				BackendName = c.BackendName,
				ClassName = c.ClassName,
				DropDownTypeId = c.DropDownTypeId,
				Attributes = c.Attributes!,
				JsonSchema = c.FormGroupListId is not null
					? await SrvField.GenerateJsonSchemaForFormGroupList(c.FormGroupListId, stepFieldsList, "Preview")
					: null
			};
		}).ToList();

		var fields = (await Task.WhenAll(fieldTasks)).Where(x => x != null).ToList();

		var formResult = fields
			.GroupBy(c => new { c!.FormGroupId, Order = c.FormGroupOrderNo, FormGroupName = c.FormGroupName })
			.Select(group => new FormGroupDTO
			{
				FormGroupName = group.Key!.FormGroupName!,
				Order = group.Key.Order,
				Fields = group.ToList()!
			})
			.OrderBy(c => c.Order)
			.ToList();

		return formResult;
	}

	public async Task<List<AttachementDTO?>> GetAllEvaluationRequestAttachmentsAsync(Guid id, string lang)
	{
		var uow = serviceScopeFactory.CreateScopedUow();

		var fields = await uow.GetRepository<EvaluationRequestFieldsValue>()
			.GetAllQueryFiltered()
			.Include(f => f.Field)
			.ThenInclude(f => f.FieldType)
			.Where(f => f.EvaluationRequestId == id)
			.ToListAsync();

		var directFileValues = fields
			.Where(f => f.Field!.FieldType!.BackendName == FieldTypeConstant.file ||
						f.Field.FieldType.BackendName == FieldTypeConstant.fileV2)
			.Select(f => f.Value)
			.Where(v => !string.IsNullOrWhiteSpace(v))
			.ToList();

		var listFileValues = new List<string>();
		var listFields = fields.Where(f => f.Field!.FieldType!.BackendName == FieldTypeConstant.list).ToList();

		foreach (var listField in listFields)
		{
			if (string.IsNullOrWhiteSpace(listField.Value))
				continue;

			try
			{
				var items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(listField.Value);
				if (items == null) continue;

				foreach (var item in items)
				{
					foreach (var kv in item)
					{
						if (Guid.TryParse(Convert.ToString(kv.Value), out var fileId))
						{
							listFileValues.Add(fileId.ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Invalid JSON in list field {listField.FieldId}: {ex.Message}");
			}
		}

		var allAttachmentIds = directFileValues
			.Concat(listFileValues)
			.Distinct()
			.ToList();

		var attachments = await uow.GetRepository<EvalAttachment>()
			.GetAllQueryFiltered()
			.Where(a => a.ServiceRequestId == id || allAttachmentIds.Contains(a.Id.ToString()))
			.Select(a => new AttachementDTO
			{
				Id = a.Id,
				UiFileName = a.UiFileName
			})
			.ToListAsync();

		return attachments;
	}

	private async Task<WebAppEvaluationRequestsDTO> FilteredEvaluationRequestsAsync(UnitOfWork uow, bool isMinistry, IQueryable<EvaluationRequestDTO> requests, FilterRequestsDTO model)
	{
		var result = new WebAppEvaluationRequestsDTO();

		if (model != null)
		{
			if (isMinistry)
			{
				if (!string.IsNullOrEmpty(model.Qid))
				{
					requests = requests.Where(x => !string.IsNullOrEmpty(x.QID) && x.QID == model.Qid);
				}

				if (model.StudentUserId.HasValue)
				{
					requests = requests.Where(x => x.StudentUserId == model.StudentUserId);
				}

				if (!string.IsNullOrEmpty(model.Mobile))
				{
					requests = requests.Where(x => !string.IsNullOrEmpty(x.Mobile) && x.Mobile == model.Mobile);
				}

				if (model.StudentNationalityId != null && model.StudentNationalityId.Any())
				{
					requests = requests.Where(x => model.StudentNationalityId.Contains(x.StudentNationalityId!));
				}

				if (model.CountryId != null && model.CountryId.Any())
				{
					requests = requests.Where(x => model.CountryId.Contains(x.CountryId));
				}

				if (model.UniversityId != null && model.UniversityId.Any())
				{
					requests = requests.Where(x => model.UniversityId.Contains(x.UniversityId));
				}

				var date_Format = await cacheDataProvider.GetSystemSettingValue(SystemSettings.DateFormat);

				if (!string.IsNullOrEmpty(model.RequestDateFrom))
				{
					if (DateTime.TryParseExact(model.RequestDateFrom, date_Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime requestDateFromDate))
					{
						requests = requests.Where(x => x.CreateDate != null && x.CreateDate.Value.Date >= requestDateFromDate.Date);
					}
				}

				if (!string.IsNullOrEmpty(model.RequestDateTo))
				{
					if (DateTime.TryParseExact(model.RequestDateTo, date_Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime requestDateToDate))
					{
						requests = requests.Where(x => x.CreateDate != null && x.CreateDate.Value.Date <= requestDateToDate.Date);
					}
				}

				if (model.StatusesList != null && model.StatusesList.Any())
				{
					model.StatusesList = model.StatusesList.Where(x => x != null).ToList();
					if (model.StatusesList.Count > 0)
					{
						requests = requests.Where(x => model.StatusesList.Contains(x.StatusId));
					}
				}
			}

			if (!string.IsNullOrEmpty(model.planNo))
			{
				requests = requests.Where(x => !string.IsNullOrEmpty(x.planNo) && x.planNo.ToLower().Contains(model.planNo.ToLower()));
			}

			if (!string.IsNullOrEmpty(model.RequestNo))
			{
				requests = requests.Where(x => !string.IsNullOrEmpty(x.RequestNumber) && x.RequestNumber.ToLower().Contains(model.RequestNo.ToLower()));
			}

			if (model.ServiceId != null && model.ServiceId.Any())
			{
				requests = requests.Where(x => model.ServiceId.Contains(x.ServiceId!.Value));
			}

			if (!string.IsNullOrEmpty(model.StatusTypeId))
			{
				requests = requests.Where(c => model.StatusTypeId == "0" ? c.StatusISOPen == false : c.StatusISOPen == true);
			}

			result.TotalDataCount = await requests.CountAsync();

			if (model.PageNumber != null)
			{
				var pageSize = 10;//Int32.Parse(await cacheDataProvider.GetSystemSettingValue(SystemSettings.ServiceRequestPageSize));
								  //if (isMinistry)
								  //    pageSize= (pageSize + 1) / 2;
				result.PageNumber = model.PageNumber.Value;
				result.PageSize = pageSize;
				var skip = (result.PageNumber - 1) * pageSize;

				if (model.OderByAction == true)
				{
					var requestList = await requests.ToListAsync();

					foreach (var item in requestList)
					{
						item.ActionCount = await srvActionStatusConfiguration.GetActionCountByStatusAsync(item.StatusId);
					}

					result.Data = requestList
						.OrderByDescending(x => x.StatusISOPen)
						.ThenByDescending(x => x.ActionCount)
						.ThenByDescending(x => x.CreateDate!.Value)
						.Skip(skip)
						.Take(pageSize)
						.ToList();

					result.IsRemainingData = result.Data.Count >= result.PageSize;
				}
				else
				{
					result.Data = await requests
						.OrderByDescending(x => x.CreateDate!.Value)
						.Skip(skip)
						.Take(pageSize)
						.ToListAsync();

					result.IsRemainingData = result.Data.Count >= result.PageSize;
				}
			}
			else
			{
				result.Data = await requests
					.OrderByDescending(x => x.StudentIsSpecial)
					.ThenByDescending(x => x.CreateDate!.Value)
					.ToListAsync();
			}
		}

		return result;
	}


}
