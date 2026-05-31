using AutoMapper;
using Azure.Core;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.SystemLog;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class SrvServiceRequest(AzureBlobStorageService StorageService,SystemModuleSrv SrvSystemModule, SrvAction SrvAction, SrvActionTransactionsLog SrvActionTransactionsLog, SrvField SrvField, SrvAttachments SrvAttachments, SrvPartyType SrvPartyType, SrvDropdown SrvDropdown, SrvActionStatusConfiguration SrvActionStatusConfiguration, SrvStatus SrvStatus, SrvUser srvUser, IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo _requestInfo)
             : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
    {
        public async Task<ServiceRequest?> GetSrvServiceRequestByIdAsync(Guid requestId, bool useMainUow = false)
        {
            if (useMainUow)
            {
                return await uow.GetRepository<ServiceRequest>()
                    .GetAllQueryFiltered(x => x.Id == requestId)
                    .Include(x => x.Status)
                    .Include(x => x.Service)
                    .FirstOrDefaultAsync();
            }

            using var scope = serviceScopeFactory.CreateScopedUow();

            return await scope.GetRepository<ServiceRequest>()
                .GetAllQueryFiltered(x => x.Id == requestId)
                .Include(x => x.Status)
                .Include(x => x.Service)
                .FirstOrDefaultAsync();
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
                                        .ThenInclude(x => x!.StatusPreventPartyTypes!.Where(p => p.IsActive && !p.IsDeleted))
                                        .Include(c => c.Service)
                                        .Include(c => c.OrgTree)
                                        .AsSplitQuery()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false);

            return Request;

        }
        public async Task<Guid?> GetOrgTreeIdByRequestIdAsync(Guid reqId)
        {
			 using var Scope = serviceScopeFactory.CreateScopedUow();

			var request = await Scope.GetRepository<EvaluationRequest>()
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
        public async Task<WebAppPlanRequestsDTO?> GetPlanRequestsAsync( FilterRequestsDTO model)
        {
            string lang = _requestInfo!.Lang;
            if (userInfo.UserId is null)
            {
                throw new UnauthorizedAccessException("UnAuthorized Data");
            }
            Guid userId = userInfo.UserId.Value;
            using var uow = serviceScopeFactory.CreateScopedUow();
            using var uow2 = serviceScopeFactory.CreateScopedUow();

            var moduleTask = SrvSystemModule.GetSystemModuleByRoutingAsync(ModuleType.EvaluationPlan);
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

            if (isMinistry)
            {
				var RequestsTask = await GetRequestsForMinistryUserAsync(uow, userId, module, lang, time_Format, date_Format);
				filteredResult = await FilteredPlanRequestsAsync( isMinistry, RequestsTask, model);
				await FillPlanDetailsAsync(uow, filteredResult.Data);
				await UpdateRequestStatusesAsync(filteredResult.Data, module?.Id);
				return filteredResult;
			}
            else
            {
				return null;

			}

        }
		private async Task FillPlanDetailsAsync(UnitOfWork uow, List<ServiceRequestDTO> requests)
		{
			if (requests == null || !requests.Any())
				return;

			var requestIds = requests.Select(x => x.Id).ToList();

			var planFields = await uow.GetRepository<ServiceRequestFieldsValue>()
				.GetAll()
				.Include(x => x.Field)
					.ThenInclude(f => f!.FieldType)
				.Where(x =>
					requestIds.Contains(x.RefId) &&
					x.Field != null &&
					x.Field.FieldType != null &&
					x.Field.FieldType.BackendName == FieldTypeConstant.EvaluationPlan &&
					!string.IsNullOrWhiteSpace(x.Value))
				.Select(x => new
				{
					x.RefId,
					x.Value
				})
				.ToListAsync();

			var planFieldsMap = planFields
				.GroupBy(x => x.RefId)
				.ToDictionary(x => x.Key, x => x.First().Value);

			foreach (var item in requests)
			{
				if (!planFieldsMap.TryGetValue(item.Id!.Value, out var planJson) ||
					string.IsNullOrWhiteSpace(planJson))
				{
					continue;
				}

				var planDto = JsonConvert.DeserializeObject<CreateEvaluationPlanDto>(planJson);

				item.PlanDateFrom = planDto?.StartDate;
				item.PlanDateTo = planDto?.EndDate;
				item.PlanName = planDto?.Name;
				item.SchoolsCount = planDto?.Schools?.Count ?? 0;
			}
		}
		public async Task<ServiceRequestDTO> GetRequestDetailsAsync(Guid id, CancellationToken ct = default)
        {
            var lang = _requestInfo.Lang;
            var userId = userInfo.UserId ??  throw new BusinessException(ExceptionMessage.UserNotFound);

            var request = await GetRequestByIdAsync(id);
            if (request == null)
                throw new BusinessException(ExceptionMessage.lblRequestNotValid);

            if (request.Service == null)
                throw new BusinessException(ExceptionMessage.lblRequestNotValid);

            if (request.Status == null)
                throw new BusinessException(ExceptionMessage.lblRequestNotValid);

            var userTask = srvUser.GetByIDActiveNonDeleted(userId);
            var moduleTask = SrvSystemModule.GetSystemModuleByIdAsync(request.Service.SystemModuleId);
            var fieldsTask = GetRequestFieldsValueAsync(request);

            await Task.WhenAll(userTask, moduleTask, fieldsTask);

            var user = await userTask;
            if (user == null)
                throw new BusinessException(ExceptionMessage.UserNotFound);

            var module = await moduleTask;
            var formGroups = await fieldsTask;

            var preventPartyTypes = request.Status.StatusPreventPartyTypes;
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
                    if (!allowed)
                        throw new UnauthorizedAccessException(ExceptionMessage.lblNoPermissionForViewRequest);
                }
            }

            bool hasFieldHistoryPermission = false;
            bool hasAllFieldHistoryPermission = false;

            // hasFieldHistoryPermission = await permissionsService.CanViewFieldHistory(user, request, module, ct);
            // hasAllFieldHistoryPermission = await permissionsService.CanViewAllFieldHistory(user, request, module, ct);

            var attachmentsTask = GetAllRequestAttachmentsAsync(request.Id, lang);
            var actionTransactionsTask = SrvActionTransactionsLog.GetActionLog(request.Id, request.ServiceId, module?.Id, user);
            var actionsTask = SrvActionStatusConfiguration.GetActionsByStatus(request.ServiceId, request.StatusId, request.Id, request.PlanId, lang);

            await Task.WhenAll(attachmentsTask, actionTransactionsTask, actionsTask);

			var ServiceRequest= new ServiceRequestDTO
			{
				formGroups = formGroups,
				Attachments = await attachmentsTask,
				ActionTransactions = await actionTransactionsTask,
				Actions = await actionsTask,
				RequestNumber = request.RequestNumber,
				Status =  SrvStatus.GetStatusDisplayName(request.StatusId, module?.Id),
				ServiceId = request.ServiceId,
				Service = lang == "ar" ? request.Service.NameAr : request.Service.NameEn,
				CanViewFieldHistory = hasFieldHistoryPermission,
				CanViewAllFieldHistory = hasAllFieldHistoryPermission
			};
			return ServiceRequest;
		}
        public async Task<List<JsTreeNodeDto>> GetScopeList_old(Guid partyId)
        {
           using var uow = serviceScopeFactory.CreateScopedUow();
            var departmentid=await uow.GetRepository<EvaluationParty>().GetAllNonDeleted().Where(x=>x.Id==partyId).Select(x=>x.DepartmentId).FirstOrDefaultAsync();

            var scopes = await uow.GetRepository<Scope>()
                                .GetAllNonDeleted()
                                .Include(x => x.ScopeType)
                                .ToListAsync();
            var treeData = new List<JsTreeNodeDto>();

            foreach (var scope in scopes)
            {
                var parentScopeTypeId = scope.ScopeType.ParentId;

                // find a parent scope whose ScopeType == parent ScopeType
                var parentScope = scopes
        .FirstOrDefault(x => x.ScopeTypeId == parentScopeTypeId);

                treeData.Add(new JsTreeNodeDto
                {
                    id = scope.Id.ToString(),
                    text = (_requestInfo.Lang == "ar" ? scope.NameAr : scope.NameEn),
                    parent = parentScope != null ? parentScope.Id.ToString() : "#"
                });
            }

            return treeData;

        }
		public async Task<List<JsTreeNodeDto>> GetScopeList(Guid partyId)
		{
			using var uow = serviceScopeFactory.CreateScopedUow();

            var departmentId = requestInfo.DepId; 

			var currentAcademicYearId = await uow.GetRepository<AcademicYear>()
				.GetAllNonDeleted()
				.Where(x => x.IsCurrent && x.DepartmentId== departmentId) 
				.Select(x => x.Id)
				.FirstOrDefaultAsync();

			var scopeAcademicYears = await uow.GetRepository<ScopeAcademicYear>()
				.GetAllNonDeleted()
				.Include(x => x.Scope)
				.Where(x =>
					x.DepartmentId == departmentId &&
					x.AcademicYearId == currentAcademicYearId)
				.OrderBy(x => x.OrderNo)
				.ToListAsync();

			var treeData = scopeAcademicYears.Select(x => new JsTreeNodeDto
			{
				id = x.ScopeId.ToString(),
				text = _requestInfo.Lang == "ar"
					? x.Scope?.NameAr ?? string.Empty
					: x.Scope?.NameEn ?? string.Empty,

				parent = x.ScopeParentId.HasValue
					? x.ScopeParentId.Value.ToString()
					: "#"
			}).ToList();

			return treeData;
		}
		public async Task<List<SupportedFileDto>> GetSupportedFiles(Guid requestId)
        {
            using var uow = serviceScopeFactory.CreateScopedUow();

            var attachments = await uow.GetRepository<EvalAttachment>()
                .GetAllNonDeleted()
                .Where(x => x.EvaluationRequestId == requestId)
                .Select(x => new SupportedFileDto
                {
                    UiFileName = x.UiFileName,
                    FileUrl = StorageService.GenerateSasToken(
                        x.FileName,
                        2,
                        x.UiFileName,
                        false,
                        StorageContainerType.evaluation
                    )
                }).ToListAsync();
            return attachments;
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

            if (request.Status!.StatusPreventPartyTypes!.Any(x => user.UserPartTypes!.Any(c => c.PartyTypeId == x.PartyTypeId)))
            {
                throw new BusinessException(ExceptionMessage.lblNoPermissionForRequestStatus);
            }
            if (isMinistry)
            {

                if (user.Id == request.CreateById)
                    return true;

                if (await ValidateMinistryUserAccessAsync(userId, module?.Id, request.Id))
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
        public async Task<bool> SaveSupportFiles(IFormFile file,Guid EvaluationRequestId,Guid ScopeId)
        {
            if (file!=null)
            {
                var uploadedFile = await StorageService.UploadFormFileAsync(file);

                if (uploadedFile != null)
                {
                    var orgtreeid=await uow.GetRepository<EvaluationRequest>().GetAllNonDeleted().Where(x=>x.Id==EvaluationRequestId).Select(x=>x.OrgTreeId).FirstOrDefaultAsync();


                    var attachment = new EvalAttachment
                    {
                        OrgTreeId = orgtreeid,
                        EvaluationRequestId = EvaluationRequestId,
                        ScopeId = ScopeId,
                        FileName = uploadedFile.CustomFileName,
                        UiFileName = uploadedFile.FileName,
                        FileExtension = Path.GetExtension(uploadedFile.FileName),
                        FileSize = uploadedFile.FileLength!.Value,

                    };

                    uow.GetRepository<EvalAttachment>().Insert(attachment);
                    await uow.CommitAsync();
                }
                else
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.IncompleteRequest);
                }
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.IncompleteRequest);
            }
            return true;
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
        private async Task<IQueryable<ServiceRequestDTO>> GetRequestsForMinistryUserAsync(UnitOfWork uow, Guid userId, SystemModule module, string lang, string timeFormat, string dateFormat)
        {
            var permissionTasks = new
            {
                IsAllowedToViewAllRequests = IsAllowedToViewAllRequestsAsync(userId),
                IsAllowedToViewAllRequestsWithoutFiltration = SrvPartyType.IsAllowedToViewAllRequestsWitoutFilterationAsync(userId, module?.Id),
                UserPartyTypeData = SrvPartyType.GetUserPartyTypeData(userInfo.UserId!)
            };

            IQueryable<ServiceRequest> baseQuery = uow
                .GetRepository<ServiceRequest>()
                .GetAllActiveNonDeleted()
				.Include(c => c.ServiceRequestFieldsValues!)
	                .ThenInclude(x => x.Field)
		                .ThenInclude(f => f!.FieldType)
				.Include(c => c.Service)
                .Include(c => c.Service!.RequestShowPartyType)
                .Include(c => c.Status)
                .Include(c => c.Status!.StatusPreventPartyTypes)
                .Include(c => c.OrgTree)
                .Include(c => c.Plan)
                    .ThenInclude(s => s!.PlanStatus);


            // Apply service type and basic filters
            baseQuery = baseQuery.AsSplitQuery().Where(c =>
                !c.Status!.StatusPreventPartyTypes!.Any(sp => userInfo.PartyTypes.Contains(sp.PartyTypeId) && sp.IsActive == true && sp.IsDeleted == false) &&
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


			var requests = baseQuery.Select(c => new ServiceRequestDTO
            {
                RequestNumber = c.RequestNumber,
                Status = lang == "ar" ? c.Status!.NameAr : c.Status!.NameEn,
                StatusColor = c.Status!.ColorCode,
                StatusId = c.StatusId,
                StatusISOPen = c.Status.ServiceStatusType!.IsOpen,
                ServiceId = c.Service!.Id,
                Service = lang == "ar" ? c.Service.NameAr : c.Service.NameEn,
                CreateOn = c.CreateDate.ToString(dateFormat),
                CreateOnTime = c.CreateDate.ToString(timeFormat),
                CreateDate = c.CreateDate,
                Id = c.Id,
                icon = c.Service.Icon,
				

			});


			return requests;
		}
        public async Task<bool> IsAllowedToViewAllRequestsAsync(Guid userId)
        {
			using var Scope = serviceScopeFactory.CreateScopedUow();

			var result = await Scope
									   .GetRepository<UserPartyType>()
                                        .GetAllQueryFiltered()
                                        .Include(x => x.PartyType)
                                        .Where(x => x.UserId == userId)
                                        .Where(x => x.PartyType!.DepartmentId == requestInfo.DepId)
                                        .AnyAsync(x => x.PartyType!.CanViewAllRequests);

            return result;
        }
        private IQueryable<ServiceRequest> ApplyUserAccessFilters(IQueryable<ServiceRequest> baseQuery, IEnumerable<UserPartyTypeDTO> userPartyTypeData, Guid userId, bool isAllowedToViewAllRequests)
        {
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
        private async Task<bool> ValidateMinistryUserAccessAsync(Guid userId, Guid? moduleId, Guid? requestId)
        {
            if (moduleId is null)
            {
                return false;
            }

           using var uow = serviceScopeFactory.CreateScopedUow();

            var userPartyDataTask = SrvPartyType.GetUserPartyTypeData(userInfo.UserId!);



            var hasRequestAccessTask = uow.GetRepository<RequestAssignment>()
                                       .GetAllQueryFiltered()
                                       .AnyAsync(a => a.ServiceRequestId == requestId &&
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
        public async Task<List<AttachementDTO?>?> GetRequestAttachmentsAsync(Guid id, string lang)
        {
			using var Scope = serviceScopeFactory.CreateScopedUow();
			using var Scope1 = serviceScopeFactory.CreateScopedUow();

			var fields = await Scope.GetRepository<ServiceRequestFieldsValue>()
                                       .GetAllQueryFiltered()
                                       .Include(c => c.Field)
                                       .ThenInclude(c => c!.FieldType)
                                       .Where(c => c.RefId == id &&
                                                   (c.Field!.FieldType!.BackendName == FieldTypeConstant.file
                                                    || c.Field.FieldType.BackendName == FieldTypeConstant.fileV2))
                                       .Select(c => c.Value).ToListAsync();

            var attachments = await Scope1.GetRepository<EvalAttachment>()
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
           using var uow = serviceScopeFactory.CreateScopedUow();

            var fields = await uow.GetRepository<ServiceRequestFieldsValue>()
                .GetAllQueryFiltered()
                .Include(f => f.Field)
                .ThenInclude(f => f!.FieldType)
                .Where(f => f.RefId == id)
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
			using var Scope = serviceScopeFactory.CreateScopedUow();

			var requestFieldsValue = await Scope.GetRepository<ServiceRequestFieldsValue>()
                .GetAllActiveNonDeleted()
                .Include(x => x.Field)
                .ThenInclude(x => x!.FieldViewConditions)
                .Where(c => c.RefId == request.Id && !hiddenFieldsIds.Contains(c.FieldId))
                .Select(c => new
                {
                    c.FieldId,
                    c.Field!.EvalFormId,
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
                    fieldValue = await SrvDropdown.ResolveDropDownTextAsync(
                        lang,
                        c.Value!,
                        c.DropDownTypeId.Value,

						request.EvaluationRequestId ?? request.Id,
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
                                        updatedValue = await SrvDropdown.ResolveDropDownTextAsync(
                                            lang,
                                            updatedValue,
                                            field.DropDownTypeId.Value,
											request.EvaluationRequestId ?? request.Id,
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
                    formId = c.EvalFormId,
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
			using var Scope = serviceScopeFactory.CreateScopedUow();

			var RequestFieldsValue = await Scope.GetRepository<ServiceRequestFieldsValue>()
                                        .GetAllActiveNonDeleted()
                                        .Include(x => x.Field)
                                        .Include(x => x.Field!.MappingField)
                                        .Include(x => x.Field!.FieldType)
                                        .Where(c => c.RefId == requestId).ToListAsync();


            return RequestFieldsValue;
        }
        private async Task<WebAppPlanRequestsDTO> FilteredPlanRequestsAsync( bool isMinistry, IQueryable<ServiceRequestDTO> requests, FilterRequestsDTO model)
        {


            var result = new WebAppPlanRequestsDTO();
			var query = requests.ToQueryString();

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

                    if (model.RequestDateFrom.HasValue)
                    {
                        requests = requests.Where(x => x.CreateDate != null && x.CreateDate >= model.RequestDateFrom);
                    }

                    if (model.RequestDateTo.HasValue)
                    {
                        requests = requests.Where(x => x.CreateDate != null && x.CreateDate <= model.RequestDateTo);
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
                if (model.PlanId.HasValue)
                {
                    var resa = requests.ToList();
                    var res = requests.Where(s => s.planId == model.PlanId).ToList();
                    requests = requests.Where(x => x.planId == model.PlanId);
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
                            item.ActionCount = await SrvActionStatusConfiguration.GetActionCountByStatusAsync(item.StatusId);
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
			using var Scope = serviceScopeFactory.CreateScopedUow();
			using var Scope1 = serviceScopeFactory.CreateScopedUow();

			var fieldValue = await Scope.GetRepository<ServiceRequestFieldsValue>()
                                    .GetAllQueryFiltered()
                                    .Include(c => c.Field!.FieldType)
                                    .Where(c => c.FieldId == fieldId && c.RefId == requestId)
                                    .FirstOrDefaultAsync();

            if (fieldValue == null) return new List<FieldTransactionDTO>();

            var dateTime_Format = await cacheDataProvider.GetSystemSettingValue(SystemSettings.DateFormat);

            var transactions = await Scope1
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
        public async Task<Dictionary<Guid, List<ServiceDTO>>> GetServicesByStatusesAsync(List<Guid> statusIds, Guid moduleId, string lang)
        {
            var result = new Dictionary<Guid, List<ServiceDTO>>();
            var today = DateTime.Today;

			if (userInfo.PartyTypes == null || !userInfo.PartyTypes.Any())
			{
				throw new BusinessException(ExceptionMessage.lblNoPartyTypeFound);
			}

			var distinctStatusIds = statusIds.Distinct().ToList();
            if (!distinctStatusIds.Any())
                return result;

			

			using var scopedUow = serviceScopeFactory.CreateScopedUow();

            //var initiators = (await cacheDataProvider.GetServiceIntiator())
            //    .Where(c => userInfo.PartyTypes.Contains(c.PartyTypeId) && c.service?.SystemModule?.DepartmentId == requestInfo.DepId)
            //    .Select(c => c.serviceId)
            //    .ToHashSet();

            //var statusConfig = (await cacheDataProvider.GetServiceStatusConfiguration())
            //	.Where(c => distinctStatusIds.Contains(c.CurrentStatusId)
            //			 && c.Service!.SystemModuleId == moduleId)
            //	.Select(c => new { c.CurrentStatusId, c.ServiceId })
            //	.ToList();

            //var allowedServiceIds = statusConfig
            //	.Where(c => initiators.Contains(c.ServiceId))
            //	.Select(c => c.ServiceId)
            //	.Distinct()
            //	.ToList();

            var services = await scopedUow
                .GetRepository<Service>()
                .GetAllQueryFiltered()
                .Include(x => x.SystemModule)
                .Where(s =>
                    //allowedServiceIds.Contains(s.Id) &&
                    s.Initialservice != true &&
                    s.SystemModule!.SystemModuleTypeId == moduleId && s.SystemModule.DepartmentId == requestInfo.DepId)
                //&&
                //s.StartDate.HasValue &&
                //today >= s.StartDate.Value &&
                //(!s.EndDate.HasValue || s.EndDate.Value.AddDays(1) >= today))
                .Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    BackendName = s.BackendName,
                    Icon = s.Icon,
                    NameAr = lang == "ar" ? s.NameAr : s.NameEn
                })
                .ToListAsync();

            foreach (var statusId in distinctStatusIds)
            {
                //var serviceIdsForStatus = statusConfig
                //	.Where(c => c.CurrentStatusId == statusId)
                //	.Select(c => c.ServiceId)
                //	.ToHashSet();

                result[statusId] = services
                    //.Where(s => serviceIdsForStatus.Contains(s.Id.Value))
                    .ToList();
            }

            return result;
        }
    }
}
