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
        

        public async Task<ServiceDTO> GetServiceByIdAsync(Guid serviceId, string Lang)
        {
            var service = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (service == null)
                return null;

            return MapToServiceDTO(service, Lang);
        }
        public async Task<Service> GetServiceById(Guid serviceId)
        {
            var service = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .Include(x=>x.SystemModule)
                .Include(x=>x.SystemModule.SystemModuleType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return service;

        }
        public async Task<Service> GetActiveAndOpenServiceById(Guid serviceId)
        {
            DateTime today = DateTime.Now;
            var service = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<Service>()
                .GetAllQueryFiltered(x => x.Id == serviceId)
                .Include(c=>c.SystemModule)
                .Include(c=>c.SystemModule.SystemModuleType)
                .Where(c =>(!c.StartDate.HasValue || today >= c.StartDate) && (null == c.EndDate || c.EndDate.Value.AddDays(1) >= today))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return service;

        }
        public async Task<Service?> GetIntialService(Guid moduleId)
        {
            var service = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<Service>()
                .GetAllQueryFiltered(x => x.SystemModuleId == moduleId && x.Initialservice)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return service;

        }

        public async Task<List<SelectListItemDTO>> GetServiceByModuleIdAsync(Guid moduleId)
        {
            string lang = requestInfo.Lang;
            var userPartyTypes = userInfo.PartyTypes;

            var repo = serviceScopeFactory.CreateScopedUow().GetRepository<Service>();

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
            var SystemModulesId = await serviceScopeFactory.CreateScopedUow()
                                       .GetRepository<Service>()
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

            var service = await serviceScopeFactory.CreateScopedUow()
                                   .GetRepository<Service>().GetAllQueryFiltered()
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

		public async Task<ServiceDTO> GetCreatePlanServiceDetailsAsync(Guid DepartementId, string lang)
		{
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");

			if (DepartementId == Guid.Empty)
				throw new ArgumentException("DepartementId ID cannot be null or empty.", nameof(DepartementId));

			//if (userInfo.UserId is null)
			//	throw new BusinessException(ExceptionMessage.UserInfoNotFound);

			//var userId = userInfo.UserId.Value;
			var today = DateTime.Now.Date;

			var employeeUserPartyTypes =
				await srvUser.GetEmployeeUserPartyTypeIdsAsync(userId, DepartementId);

            //if (!employeeUserPartyTypes.Any())
            //	throw new BusinessException(ExceptionMessage.ServiceNotFound);

            var serviceQuery = serviceScopeFactory.CreateScopedUow()
                .GetRepository<Service>()
                .GetAllQueryFiltered()
                .AsNoTracking()
                .Include(x => x.ServiceInitiatorPartyType)
                .Include(x => x.SystemModule)
                .Include(x => x.SystemModule!.Department)
                .Include(x => x.SystemModule!.SystemModuleType)
                .Where(c => c.Initialservice == true && c.SystemModule!.DepartmentId == DepartementId && c.SystemModule.SystemModuleType!.BackendName == ModuleType.EvaluationPlan);
				//.Where(c => c.ServiceInitiatorPartyType!
				//			   .Any(x => employeeUserPartyTypes.Contains(x.PartyTypeId)))
				//.Where(c => today >= c.StartDate &&
				//			(c.EndDate == null || c.EndDate.Value.AddDays(1) >= today));

			var service = await serviceQuery.FirstOrDefaultAsync();

			if (service == null)
				throw new BusinessException(ExceptionMessage.ServiceNotFound);

            var dto = new ServiceDTO();//   service.Adapt<ServiceDTO>();
            dto.Id = service.Id;
            dto.Name = service.NameAr;
            //dto.Id = service.;
            //dto.Id = service.Id;
			Guid? planId = null;
			var CheckActionCondition = true;

			var actions = await SrvActionStatusConfiguration.GetActionsByStatus(
				service.Id,
				statusId: null,
				requestId: null,
				planId: planId,
				lang: lang,
				CheckActionCondition
			);

			dto.Actions = actions;
			dto.Routing = service.SystemModule?.Routing;

			return dto;
			
		}
		public async Task<ServiceDTO> GetEvaluationPartyServiceDetailsAsync(Guid DepartementId,Guid serviceId ,string lang)
		{
			var userId = userInfo.UserId ?? Guid.Parse("C2536611-576B-4EB8-84F4-747F4ECE9A23");

			if (DepartementId == Guid.Empty)
				throw new ArgumentException("DepartementId ID cannot be null or empty.", nameof(DepartementId));

			//if (userInfo.UserId is null)
			//	throw new BusinessException(ExceptionMessage.UserInfoNotFound);

			//var userId = userInfo.UserId.Value;
			var today = DateTime.Now.Date;

			var employeeUserPartyTypes =
				await srvUser.GetEmployeeUserPartyTypeIdsAsync(userId, DepartementId);

			//if (!employeeUserPartyTypes.Any())
			//	throw new BusinessException(ExceptionMessage.ServiceNotFound);

			var serviceQuery = serviceScopeFactory.CreateScopedUow()
				.GetRepository<Service>()
				.GetAllQueryFiltered()
				.AsNoTracking()
				.Include(x => x.ServiceInitiatorPartyType)
				.Include(x => x.SystemModule)
				.Include(x => x.SystemModule!.Department)
				.Include(x => x.SystemModule!.SystemModuleType)
				.Where(c => c.Id == serviceId && c.SystemModule!.DepartmentId == DepartementId && c.SystemModule.SystemModuleType!.BackendName == ModuleType.EvaluationParty);
			//.Where(c => c.ServiceInitiatorPartyType!
			//			   .Any(x => employeeUserPartyTypes.Contains(x.PartyTypeId)))
			//.Where(c => today >= c.StartDate &&
			//			(c.EndDate == null || c.EndDate.Value.AddDays(1) >= today));

			var service = await serviceQuery.FirstOrDefaultAsync();

			if (service == null)
				throw new BusinessException(ExceptionMessage.ServiceNotFound);

			var dto = new ServiceDTO();//   service.Adapt<ServiceDTO>();
			dto.Id = service.Id;
			dto.Name = service.NameAr;
			//dto.Id = service.;
			//dto.Id = service.Id;
			Guid? planId = null;
			var CheckActionCondition = true;

			var actions = await SrvActionStatusConfiguration.GetActionsByStatus(
				service.Id,
				statusId: null,
				requestId: null,
				planId: planId,
				lang: lang,
				CheckActionCondition
			);

			dto.Actions = actions;
			dto.Routing = service.SystemModule?.Routing;

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
                                                   .Include(x => x.PartyType!.SystemModule)
                                                   .Where(x => x.PartyType!.SystemModule.Id == Module.Id && x.PartyType.IsEmployeePartyType)
                                                   .Select(x => x.PartyType!.Id)
                                                   .Distinct()
                                                   .ToListAsync();

                    if (employeeUserPartyTypes.Any())
                    {
                        serviceList = await Scoped.GetRepository<ServiceInitiatorPartyType>().GetAllQueryFiltered()
                                     .Where(c => employeeUserPartyTypes.Contains(c.PartyTypeId))
                                     .Where(c => c.service.SystemModuleId == Module.Id)
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
                                    .Where(c => !c.PartyType.IsEmployeePartyType)
                                    .Where(c => c.service.SystemModuleId == Module.Id)
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
      


        //private async Task<List<ScholarshipDataDTO>> GetEligibleScholarShipsAsync(Service service, string lang)
        //{
        //   using var scope = serviceScopeFactory.CreateScopedUow();
        //   using var scope1 = serviceScopeFactory.CreateScopedUow();

        //        var userTask = scope.GetRepository<MinistryUser>()
        //                            .GetAllQueryFiltered()?
        //                            .Include(x=>x.UserPartTypes)
        //                            .FirstAsync(c => c.Id == userInfo.UserId);

        //        var departmentUserIdsTask = scope1.GetRepository<PartyType>()
        //                                         .GetAllQueryFiltered(x => x.SystemModuleId == service.SystemModuleId)
        //                                         .AsNoTracking()
        //                                         .Select(x => x.Id)
        //                                         .ToListAsync();


        //        var allowedStatus = service!.SchServiceStatusConfiguration!
        //                                   .Select(c => c.CurrentStatusId)
        //                                   .ToList();

        //        var departmentUserIds = await departmentUserIdsTask;

        //        var partyTypeIdsList = userInfo.PartyTypes
        //                                            .Where(id => departmentUserIds.Contains(id));
        //        var canViewAllScholarships = false;

        //        if (partyTypeIdsList != null)
        //        {
        //            canViewAllScholarships = await scope1.GetRepository<UserPartyType>()
        //                                                 .GetAllQueryFiltered(x => partyTypeIdsList.Contains(x.PartyTypeId))
        //                                                 .AsNoTracking()
        //                                                 .Include(x => x.PartyType)
        //                                                 .AnyAsync(x => x.PartyType!.CanViewAllEvaluations);
        //        }
        //        var user = await userTask!;
        //        if (user is MinistryUser)
        //        {
        //            var Scholarships = await scope.GetRepository< ScholarshipData>()
        //                                          .GetAllQueryFiltered()
        //                                          .AsNoTracking()
        //                                          .Include(c => c.SchStatus)
        //                                          .Include(c=>c.SchAssignment)
        //                                          .Include(c => c.AcademicDegree)
        //                                          .Include(c => c.Major)
        //                                          .Include(c => c.StudentUser)
        //                                          .Where(c => allowedStatus.Contains(c.SchStatusId))
        //                                          .Where(c => canViewAllScholarships || 
        //                                                      c.SchAssignment!.Where(x=>
        //                                                          x.IsActive==true && x.IsDeleted==false
        //                                                          && user.UserPartTypes!.Select(p=>p.PartyTypeId)
        //                                                              .Contains(x.PartyTypeId)).Any(x => x.MinistryUserId == user.Id)
        //                                                      )
        //                                          .Select(c => new ScholarshipDataDTO
        //                                          {
        //                                              Id = c.Id,
        //                                              StudentQid = c.StudentUser!.QID,
        //                                              StudentName = lang == "ar" ? c.StudentUser!.FullNameAr : c.StudentUser!.FullNameEn,
        //                                              ScholarshipNumber = c.ScholarshipNumber,
        //                                              SchStatus = lang == "ar" ? c.SchStatus!.NameAr : c.SchStatus!.NameEn,
        //                                              SubDegreeName = lang == "ar" ? c.AcademicDegree!.NameAr : c.AcademicDegree!.NameEn,
        //                                              MajorName = lang == "ar" ? c.Major!.NameAr : c.Major!.NameEn,
        //                                          })
        //                                          .ToListAsync();

        //            return Scholarships;
        //        }

             
        //        else if (user is StudentUser)
        //        {
        //            var Scholarships = await scope.GetRepository<ScholarshipData>()
        //                                          .GetAllActiveNonDeleted()
        //                                          .AsNoTracking()
        //                                          .Include(c => c.SchStatus)
        //                                          .Include(c => c.AcademicDegree)
        //                                          .Include(c => c.Major)
        //                                           .Include(c => c.StudentUser)
        //                                          .Where(c => allowedStatus.Contains(c.SchStatusId))
        //                                          .Where(c => c.StudentUserId == user.Id)
        //                                          .Select(c => new ScholarshipDataDTO
        //                                          {
        //                                              Id = c.Id,
        //                                              StudentQid = c.StudentUser!.QID,
        //                                              StudentName = lang == "ar" ? c.StudentUser!.FullNameAr : c.StudentUser!.FullNameEn,
        //                                              ScholarshipNumber = c.ScholarshipNumber,
        //                                              SchStatus = lang == "ar" ? c.SchStatus!.NameAr : c.SchStatus!.NameEn,
        //                                              SubDegreeName = lang == "ar" ? c.AcademicDegree!.NameAr : c.AcademicDegree!.NameEn,
        //                                              MajorName = lang == "ar" ? c.Major!.NameAr : c.Major!.NameEn,
        //                                          })
        //                                          .ToListAsync();

        //            return Scholarships;
        //        }

        //        return new List<ScholarshipDataDTO>();
            
        //}

        public async Task<bool> CanCreateDraftAsync(Guid? serviceId, Guid? ownerId)
        {
            if (serviceId is null || ownerId is null)
                throw new ArgumentException("Service ID and Owner ID cannot be null or empty.");

            var service = await serviceScopeFactory.CreateScopedUow()
                                 .GetRepository<Service>().GetAllQueryFiltered()
                .FirstOrDefaultAsync(x => x.Id == serviceId);

            if (service == null)
                throw new BusinessException(ExceptionMessage.ServiceNotFound);

            var draftRequestsCount = await serviceScopeFactory.CreateScopedUow()
                                          .GetRepository<ServiceRequest>()
                                          .GetAllQueryFiltered()
                                          .Include(x => x.Status)
                                         .CountAsync(x => x.ServiceId == serviceId && x.StatusId == ownerId && x.Status.IsOpen && x.Status.IsInitial);

            if (draftRequestsCount > 0)
                return false;

            var draftActionsCount = await serviceScopeFactory.CreateScopedUow()
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

            var serviceSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(serviceObj.ServiceSettings);
            if (serviceSettings == null)
            {
                serviceSettings = new Dictionary<string, object>();
            }

            serviceSettings[settingKey] = value;

            serviceObj.ServiceSettings = JsonConvert.SerializeObject(serviceSettings);

             uow.GetRepository<Service>().Update(serviceObj);
        }
    }
}
