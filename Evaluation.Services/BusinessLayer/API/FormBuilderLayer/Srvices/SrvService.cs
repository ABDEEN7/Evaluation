using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceDTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;


namespace Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices
{
    public class  SrvService(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow,SrvUser srvUser, SrvActionStatusConfiguration SrvActionStatusConfiguration, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceProvider serviceProvider, RequestInfo requestInfo)
            : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
        {
        public async Task<ServiceDTO?> GetServiceByIdAsync(Guid serviceId, string Lang)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var service = await scopedUow.GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (service == null)
                return null;

            return MapToServiceDTO(service, Lang);
        }
        public async Task<Service?> GetServiceById(Guid serviceId)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var service = await scopedUow.GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .Include(x=>x.SystemModule)
                .Include(x=>x.SystemModule!.SystemModuleType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return service;

        }
        public async Task<Service?> GetActiveAndOpenServiceById(Guid serviceId,bool IsIntialAction)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			DateTime today = DateTime.Now;
            var service = await scopedUow.GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .Include(c=>c.SystemModule)
                .Include(c=>c.SystemModule!.SystemModuleType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

			if (IsIntialAction && service!.SystemModule?.SystemModuleType?.BackendName != ModuleType.EvaluationRequest)
			{
				bool hasDateRange = service.StartDate.HasValue || service.EndDate.HasValue;

				if (hasDateRange)
				{
					bool isInValidDateRange =
						(!service.StartDate.HasValue || today >= service.StartDate.Value.Date) &&
						(!service.EndDate.HasValue || today <= service.EndDate.Value.Date);

					if (!isInValidDateRange)
						return null;
				}
			}

			return service;

        }
        public async Task<Service?> GetIntialService(Guid moduleId)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var service = await scopedUow.GetRepository<Service>()
                .GetAllQueryFiltered(x => x.SystemModuleId == moduleId && x.Initialservice)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return service;

        }
        public async Task<List<SelectListItemDTO>> GetServiceByModuleIdAsync(Guid moduleId)
        {
            string lang = requestInfo.Lang;
            var userPartyTypes = userInfo.PartyTypes;
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var repo = scopedUow.GetRepository<Service>();

            var query = repo.GetAllQueryFiltered(x => x.SystemModuleId == moduleId)
                            .Include(x => x.RequestShowPartyType)
                            .AsNoTracking();

            var filtered = query.Where(x => x.RequestShowPartyType != null &&
                                            x.RequestShowPartyType.Any(pt => userPartyTypes.Contains(pt.PartyTypeId)));

            var services = await filtered.Select(c => new SelectListItemDTO
            {
                Value = c.Id.ToString(),
                TextAr = c.NameAr,
                TextEn = c.NameEn,
                Text = lang == "ar" ? c.NameAr : c.NameEn,
                Parent = moduleId
            }).ToListAsync();

            return services;
        }
        public async Task<Guid> GetModuleIdForServiceAsync(Guid serviceId)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var SystemModulesId = await scopedUow.GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .Select(x => x.SystemModuleId)
                .FirstOrDefaultAsync();
            return SystemModulesId;
        }
        private ServiceDTO MapToServiceDTO(Service service, string Lang)
        {
            var resultService = service.Adapt<ServiceDTO>();
            resultService.Name = Lang == "ar" ? service.NameAr : service.NameEn;
            resultService.Description = Lang == "ar" ? service.DescriptionAr : service.DescriptionEn;
            return resultService;
        }
        public async Task<ServiceDTO> GetServiceDetailsAsync(Guid serviceId, string lang)
        {
            DateTime today = DateTime.Now;

            if (serviceId == Guid.Empty)
                throw new ArgumentException("Service ID cannot be null or empty.", nameof(serviceId));
			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var service = await scopedUow.GetRepository<Service>().GetAllQueryFiltered()
                                  .AsNoTracking()
                                  .Include(x => x.SystemModule)
                                  //.Include(x => x.SchServiceStatusConfiguration)
                                  .FirstOrDefaultAsync(c => c.Id == serviceId && today >= c.StartDate && (c.EndDate == null || c.EndDate.Value.AddDays(1) >= today));


            if (service == null)
                throw new BusinessException(ExceptionMessage.ServiceNotFound);

            var dto = service.Adapt<ServiceDTO>();

            Guid? planId = null;

            var CheckActionCondition=true;
            if (!service.Initialservice)
            {
                //dto.EligableScholarShips = await GetEligibleScholarShipsAsync(service, lang);

                //if (dto.EligableScholarShips == null)
                //    throw new BusinessException(ExceptionMessage.IncompleteRequest);

                //if (dto.EligableScholarShips.Count == 1)
                //{
                //    CheckActionCondition = true;
                //    planId = dto.EligableScholarShips.FirstOrDefault()?.Id;
                //}
                //else if (dto.EligableScholarShips.Count > 1)
                //{
                //    planId = null;
                //    CheckActionCondition = false;
                //}

            }

            var actions = await SrvActionStatusConfiguration.GetActionsByStatus(service.Id, statusId: null,requestId: null, planId: planId,lang: lang, CheckActionCondition);

            dto.Actions = actions;
            dto.Routing = service.SystemModule?.Routing;

            return dto;
        }
		public async Task<ServiceDTO> GetServiceDetailsByModuleTypeAsync(
		Guid departmentId,
		string moduleTypeBackendName,
		string lang,
		Guid? serviceId = null,
		bool? initialService = null,
		Guid? planId = null,
		bool checkActionCondition = true)
		{
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");

			if (departmentId == Guid.Empty)
				throw new ArgumentException("departmentId cannot be empty.", nameof(departmentId));

			using var scopedUow = serviceScopeFactory.CreateScopedUow();

			var employeeUserPartyTypes =
				await srvUser.GetEmployeeUserPartyTypeIdsAsync(userId, departmentId);

			//if (!employeeUserPartyTypes.Any())
			//	throw new BusinessException(ExceptionMessage.ServiceNotFound);

			var q = scopedUow.GetRepository<Service>()
				.GetAllQueryFiltered()
				.AsNoTracking()
				.Include(x => x.ServiceInitiatorPartyType)
				.Include(x => x.SystemModule)
				.Include(x => x.SystemModule!.Department)
				.Include(x => x.SystemModule!.SystemModuleType)
				.Where(s => s.SystemModule!.DepartmentId == departmentId &&
							s.SystemModule.SystemModuleType!.BackendName == moduleTypeBackendName);
			//.Where(c => c.ServiceInitiatorPartyType!
			//			   .Any(x => employeeUserPartyTypes.Contains(x.PartyTypeId)))
			//.Where(c => today >= c.StartDate &&
			//			(c.EndDate == null || c.EndDate.Value.AddDays(1) >= today));
			if (serviceId.HasValue)
				q = q.Where(s => s.Id == serviceId.Value);

			if (initialService.HasValue)
				q = q.Where(s => s.Initialservice == initialService.Value);

			var service = await q.FirstOrDefaultAsync();

			if (service == null)
				throw new BusinessException(ExceptionMessage.ServiceNotFound);

			var dto = new ServiceDTO
			{
				Id = service.Id,
				Name = lang == "en" ? service.NameEn : service.NameAr,
				Routing = service.SystemModule?.Routing
			};

			var actions = await SrvActionStatusConfiguration.GetActionsByStatus(
				service.Id,
				statusId: null,
				requestId: null,
				planId: planId,
				lang: lang,
				CheckActionCondition: checkActionCondition
			);

			dto.Actions = actions;
			return dto;
		}

		public async Task<List<ServiceDTO>> GetServicesbyDepartementAndPartyType(string? departmentRoute, Guid? userProfileId)
        {
            List<Guid>? serviceList = null;
            using (var Scoped = serviceScopeFactory.CreateScopedUow())
            {
                var Module = await Scoped.GetRepository<SystemModule>().GetAllQueryFiltered().Where(c => c.Routing == departmentRoute).FirstOrDefaultAsync();
              
                if (Module == null)
                {
                    throw new BusinessException(ExceptionMessage.ServiceNotFound);
                }
                DateTime today = DateTime.Now;
                IQueryable<Service> query = Scoped.GetRepository<Service>()
                                            .GetAllQueryFiltered(c => today >= c.StartDate && (null == c.EndDate || c.EndDate.Value.AddDays(1) >= today))
                                            .Where(c => c.SystemModuleId == Module.Id)
                                            .OrderBy(x => x.OrderNo);

                if (userProfileId is not null)
                {

                    var employeeUserPartyTypes = await Scoped.GetRepository<UserPartyType>()
                                                   .GetAllQueryFiltered(x => x.UserId == userProfileId)
                                                   .Include(x => x.PartyType!.Department)
                                                   .Where(x => x.PartyType!.DepartmentId == Module.DepartmentId && x.PartyType.IsEmployeePartyType)
                                                   .Select(x => x.PartyType!.Id)
                                                   .Distinct()
                                                   .ToListAsync();

                    if (employeeUserPartyTypes.Any())
                    {
                        serviceList = await Scoped.GetRepository<ServiceInitiatorPartyType>().GetAllQueryFiltered()
                                     .Where(c => employeeUserPartyTypes.Contains(c.PartyTypeId))
                                     .Where(c => c.service!.SystemModuleId == Module.Id)
                                     .Select(x => x.serviceId)
                                     .Distinct()
                                     .ToListAsync();

                    }
                    else// is student user
                    {
                        //var IsUserEmailVerified = await SrvUser.IsUserEmailVerifiedAsync(userProfileId.Value);
                        //if (!IsUserEmailVerified)
                        //{
                        
                        //    throw new BusinessException(ExceptionMessage.UserNotVerified);
                        //}
                            serviceList = await Scoped.GetRepository<ServiceInitiatorPartyType>().GetAllQueryFiltered()
                                    .Include(x => x.PartyType)
                                    .Where(c => !c.PartyType!.IsEmployeePartyType)
                                    .Where(c => c.service!.SystemModuleId == Module.Id)
                                    .Select(x => x.serviceId)
                                    .Distinct()
                                    .ToListAsync();

                       
                            var initialServiceIds = await Scoped.GetRepository<Service>()
                                .GetAllQueryFiltered(x => x.SystemModuleId == Module.Id && x.Initialservice)
                                .Select(x => x.Id)
                                .ToListAsync();

                            serviceList = serviceList.Except(initialServiceIds).ToList();
                        
                    }

                    query = query.Where(c => serviceList.Contains(c.Id));

                }

                var services = await query.Include(c => c.FormGroups).ToListAsync();

                var dtos = services.Adapt<List<ServiceDTO>>();
                return dtos;

            }
            
            
           

          
        }
        public async Task<bool> CanCreateDraftAsync(Guid? serviceId, Guid? ownerId)
		{
			using var scopedUow = serviceScopeFactory.CreateScopedUow();
			using var scopedUow1 = serviceScopeFactory.CreateScopedUow();
			using var scopedUow2 = serviceScopeFactory.CreateScopedUow();

			if (serviceId is null || ownerId is null)
                throw new ArgumentException("Service ID and Owner ID cannot be null or empty.");

            var service = await scopedUow
                                 .GetRepository<Service>().GetAllQueryFiltered()
                .FirstOrDefaultAsync(x => x.Id == serviceId);

            if (service == null)
                throw new BusinessException(ExceptionMessage.ServiceNotFound);

            var draftRequestsCount = await scopedUow1
										  .GetRepository<ServiceRequest>()
                                          .GetAllQueryFiltered()
                                          .Include(x => x.Status)
                                         .CountAsync(x => x.ServiceId == serviceId && x.StatusId == ownerId && x.Status!.ServiceStatusType!.IsOpen && x.Status.IsInitial);

            if (draftRequestsCount > 0)
                return false;

            var draftActionsCount = await scopedUow2
										  .GetRepository<ActionStatusConfiguration>()
                                          .GetAllQueryFiltered()
                                          .Include(x => x.ServiceAction)
                                          .Include(x => x.CurrentStatus)
                                          .CountAsync(x => x.ServiceAction!.ServiceId == serviceId && x.ServiceAction!.ActionType!.BackendName == ActionTypeKeys.SaveAsDraft);

            return draftActionsCount > 0;
        }
		public void UpdateServiceSettingsAsync(Service serviceObj, string settingKey, int value)
		{
			if (serviceObj == null)
			{
				throw new BusinessException(ExceptionMessage.ServiceNotFound);
			}

			var serviceSettings =
				!string.IsNullOrWhiteSpace(serviceObj.ServiceSettings)
					? JsonConvert.DeserializeObject<Dictionary<string, object>>(serviceObj.ServiceSettings)
					: new Dictionary<string, object>();

			if (serviceSettings == null)
			{
				serviceSettings = new Dictionary<string, object>();
			}

			serviceSettings[settingKey] = value;

			serviceObj.ServiceSettings = JsonConvert.SerializeObject(serviceSettings);

			uow.GetRepository<Service>().Update(serviceObj);
		}
		public async Task<bool> ValidateCanCreateRequest(Guid? planId, Guid? EvlReqId, Service serviceObj)
		{
			if (serviceObj == null)
				throw new BusinessException(ExceptionMessage.IncompleteRequest);

			int maxCountOpen = int.Parse(ServiceSettings.MaxCountOpen);

			var serviceSettingsJson = serviceObj.ServiceSettings ?? "{}";

			var serviceSettings =
				JsonConvert.DeserializeObject<Dictionary<string, object>>(serviceSettingsJson)
				?? new Dictionary<string, object>();

			if (!TryGetSettingValue(serviceSettings, nameof(ServiceSettings.MaxCountOpen), out maxCountOpen))
			{
				maxCountOpen = int.Parse(ServiceSettings.MaxCountOpen);

				 UpdateServiceSettingsAsync(serviceObj,nameof(ServiceSettings.MaxCountOpen),maxCountOpen);
			}

			await ValidateIfThereIsOpenedRequestForServiceAsync(planId,EvlReqId,serviceObj,maxCountOpen);

			return true;
		}

		private bool TryGetSettingValue(Dictionary<string, object> settings,string settingKey,out int result)
		{
			result = default;

			if (settings != null &&
				settings.TryGetValue(settingKey, out var settingValue) &&
				settingValue != null)
			{
				return int.TryParse(settingValue.ToString(), out result);
			}

			return false;
		}
		private async Task ValidateIfThereIsOpenedRequestForServiceAsync(Guid? PlanId, Guid? EvlReqId, Service serviceObj, int maxCountOpen)
		{

			var user = await srvUser.GetByIDActiveNonDeleted(userInfo!.UserId!.Value);


			var isMinistry = user is MinistryUser;

			using var scope = serviceScopeFactory.CreateScopedUow();

			var openRequests = await scope.GetRepository<ServiceRequest>()
										.GetAllQueryFiltered()
										.Include(c => c.Status)
										.Where(c => c.PlanId == PlanId || (serviceObj.Initialservice && serviceObj.SystemModule!.SystemModuleTypeId == ModuleTypeIds.EvaluationPlan))
										.Where(c => c.ServiceId == serviceObj.Id && c.Status!.ServiceStatusType!.IsOpen && !c.IsDeleted)
										.Where(c => c.EvaluationRequestId == EvlReqId || EvlReqId == null)
										.CountAsync();


			if (openRequests >= maxCountOpen)
			{
				throw new BusinessException(ExceptionMessage.lblRequestAlreadyOpened);
			}
		}

	}
}
