using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.SystemLog;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Globalization;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvServiceRequest(SystemModuleSrv SrvSystemModule,SrvAction SrvAction, SchoolRepository schoolRepository, SrvActionTransactionsLog SrvActionTransactionsLog, SrvField SrvField, SrvAttachments SrvAttachments, SrvPartyType SrvPartyType, SrvDropdown SrvDropdown, SrvActionStatusConfiguration SrvActionStatusConfiguration, SrvStatus SrvStatus, SrvUser srvUser, IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
             : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)

    {
		public async Task<ServiceRequest?> GetSrvServiceRequestByIdAsync(Guid RequestId, bool UseMainUow = false)
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
			var Request = await Scope.GetRepository<ServiceRequest>()
				.GetAllQueryFiltered(x => x.Id == RequestId)
				.Include(x => x.Status)
				.Include(x => x.Service)
				.FirstOrDefaultAsync();

			return Request;

		}
		public async Task<ServiceRequest?> GetRequestByIdAsync(Guid RequestId, bool UseMainUow = false)
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
			var Request = await Scope.GetRepository<ServiceRequest>()
										.GetAllActiveNonDeleted(x => x.Id == RequestId)
										.Include(c => c.Plan)
										.Include(c => c.EvaluationRequest)
										.Include(x => x.Status)
										.ThenInclude(x => x!.StatusPreventPartyTypes)
										.Include(c => c.Service)
										.Include(c => c.OrgTree)
										.AsSplitQuery()
										.AsNoTracking()
										.FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false);

			return Request;

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
		public async Task<Guid?> GetOrgTreeIdByRequestIdAsync(Guid reqId)
		{
			var request = await serviceScopeFactory.CreateScopedUow()
									   .GetRepository<ServiceRequest>()
										.GetAllActiveNonDeleted(x => x.Id == reqId).FirstOrDefaultAsync();
			return request?.OrgTreeId;
		}
		public Guid? GetOrgTreeRequestId(ServiceRequest? request)
		{
			if (request != null)
			{
				return request.OrgTreeId;
			}
			return null;
		}
		public ServiceRequest InsertRequest(ServiceRequest request)
		{
			return uow.GetRepository<ServiceRequest>().Insert(request, false);
		}
		public async Task<WebAppPlanRequestsDTO> GetPlanRequestsAsync(Guid userId, FilterRequestsDTO model)
		{
			string lang = _requestInfo!.Lang;
			model.ModuleName = "/evaluation-plan";

			using var uow = serviceScopeFactory.CreateScopedUow();
			using var uow2 = serviceScopeFactory.CreateScopedUow();

			var moduleTask = SrvSystemModule.GetSystemModuleByRoutingAsync(model.ModuleName);
			var userTask = srvUser.GetByIDActiveNonDeleted(userId);
			var timeFormatTask = cacheDataProvider.GetSystemSettingValue(SystemSettings.ShortTimeFormat);
			var dateFormatTask = cacheDataProvider.GetSystemSettingValue(SystemSettings.DateFormat);

			await Task.WhenAll(moduleTask, userTask, timeFormatTask, dateFormatTask);

			var module = moduleTask.Result;
			var user = userTask.Result;
			var time_Format = timeFormatTask.Result;
			var date_Format = dateFormatTask.Result;
			WebAppPlanRequestsDTO filteredResult;
			if (user == null || module == null)
				throw new BusinessException(ExceptionMessage.UserInfoNotFound);

			var isMinistry = user is MinistryUser;

			if (!isMinistry)
			{
				//var requestsQuery = GetRequestsForStudentUser(uow, userId, module, lang, time_Format, date_Format);
				filteredResult = null;// await FilteredRequestsAsync(uow, isMinistry, requestsQuery, model);
			}
			else
			{
				var RequestsTask = await GetRequestsForMinistryUserAsync(uow, userId, module, lang, time_Format, date_Format);
				filteredResult = await FilteredPlanRequestsAsync(uow, isMinistry, RequestsTask, model);
			}

			//await UpdateRequestStatusesAsync(filteredResult.Data, module?.Id);


			return filteredResult;


		}
		//public async Task<List<ServiceRequestSummaryDTO>> SearchServiceRequestSummary(Guid planId, string requestNumber)
		//{
		//	var requestsList = await serviceScopeFactory.CreateScopedUow()
		//		.GetRepository<ServiceRequest>()
		//		.GetAllActiveNonDeleted(x => x.RequestNumber.Contains(requestNumber) && x.planId == planId)
		//		.Include(c => c.Plan)
		//		.Include(c => c.InitialHistory)
		//		.Include(x => x.Status)
		//		.ThenInclude(x => x!.StatusPreventPartyTypes)
		//		.Include(c => c.Service)
		//		.Include(c => c.Student)
		//		.ThenInclude(c => c!.UserGender)
		//		.AsNoTracking()
		//		.Select(x => new ServiceRequestSummaryDTO
		//		{
		//			RequestId = x.Id,
		//			RequestNumber = x.RequestNumber,
		//			ServiceName = x.Service != null ? (_requestInfo.Lang == "ar" ? x.Service.NameAr : x.Service.NameEn) : "",
		//			QID = x.Student != null ? x.Student.QID : "",
		//			ServiceRequestStatus = x.Status != null ? (_requestInfo.Lang == "ar" ? x.Status.NameAr : x.Status.NameEn) : ""
		//		})
		//		.ToListAsync();

		//	return requestsList;
		//}
		//public async Task<ServiceRequestSummaryDTO?> GetRequestSummaryById(Guid requestId)
		//{
		//	var requestObj = await serviceScopeFactory.CreateScopedUow()
		//		.GetRepository<ServiceRequest>()
		//		.GetAllActiveNonDeleted(x => x.Id == requestId)
		//		.Include(c => c.plan)
		//		.Include(c => c.InitialHistory)
		//		.Include(x => x.Status)
		//		.ThenInclude(x => x!.StatusPreventPartyTypes)
		//		.Include(c => c.Service)
		//		.Include(c => c.Student)
		//		.ThenInclude(c => c!.UserGender)
		//		.AsNoTracking()
		//		.Select(x => new ServiceRequestSummaryDTO
		//		{
		//			RequestId = x.Id,
		//			RequestNumber = x.RequestNumber,
		//			ServiceName = x.Service != null ? (_requestInfo.Lang == "ar" ? x.Service.NameAr : x.Service.NameEn) : "",
		//			QID = x.Student != null ? x.Student.QID : "",
		//			ServiceRequestStatus = x.Status != null ? (_requestInfo.Lang == "ar" ? x.Status.NameAr : x.Status.NameEn) : ""
		//		})
		//		.FirstOrDefaultAsync();

		//	return requestObj;
		//}

		public async Task<WebAppEvaluationRequestsDTO> GetEvaluationRequestsAsync(Guid userId, FilterRequestsDTO model)
		{
			string lang = _requestInfo!.Lang;
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
		private async Task<IQueryable<EvaluationRequestDTO>> GetDepEvaluationRequestsAsync(UnitOfWork uow,Guid userId,SystemModule module,string lang,string timeFormat,string dateFormat)
		{
			var permissionTasks = new
			{
				IsAllowedToViewAllRequests =
					IsAllowedToViewAllRequestsAsync(userId, module.Id),

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
				.Where(x =>x.Service!.SystemModuleId == module.Id);

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
				RequestNumber="1234",
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
		//private IQueryable<EvaluationRequest> ApplyUserAccessFiltersForEvaluationRequests(IQueryable<EvaluationRequest> query,UserPartyTypeDataDTO userPartyTypeData,Guid userId,bool isAllowedToViewAllRequests)
		//{
		//	if (isAllowedToViewAllRequests)
		//		return query;

		//	if (userPartyTypeData.AllowedOrgTreeIds?.Any() == true)
		//	{
		//		query = query.Where(e => userPartyTypeData.AllowedOrgTreeIds.Contains(e.OrgTreeId));
		//	}

		//	if (userPartyTypeData.AllowedPlanIds?.Any() == true)
		//	{
		//		query = query.Where(e => userPartyTypeData.AllowedPlanIds.Contains(e.PlanId));
		//	}

		//	return query;
		//}

		public async Task<ServiceRequestDTO> GetRequestDetailsAsync(Guid id)
		{
			string lang = _requestInfo.Lang;
			var userId = userInfo.UserId;

			bool hasFieldHistoryPermission = false;
			bool hasAllFieldHistoryPermission = false;

			var request = await GetRequestByIdAsync(id);
			if (request == null) throw new BusinessException(ExceptionMessage.lblRequestNotValid);

			var userTask = srvUser.GetByIDActiveNonDeleted(userId!.Value);
			var RequestFieldsValueTask = GetRequestFieldsValueAsync(request);

			var ModuleTask = SrvSystemModule.GetSystemModuleByIdAsync(request.Service!.SystemModuleId);

			var user = await userTask;

			if (user == null)
			{
				throw new BusinessException(ExceptionMessage.UserNotFound);
			}

			if (request.Status!.StatusPreventPartyTypes.Any(x => user.UserPartTypes!.Any(c => c.PartyTypeId == x.PartyTypeId)))
			{
				throw new BusinessException(ExceptionMessage.lblNoPermissionForRequestStatus);
			}
			var Module = await ModuleTask;

			var isMinistry = user is MinistryUser;


			if (isMinistry)
			{
				
				if (user.Id != request.CreateById && !await ValidateMinistryUserAccessAsync(userId!.Value, Module?.Id,   request.Id))
				{
					throw new UnauthorizedAccessException(ExceptionMessage.lblNoPermissionForViewRequest);
				}

			}
			

			var attachmentsTask = GetAllRequestAttachmentsAsync(request.Id, lang);
			var actionTransactionsTask = SrvActionTransactionsLog.GetActionLog(request.Id, request.ServiceId, Module?.Id, user);
			var applicantTask = srvUser.GetApplicantStudent(request);
			var actionsTask = SrvActionStatusConfiguration.GetActionsByStatus(request.ServiceId, request.StatusId, request.Id, request.PlanId, lang);

			await Task.WhenAll(attachmentsTask, actionTransactionsTask, applicantTask, actionsTask, RequestFieldsValueTask);
			var RequestFieldsValue = await RequestFieldsValueTask;
			var dto = new ServiceRequestDTO
			{
				formGroups = RequestFieldsValue,
				Attachments = attachmentsTask.Result,
				ActionTransactions = actionTransactionsTask.Result,
				Applicant = applicantTask.Result,
				Actions = actionsTask.Result,
				//planNo = request.EvaluationRequest?.,
				//planId = request.Plan?.Id,
				RequestNumber = request.RequestNumber,
				Status = SrvStatus.GetStatusDisplayName(request.StatusId, Module?.Id),
				Service = lang == "ar" ? request.Service.NameAr : request.Service.NameEn,
				CanViewFieldHistory = hasFieldHistoryPermission,
				CanViewAllFieldHistory = hasAllFieldHistoryPermission
			};

			return dto;

		}

		public async Task<EvaluationRequestDTO> GetEvaluationDetailsAsync(Guid id)
		{
			string lang = _requestInfo.Lang;
			var userId = userInfo.UserId;

			bool hasFieldHistoryPermission = false;
			bool hasAllFieldHistoryPermission = false;

			var request = await GetEvaluationRequestByIdAsync(id);
			if (request == null) throw new BusinessException(ExceptionMessage.lblRequestNotValid);

			var userTask = srvUser.GetByIDActiveNonDeleted(userId!.Value);
			//var RequestFieldsValueTask = GetRequestFieldsValueAsync(request);

			var ModuleTask = SrvSystemModule.GetSystemModuleByIdAsync(request.Service!.SystemModuleId);

			var user = await userTask;

			if (user == null)
			{
				throw new BusinessException(ExceptionMessage.UserNotFound);
			}

			//if (request.Status!.StatusPreventPartyTypes.Any(x => user.UserPartTypes!.Any(c => c.PartyTypeId == x.PartyTypeId)))
			//{
			//	throw new BusinessException(ExceptionMessage.lblNoPermissionForRequestStatus);
			//}
			var Module = await ModuleTask;

			var isMinistry = user is MinistryUser;


			if (isMinistry)
			{

				if (user.Id != request.CreateById && !await ValidateMinistryUserAccessAsync(userId!.Value, Module?.Id, request.Id))
				{
					throw new UnauthorizedAccessException(ExceptionMessage.lblNoPermissionForViewRequest);
				}

			}


			//var attachmentsTask = GetAllRequestAttachmentsAsync(request.Id, lang);
			//var actionTransactionsTask = SrvActionTransactionsLog.GetActionLog(request.Id, request.ServiceId, Module?.Id, user);
			var applicantTask = schoolRepository.GetSchoolDetails(request.OrgTreeId);
			//var actionsTask = SrvActionStatusConfiguration.GetActionsByStatus(request.ServiceId, request.StatusId, request.Id, request.PlanId, lang);

			//await Task.WhenAll( actionTransactionsTask, applicantTask, actionsTask);//RequestFieldsValueTask,attachmentsTask
																					//var RequestFieldsValue = await RequestFieldsValueTask;

			var schoolsResponse = mapper.Map<ResponseSchools>(applicantTask.Result);

			var dto = new EvaluationRequestDTO
			{
				//formGroups = RequestFieldsValue,
				//Attachments = attachmentsTask.Result,
				//ActionTransactions = actionTransactionsTask.Result,
				School = schoolsResponse,
				//Actions = actionsTask.Result,
				//planNo = request.EvaluationRequest?.,
				//planId = request.Plan?.Id,
				//RequestNumber = request.RequestNumber,
				//Status = SrvStatus.GetStatusDisplayName(request.StatusId, Module?.Id),
				Service = lang == "ar" ? request.Service.NameAr : request.Service.NameEn,
				//CanViewFieldHistory = hasFieldHistoryPermission,
				//CanViewAllFieldHistory = hasAllFieldHistoryPermission
			};

			return dto;

		}
		public async Task<bool> HasAccessToRequestAsync(Guid requestId, Guid userId)
		{
			var requestTask = GetRequestByIdAsync(requestId);

			var userTask = srvUser.GetByIDActiveNonDeleted(userId);

			var request = await requestTask;
			if (request == null)
				throw new BusinessException(ExceptionMessage.lblRequestNotValid);

			var user = await userTask;
			if (user == null)
				throw new BusinessException(ExceptionMessage.UserNotFound);

			var module = await SrvSystemModule.GetSystemModuleByIdAsync(request.Service!.SystemModuleId);

			bool isMinistry = user is MinistryUser;

			if (request.Status!.StatusPreventPartyTypes.Any(x => user.UserPartTypes!.Any(c => c.PartyTypeId == x.PartyTypeId)))
			{
				throw new BusinessException(ExceptionMessage.lblNoPermissionForRequestStatus);
			}
			if (isMinistry)
			{

				if (user.Id == request.CreateById)
					return true;

				if (await ValidateMinistryUserAccessAsync(userId, module?.Id,  request.Id))
					return true;

				return false;
			}
			else
			{
			//	if (user.Id == request.StudentId)
			//		return true;

				return false;
			}
		}
		private async Task UpdateRequestStatusesAsync(List<ServiceRequestDTO> requests, Guid? moduleId)
		{
			string lang = _requestInfo.Lang;

			foreach (var item in requests)
			{
				item.Status = SrvStatus.GetStatusDisplayName(item.StatusId, moduleId);
			}
		}
		private async Task UpdateRequestStatusesAsync(List<EvaluationRequestDTO> requests, Guid? moduleId)
		{
			string lang = _requestInfo.Lang;

			foreach (var item in requests)
			{
				item.Status = SrvStatus.GetStatusDisplayName(item.StatusId, moduleId);
			}
		}

		//private IQueryable<ServiceRequestDTO> GetRequestsForStudentUser(UnitOfWork uow, Guid userId, SystemModule module, string lang, string timeFormat, string dateFormat)
		//{
		//	var query =
		//					 uow.GetRepository<ServiceRequest>()
		//					  .GetAllActiveNonDeleted()
		//					  .Include(c => c.Service)
		//					  .Include(c => c.Service!.RequestShowPartyType)
		//					 .Include(c => c.Status!.StatusPreventPartyTypes)
		//					 .Include(c => c.plan)
		//					 .ThenInclude(c => c!.SchStatus)
		//					 .Include(c => c.InitialHistory)
		//					 .Include(c => c.Status)
		//					 .Include(c => c.Country)
		//					 .Include(c => c.University)
		//					 .AsSplitQuery()
		//					 .Where(c => c.Service!.SystemModuleId == module.Id)
		//					 .Where(c => c.CreateById == userId || c.StudentId == userId
		//					   || c.plan!.StudentUserId == userId
		//					   && c.Service!.RequestShowPartyType!.Any(x => userInfo.PartyTypes.Contains(x.PartyTypeId)))
		//					 .Where(c =>
		//					 !c.Status!.StatusPreventPartyTypes.Any(sp => userInfo.PartyTypes.Contains(sp.PartyTypeId) && sp.IsActive == true && sp.IsDeleted == false))
		//					 .Select(c => new ServiceRequestDTO
		//					 {
		//						 RequestNumber = c.RequestNumber,
		//						 Status = lang == "ar" ? c.Status!.NameAr : c.Status!.NameEn,
		//						 StatusColor = lang == "ar" ? c.Status.ColorCode : c.Status.ColorCode,
		//						 StatusISOPen = c.Status.IsOpen,
		//						 ServiceId = c.Service!.Id,
		//						 Service = lang == "ar" ? c.Service.NameAr : c.Service.NameEn,
		//						 CreateOn = c.CreateDate.ToString(dateFormat),
		//						 CreateOnTime = c.CreateDate.ToString(timeFormat),
		//						 CreateDate = c.CreateDate,
		//						 Id = c.Id,
		//						 StatusId = c.StatusId,
		//						 icon = c.Service.Icon,
		//						 OwnerName = lang == "ar" ? c.Student!.FullNameAr : c.Student!.FullNameEn,
		//						 StudentNationalityId = c.Student.NationalityCode,
		//						 StudentIsSpecial = c.Student.IsSpecial,
		//						 planNo = c.plan!.planNumber,
		//						 planId = c.plan.Id,
		//						 SchStatus = lang == "ar" ? c.plan.SchStatus!.NameAr : c.plan.SchStatus!.NameEn,
		//						 CountryId = c.CountryId,
		//						 UniversityId = c.UniversityId,
		//						 CountryName = lang == "ar"
		//										? (c.Country != null ? c.Country.NameAr : null)
		//										: (c.Country != null ? c.Country.NameEn : null),
		//						 CountryCode = c.Country!.ISOCode,
		//						 UniversityCode = c.University!.Code,
		//						 UniversityName = lang == "ar" ? c.University.NameAr : c.University.NameEn,
		//					 });
		//	string x = query.ToQueryString();
		//	return query;
		//}
		private async Task<IQueryable<ServiceRequestDTO>> GetRequestsForMinistryUserAsync(UnitOfWork uow, Guid userId, SystemModule module, string lang, string timeFormat, string dateFormat)
		{
			var permissionTasks = new
			{
				IsAllowedToViewAllRequests = IsAllowedToViewAllRequestsAsync(userId, module?.Id),
				IsAllowedToViewAllRequestsWithoutFiltration = SrvPartyType.IsAllowedToViewAllRequestsWitoutFilterationAsync(userId, module?.Id),
				UserPartyTypeData = SrvPartyType.GetUserPartyTypeData(userInfo.UserId!, module?.Id)
			};

			IQueryable<ServiceRequest> baseQuery = uow
				.GetRepository<ServiceRequest>()
				.GetAllActiveNonDeleted()
				.Include(c => c.Service)
				.Include(c => c.Service!.RequestShowPartyType)
				.Include(c => c.Status)
				.Include(c => c.Status!.StatusPreventPartyTypes)
				.Include(c => c.OrgTree)
				.Include(c => c.Plan)
					.ThenInclude(s => s!.PlanStatus);


			// Apply service type and basic filters
			baseQuery = baseQuery.AsSplitQuery().Where(c =>
				!c.Status!.StatusPreventPartyTypes.Any(sp => userInfo.PartyTypes.Contains(sp.PartyTypeId) && sp.IsActive == true && sp.IsDeleted == false) &&
				c.Service!.SystemModuleId == module!.Id &&
				c.Status.IsInitial != true);

			var permissions = new
			{
				IsAllowedToViewAllRequests = await permissionTasks.IsAllowedToViewAllRequests,
				IsAllowedToViewAllRequestsWithoutFiltration = await permissionTasks.IsAllowedToViewAllRequestsWithoutFiltration,
				UserPartyTypeData = await permissionTasks.UserPartyTypeData
			};

			if (!permissions.IsAllowedToViewAllRequestsWithoutFiltration)
			{
				baseQuery = ApplyUserAccessFilters(baseQuery, permissions.UserPartyTypeData, userId, permissions.IsAllowedToViewAllRequests);
			}


			return baseQuery.Select(c => new ServiceRequestDTO
			{
				RequestNumber = c.RequestNumber,
				Status = lang == "ar" ? c.Status!.NameAr : c.Status!.NameEn,
				StatusColor = c.Status!.ColorCode,
				StatusId = c.StatusId,
				StatusISOPen = c.Status.IsOpen,
				ServiceId = c.Service!.Id,
				Service = lang == "ar" ? c.Service.NameAr : c.Service.NameEn,
				CreateOn = c.CreateDate.ToString(dateFormat),
				CreateOnTime = c.CreateDate.ToString(timeFormat),
				CreateDate = c.CreateDate,
				Id = c.Id,
				icon = c.Service.Icon,
				//QID = c.Student != null ? c.Student.QID : "",
				//StudentUserId = c.Student!.Id,
				//StudentNationalityId = c.Student != null ? c.Student.NationalityCode : null,
				//Mobile = c.Student != null ? c.Student.Mobile : "",
				//OwnerName = c.Student != null ? (lang == "ar" ? c.Student.FullNameAr : c.Student.FullNameEn) : "",
				//StudentIsSpecial = c.Student != null && c.Student.IsSpecial,
				//planNo = c.plan != null ? c.plan.planNumber : "",
				//planId = c.plan != null ? c.plan.Id : null,
				//SchStatus = c.plan != null && c.plan.SchStatus != null
				//	? (lang == "ar" ? c.plan.SchStatus.NameAr : c.plan.SchStatus.NameEn)
				//	: "",
				
			});
		}
		private IQueryable<ServiceRequest> ApplyUserAccessFilters(IQueryable<ServiceRequest> baseQuery, IEnumerable<UserPartyTypeDTO> userPartyTypeData, Guid userId, bool isAllowedToViewAllRequests)
		{
			
			// User-specific filtering (created by or assigned to)
			if (!isAllowedToViewAllRequests)
			{
				return ApplyUserSpecificFilter(baseQuery, userId);
			}

			return baseQuery;
		}
	
		private IQueryable<ServiceRequest> ApplyUserSpecificFilter(IQueryable<ServiceRequest> baseQuery, Guid userId)
		{
			return baseQuery
				.Include(c => c.Assignments)
				.Where(c =>
					c.CreateById == userId ||
					(c.Assignments != null && c.Assignments.Any(a =>
						a.MinistryUserId == userId && userInfo.PartyTypes.Contains(a.PartyTypeId))));
		
		}

		private async Task<bool> ValidateMinistryUserAccessAsync(Guid userId, Guid? moduleId,  Guid? requestId)
		{
			if (moduleId is null)
			{
				return false;
			}

			var uow = serviceScopeFactory.CreateScopedUow();

			var userPartyDataTask = SrvPartyType.GetUserPartyTypeData(userInfo.UserId!, moduleId);



			var hasRequestAccessTask = uow.GetRepository<RequestAssignment>()
									   .GetAllQueryFiltered()
									   .AnyAsync(a => a.ServiceRequestId == requestId &&
													  a.MinistryUserId == userId &&
													  userInfo.PartyTypes.Contains(a.PartyTypeId));

			var userPartyData = await userPartyDataTask;

			if (userPartyData.Any(pt => pt.CanViewAllRequests ))
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
		public async Task<List<AttachementDTO?>?> GetRequestAttachmentsAsync(Guid id, string lang)
		{
			var fields = await serviceScopeFactory.CreateScopedUow()
									   .GetRepository<ServiceRequestFieldsValue>()
									   .GetAllQueryFiltered()
									   .Include(c => c.Field)
									   .ThenInclude(c => c!.FieldType)
									   .Where(c => c.ServiceRequestId == id &&
												   (c.Field!.FieldType!.BackendName == FieldTypeConstant.file
													|| c.Field.FieldType.BackendName == FieldTypeConstant.fileV2))
									   .Select(c => c.Value).ToListAsync();

			var attachments = await serviceScopeFactory.CreateScopedUow()
									   .GetRepository<EvalAttachment>()
									   .GetAllQueryFiltered()
									   .Where(c => c.ServiceRequestId == id || fields.Contains(c.Id.ToString()))
									   .Select(m => new AttachementDTO()
									   {
										   Id = m.Id,
										   UiFileName = m.UiFileName
									   }).ToListAsync();
			return attachments!;
		}

		public async Task<List<AttachementDTO?>> GetAllRequestAttachmentsAsync(Guid id, string lang)
		{
			var uow = serviceScopeFactory.CreateScopedUow();

			var fields = await uow.GetRepository<ServiceRequestFieldsValue>()
				.GetAllQueryFiltered()
				.Include(f => f.Field)
				.ThenInclude(f => f.FieldType)
				.Where(f => f.ServiceRequestId == id)
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

		public async Task<List<FormGroupDTO>> GetRequestFieldsValueAsync(ServiceRequest request)
		{
			string lang = _requestInfo.Lang;
			var stepFieldsListTask = SrvField.GetFieldsListByActionIdAsync(request.ServiceId);
			var hiddenFieldsIds = await SrvField.GetHiddenFields(request.ServiceId);

			var requestFieldsValue = await serviceScopeFactory.CreateScopedUow()
				.GetRepository<ServiceRequestFieldsValue>()
				.GetAllActiveNonDeleted()
				.Include(x => x.Field)
				.ThenInclude(x => x!.FieldViewConditions)
				.Where(c => c.ServiceRequestId == request.Id && !hiddenFieldsIds.Contains(c.FieldId))
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
					fieldValue = await ResolveDropDownTextAsync(
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
										updatedValue = await ResolveDropDownTextAsync(
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
		public async Task<List<ServiceRequestFieldsValue>> GetRequestFieldsValueAsync(Guid? requestId)
		{
			var RequestFieldsValue = await serviceScopeFactory.CreateScopedUow()
										.GetRepository<ServiceRequestFieldsValue>()
										.GetAllActiveNonDeleted()
										.Include(x => x.Field)
										.Include(x => x.Field!.MappingField)
										.Include(x => x.Field!.FieldType)
										.Where(c => c.ServiceRequestId == requestId).ToListAsync();


			return RequestFieldsValue;
		}
	
		private async Task<bool> IsAllowedToViewAllRequestsAsync(Guid userId, Guid? ModuleId)

		{
			if (ModuleId is null)
			{
				return false;
			}

			var result = await serviceScopeFactory.CreateScopedUow()
									   .GetRepository<UserPartyType>()
										.GetAllQueryFiltered()
										.Include(x => x.PartyType)
										.Where(x => x.UserId == userId)
										.Where(x => x.PartyType!.SystemModuleId == ModuleId)
										.AnyAsync(x => x.PartyType!.CanViewAllRequests);

			return result;
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
							item.ActionCount = await GetActionCountByStatusAsync(item.StatusId);
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

		private async Task<WebAppPlanRequestsDTO> FilteredPlanRequestsAsync(UnitOfWork uow, bool isMinistry, IQueryable<ServiceRequestDTO> requests, FilterRequestsDTO model)
		{
			var result = new WebAppPlanRequestsDTO();

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
							item.ActionCount = await GetActionCountByStatusAsync(item.StatusId);
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
						.OrderByDescending(x => x.CreateDate!.Value)
						.ToListAsync();
				}
			}

			return result;
		}
		private async Task<int> GetActionCountByStatusAsync(Guid? statusId)
		{
			var ActionStatusConfiguration = (await cacheDataProvider.GetActionStatusConfiguration()).Where(c => c.CurrentStatusId == statusId).OrderBy(c => c.OrderNo).Select(c => c.ServiceActionId).ToList();
			var actionPartType = (await cacheDataProvider.GetActionPartyTypes()).Where(c => userInfo.PartyTypes.Contains(c.PartyTypeId)).Select(c => c.ServiceActionId).ToList();
			var actionsId = ActionStatusConfiguration.Intersect(actionPartType).ToList();
			return actionsId.Count();
		}
		public async Task<IList<FieldTransactionDTO>> GetFieldHistoryAsync(Guid fieldId, Guid requestId, string lang = "ar")
		{
			var user = await srvUser.GetByIDActiveNonDeleted(userInfo.UserId!.Value);

			var isMinistry = user is MinistryUser;

			var hasFieldHistoryPermission = false;

			if (isMinistry)
			{
				hasFieldHistoryPermission = await srvUser.HasPermission(userInfo.UserId.Value, AdminPermission.CanViewFieldHistory);
			}

			if (!hasFieldHistoryPermission)
				throw new BusinessException(ExceptionMessage.lblOnlyAuthorizedCanSeeHistory);

			var fieldValue = await serviceScopeFactory.CreateScopedUow()
				.GetRepository<ServiceRequestFieldsValue>()
				.GetAllQueryFiltered()
				.Include(c => c.Field!.FieldType)
				.Where(c => c.FieldId == fieldId && c.ServiceRequestId == requestId)
				.FirstOrDefaultAsync();

			if (fieldValue == null) return new List<FieldTransactionDTO>();

			var dateTime_Format = await cacheDataProvider.GetSystemSettingValue(SystemSettings.DateFormat);

			var transactions = await serviceScopeFactory.CreateScopedUow()
				.GetRepository<FieldValueTransactionsLog>()
				.GetAllQueryFiltered(c => c.ServiceRequestFieldsValueId == fieldValue.Id)
				.Include(c => c.ServiceRequestFieldsValue!.Field)
				.Include(c => c.ServiceRequestFieldsValue!.Field!.FieldType)
				.Include(c => c.TransactionsType)
				.Include(c => c.CreateBy)
				.AsSplitQuery()
				.OrderBy(c => c.CreateDate)
				.Select(c => new FieldTransactionDTO()
				{
					FieldName = lang == "ar" ? c.ServiceRequestFieldsValue!.Field!.TitleAr : c.ServiceRequestFieldsValue!.Field!.TitleEn,
					OldValue = c.OldValue,
					NewValue = c.NewValue,
					TransactionDate = c.CreateDate.ToString(dateTime_Format),
					TransactionType = lang == "ar" ? c.TransactionsType!.NameAr : c.TransactionsType!.NameEn,
					ActionName = lang == "ar" ? c.ServiceAction!.NameAr! : c.ServiceAction!.NameEn!,
					Type = c.ServiceRequestFieldsValue.Field.FieldType!.NameEn,
					dropDownTypeId = c.ServiceRequestFieldsValue.Field.DropDownTypeId,
					User = lang == "ar" ? c.CreateBy!.NameAr : c.CreateBy!.NameEn
				})
				.ToListAsync();

			foreach (var item in transactions)
			{
				// Handle dropdown or select2 types
				if (item.Type == "dropdown" || item.Type == "select2")
				{
					if (Guid.TryParse(item.OldValue, out Guid oldGuid) && Guid.TryParse(item.NewValue, out Guid newGuid))
					{
						var oldValue = await SrvDropdown.GetDropDownValue(lang, oldGuid, item.dropDownTypeId ?? Guid.Empty, null, null);
						var newValue = await SrvDropdown.GetDropDownValue(lang, newGuid, item.dropDownTypeId ?? Guid.Empty, null, null);

						item.OldValue = oldValue != null ? (lang == "ar" ? oldValue?.TitleAr : oldValue?.TitleEn) : null;
						item.NewValue = newValue != null ? (lang == "ar" ? newValue?.TitleAr : newValue?.TitleEn) : null;
					}
					else
					{
						item.OldValue = null;
						item.NewValue = null;
					}
				}

				// Handle file types
				if (item.Type == "file" || item.Type == "fileV2")
				{
					if (Guid.TryParse(item.OldValue, out Guid oldFileId) && Guid.TryParse(item.NewValue, out Guid newFileId))
					{
						var oldValue = await SrvAttachments.GetRequestAttachmentsByfieldId(oldFileId, requestId);
						var newValue = await SrvAttachments.GetRequestAttachmentsByfieldId(newFileId, requestId);

						item.OldAttachmentId = oldValue?.Id.ToString();
						item.OldValue = oldValue?.UiFileName;
						item.NewAttachmentId = newValue?.Id.ToString();
						item.NewValue = newValue?.UiFileName;
					}
					else
					{
						item.OldValue = null;
						item.NewValue = null;
						item.OldAttachmentId = null;
						item.NewAttachmentId = null;
					}
				}
			}
			return transactions;
		}

		private static List<Guid> TryParseGuidList(string? raw)
		{
			var result = new List<Guid>();
			if (string.IsNullOrWhiteSpace(raw)) return result;
			try
			{
				var asArray = JsonConvert.DeserializeObject<List<string>>(raw!);
				if (asArray != null && asArray.Count > 0)
				{
					foreach (var s in asArray)
						if (Guid.TryParse(s, out var g)) result.Add(g);
					if (result.Count > 0) return result;
				}
			}
			catch { }

			var parts = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			foreach (var p in parts)
				if (Guid.TryParse(p, out var g)) result.Add(g);

			return result;
		}

		private async Task<string> ResolveDropDownTextAsync(string lang, string rawValue, Guid dropDownTypeId,Guid? PlanId)
		{
			if (Guid.TryParse(rawValue, out var singleId) && singleId != Guid.Empty)
			{
				var v = await SrvDropdown.GetDropDownValue(lang, singleId, dropDownTypeId,null, PlanId);
				if (v != null) return lang == "ar" ? v.TitleAr ?? "" : v.TitleEn ?? "";
				return rawValue;
			}
			var ids = TryParseGuidList(rawValue);
			if (ids.Count == 0) return rawValue ?? string.Empty;

			var titles = new List<string>(ids.Count);
			foreach (var id in ids)
			{
				var v = await SrvDropdown.GetDropDownValue(lang, id, dropDownTypeId,null, PlanId);
				titles.Add(v != null ? (lang == "ar" ? v.TitleAr ?? "" : v.TitleEn ?? "") : id.ToString());
			}
			var sep = lang == "ar" ? "، " : ", ";
			return string.Join(sep, titles.Where(t => !string.IsNullOrWhiteSpace(t)));
		}


	}
}
