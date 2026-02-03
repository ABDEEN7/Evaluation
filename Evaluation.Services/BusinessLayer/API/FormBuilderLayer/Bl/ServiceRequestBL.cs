using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.SystemLog;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using static Evaluation.DAL.ConstantKeys;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Models.API
{

    public class ServiceRequestBL(
        IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvNotification SrvNotification, SrvUser SrvUser, 
        LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, SystemModuleSrv systemModuleSrv, SrvAction SrvAction, 
        SrvStatus SrvStatus, SrvAssignment SrvAssignment,SrvEvaluationRequestAssignment _srvEvaluationRequestAssignment, SrvActionTransactionsLog SrvActionTransactionsLog,  PerformActionBL _performActionBL,

		SrvService SrvService, SrvServiceRequest _srvServiceRequest, EvaluationRequestService _evaluationRequestService, SrvAttachments _srvAttachments, IServiceProvider serviceProvider,RequestInfo _requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
    {

		public async Task<WebAppPlanRequestsDTO> GetPlanRequestsAsync(FilterRequestsDTO filter)
		{
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23") ;
			return await _srvServiceRequest.GetPlanRequestsAsync(userId, filter);
		}
		public async Task<WebAppEvaluationRequestsDTO> GetEvaluationRequestsAsync(FilterRequestsDTO filter)
		{
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");
			return await _evaluationRequestService.GetEvaluationRequestsAsync(userId, filter);
		}

		public async Task<ServiceRequestDTO> GetApplicationDetailsAsync(Guid requestId)
		{
			return await _srvServiceRequest.GetRequestDetailsAsync(requestId);
		}
		public async Task<EvaluationRequestDTO> GetEvaluationDetailsAsync(Guid requestId)
		{
			return await _evaluationRequestService.GetEvaluationDetailsAsync(requestId);
		}
		public async Task<ServiceRequestDTO> HandleServiceRequestAsync(ActionFormDTO? actionFormDTO, Guid? planId,
			Guid serviceId, string actionName, string fieldValuesJson, List<AssignUserDTO?> assignUsers, List<EvalTeamRequestDto> teamUsers,
			IFormFileCollection files, string remarks, bool saveAsDraft = false)
		{
			string lang = _requestInfo.Lang;
			var requestId = actionFormDTO?.RequestId;

			var serviceObj = await SrvService.GetActiveAndOpenServiceById(serviceId);

			if (serviceObj == null)
			{
				throw new BusinessException(ExceptionMessage.IncompleteRequest);
			}
			var requestType = systemModuleSrv.GetRequestType(serviceObj);
			if (requestType == RequestType.Evaluation && (requestId == null || requestId == Guid.Empty))
			{
				throw new BusinessException("EvaluationRequest cannot be created from web app.");
			}
			var action = await SrvAction.GetActionByBackendNameAsync(serviceObj.Id, actionName) ??
						 await SrvAction.GetInitialActionAsync(serviceId, actionName);

			if (action == null || action.ActionType == null)
			{
				throw new BusinessException(ExceptionMessage.IncompleteRequest);
			}

			var (filesWithFieldId, othersAttachement) = _srvAttachments.SeparateFilesByFieldId(files);

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

				

				var request = new ServiceRequest
				{
					Id = Guid.NewGuid(),
					StatusId = status.Id,
					ServiceId = serviceId,
					//OrgTreeId = OrgTreeId,
					//EvaluationRequestId  = EvaluationRequestId ,
					//InitialHistoryId = InitialHistoryId,
					

				};

				var validateRequestTask = ValidateCanCreateRequest(planId, serviceObj);
				if (actionFormDTO?.FieldValues != null)
				{
					var validateActionTask = SrvAction.ValidateActionAndActionFieldAsync(null, actionFormDTO?.FieldValues!, remarks, othersAttachement, serviceObj, status.Id, action, fileFields, saveAsDraft);
					await Task.WhenAll(validateRequestTask, validateActionTask);

					var validatedFields = await validateActionTask;

					actionFormDTO!.FieldValues = (await _srvAttachments.UploadAndInsertAttachments(validatedFields.ToList(),requestType, request.Id, request.EvaluationRequestId, fileFields, filesWithFieldId)).Cast<FieldValueDTO?>().ToList();
				}




				resultRequest!.Id = request.Id;

				var actionResult = await _performActionBL.PerformAction(request, requestType, serviceObj, actionFormDTO!.FieldValues!, action.BackendName, assignUsers.Where(c => c!.IsSelected).ToList()!,null, remarks, saveAsDraft);

				var otherAttachmentsTask = _srvAttachments.UploadAndInsertOtherAttachments(othersAttachement, actionResult.actionlog,requestType,requestId, request.EvaluationRequestId);
				var sequence = request.Sequence;
				var requestNumber = DateTime.Now.ToString(serviceObj.ReqNumberDef ?? "", new CultureInfo("en-US")) + sequence;

				request.RequestNumber = requestNumber;
				resultRequest.RequestNumber = requestNumber;

				_srvServiceRequest.InsertRequest(request);

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
				var application = await GetRequestUnifiedAsync(requestId!.Value,requestType, true);

				var allFields = JsonConvert.DeserializeObject<List<FieldValueDTO?>>(fieldValuesJson);
				var validatedFields = await SrvAction.ValidateActionAndActionFieldAsync(application, allFields!, remarks, othersAttachement, serviceObj, application.StatusId, action, fileFields, saveAsDraft);


				actionFormDTO!.FieldValues = (await _srvAttachments.UploadAndInsertAttachments(validatedFields.ToList(),requestType, requestId, application.EvaluationRequestId, fileFields, filesWithFieldId)).Cast<FieldValueDTO?>().ToList();

				var actionResult = await _performActionBL.PerformAction(application, requestType,serviceObj, actionFormDTO.FieldValues!, actionName, assignUsers.Where(c => c!.IsSelected).ToList()!, teamUsers, remarks, saveAsDraft);

				var otherAttachments = await _srvAttachments.UploadAndInsertOtherAttachments(othersAttachement, actionResult.actionlog,requestType, application.Id, application.EvaluationRequestId);


				await uow.CommitAsync();

				if (actionResult.notifications != null && !saveAsDraft)
				{
					await SrvNotification.HandleNotification(actionResult.notifications, application, actionResult.actiondb.Id, lang, remarks, otherAttachments);
				}
				resultRequest = new ServiceRequestDTO { Id = requestId, RequestNumber = application.RequestNumber };
			}

			return resultRequest;
		}
		public async Task<ServiceRequest?> GetRequestUnifiedAsync(Guid requestId, RequestType requestType, bool useMainUow = false)
		{
			if (requestType == RequestType.Evaluation)
			{
				var er = await _evaluationRequestService.GetEvaluationRequestByIdAsync(requestId, useMainUow);
				return er == null ? null : _evaluationRequestService.MapEvaluationToServiceRequest(er);
			}

			return await _srvServiceRequest.GetSrvServiceRequestByIdAsync(requestId, useMainUow);
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
	
		public async Task<bool> ValidateCanCreateRequest(Guid? planId, Service serviceObj)
		{
			if (serviceObj == null)
			{
				throw new BusinessException(ExceptionMessage.IncompleteRequest);
			}

			#region maxCountOpen

			int defaultMaxCountOpen = int.Parse(ServiceSettings.MaxCountOpen);
			int maxCountOpen = defaultMaxCountOpen;

			//var serviceSettingsJson = serviceObj.ServiceSettings ?? "{}";
			//var serviceSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(serviceSettingsJson);

			//if (!TryGetSettingValue(serviceSettings!, ServiceSettings.MaxCountOpen, out maxCountOpen))
			//{
			//	var systemSettingsJson = await cacheDataProvider.GetSystemSettingValue(SystemSettings.ServiceSettings);
			//	if (!string.IsNullOrEmpty(systemSettingsJson))
			//	{
			//		var systemSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(systemSettingsJson);
			//		if (systemSettings == null || !TryGetSettingValue(systemSettings, ServiceSettings.MaxCountOpen, out maxCountOpen))
			//		{
			//			SrvService.UpdateServiceSettingsAsync(serviceObj, ServiceSettings.MaxCountOpen, maxCountOpen);
			//		}
			//	}
			//}

			await ValidateIfThereIsOpenedRequestForServiceAsync(planId, serviceObj, maxCountOpen);

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
		private async Task ValidateIfThereIsOpenedRequestForServiceAsync(Guid? PlanId, Service serviceObj, int maxCountOpen)
		{

			var user = await SrvUser.GetByIDActiveNonDeleted(userInfo!.UserId!.Value);


			var isMinistry = user is MinistryUser;


			var openRequests = await serviceScopeFactory.CreateScopedUow()
									.GetRepository<ServiceRequest>()
										.GetAllQueryFiltered()
										.Include(c => c.Status)
										.Where(c => c.PlanId == PlanId || serviceObj.Initialservice)
										.Where(c => c.ServiceId == serviceObj.Id && c.Status!.IsOpen && !c.IsDeleted)
										//.Where(c => c.OrgTreeId == userInfo!.UserId || isMinistry)
										.CountAsync();


			if (openRequests >= maxCountOpen)
			{
				throw new BusinessException(ExceptionMessage.lblRequestAlreadyOpened);
			}
		}

		public async Task<string> GetAttachmentUrlAsync(Guid attachmentId, Guid requestId, Guid EvlReqtId )
		{
			if (requestId != Guid.Empty && EvlReqtId  != Guid.Empty)
			{
				throw new UnauthorizedAccessException("You do not have permission to view this request.");
			}

			var userId = userInfo.UserId;

			if (requestId != Guid.Empty &&
				!await _srvServiceRequest.HasAccessToRequestAsync(requestId, userId!.Value))
			{
				throw new UnauthorizedAccessException("You do not have permission to view this request.");
			}

			//if (EvlReqtId  != Guid.Empty &&
			//	!await _srvScholarship.HasAccessToScholarship(EvlReqtId ))
			//{
			//	throw new UnauthorizedAccessException("You do not have permission to view this request.");
			//}

			var url = await _srvAttachments.GetAttachmentById(attachmentId, requestId);
			return url!;
		}

		public async Task<NdaApproveResponse> ApproveNda(NdaApproveRequest dto)
		{
			return  await _srvEvaluationRequestAssignment.ApproveNda(dto);
		}
	}
}
