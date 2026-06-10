using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Mappers.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Models.Admin
{
    public class SrvServiceStatusConfigurationBL : AdminBase
    {
        public SrvServiceStatusConfigurationBL(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo) : base(serviceProvider, uow, loggingServices, mapper, userInfo, serviceScopeFactory, requestInfo)
        {

        }


        public async Task<List<ServiceStatusConfigurationDTO>> GetServiceStatusConfigurationList(int Page, int PageSize, Guid ServiceId,Guid systemModuleId)
        {
           var systemModuleBackendName=await GetBackendNameOfSystemModule(systemModuleId);
            var list = await uow.GetRepository<ServiceStatusConfiguration>()
                .GetAllNonDeleted()
                .Include(x => x.Service)
                .Include(x => x.CreateBy)
                .Where(x => x.ServiceId == ServiceId)
                .OrderByDescending(x => x.CreateDate)
                 .Skip(Page * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var result = mapper.Map<List<ServiceStatusConfigurationDTO>>(list, opts =>
            {
                opts.Items["Language"] = _requestInfo.Lang;
                opts.Items["SystemModuleBackendName"] = systemModuleBackendName;
            });

            return result;

        }

        public async Task<ServiceStatusConfigurationDTO> SaveServiceStatusConfiguration(ServiceStatusConfigurationDTO message)
        {


            var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == message.ServiceId && x.IsFreez == true).ToListAsync();

            if (servicefreezecount.Count > 0)
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_ADD);
            }

            ServiceStatusConfiguration obj = new ServiceStatusConfiguration();

            obj.ServiceId = message.ServiceId;
            obj.CurrentStatusId = message.CurrentStatusId;
            obj.NextStatusId = message.NextStatusId;
            obj.IsActive = message.IsActive;

            uow.GetRepository<ServiceStatusConfiguration>().Insert(obj);
            await uow.CommitAsync();
            var result = mapper.Map<ServiceStatusConfigurationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);

            result.ResponseStatus = DBResult.Inserted;
            return result;



        }
        public async Task<ServiceStatusConfigurationDTO> UpdateServiceStatusConfiguration(ServiceStatusConfigurationDTO message)
        {




            var result = new ServiceStatusConfigurationDTO();

            if (message.Id is not null)
            {


                ServiceStatusConfiguration obj = await uow.GetRepository<ServiceStatusConfiguration>()
                                      .GetAllNonDeleted()
                                      .Include(x => x.CreateBy)
                                      .Where(x => x.Id == message.Id)
                                      .FirstAsync();
                var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();

                if (servicefreezecount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_EDIT);
                }
                obj.ServiceId = message.ServiceId;
                obj.CurrentStatusId = message.CurrentStatusId;
                obj.NextStatusId = message.NextStatusId;
                obj.IsActive = message.IsActive;

                uow.GetRepository<ServiceStatusConfiguration>().Update(obj);
                await uow.CommitAsync();
                result = mapper.Map<ServiceStatusConfigurationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Updated;
            }

            return result;

        }

        public async Task<ServiceStatusConfigurationDTO> DeleteServiceStatusConfiguration(Guid? Id)
        {




            var result = new ServiceStatusConfigurationDTO();
            if (Id is not null)
            {
                ServiceStatusConfiguration obj = await uow.GetRepository<ServiceStatusConfiguration>()
                                      .GetAllNonDeleted()
                                      .Where(x => x.Id == Id)
                                      .FirstAsync();
                var servicefreezecount = await uow.GetRepository<Service>()
.GetAllNonDeleted()
                      .Where(x => x.Id == obj.ServiceId && x.IsFreez == true).ToListAsync();

                if (servicefreezecount.Count > 0)
                {
                    throw new BusinessException(ConstantKeys.ExceptionMessage.SERVICE_FREEZED_CANNOT_DELETE);
                }
                uow.GetRepository<ServiceStatusConfiguration>().Delete(obj);
                await uow.CommitAsync();
                result = mapper.Map<ServiceStatusConfigurationDTO>(obj, opts => opts.Items["Language"] = _requestInfo.Lang);
                result.ResponseStatus = DBResult.Deleted;
            }
            return result;

        }
        public async Task<List<ServiceStatuisDDLDto>> GetServiceStatuis(
      Guid systemModuleId,
      Guid departmentId)
        {
            string? backendName = await GetBackendNameOfSystemModule(systemModuleId);

            var isArabic = _requestInfo.Lang == LanguageConst.Ar;

            return backendName switch
            {
                ConstantKeys.ModuleType.EvaluationParty =>
                    await uow.GetRepository<ServiceStatus>()
                        .GetAllActiveNonDeleted(x =>
                            x.Service.SystemModule.BackendName == ConstantKeys.ModuleType.EvaluationRequest &&
                            x.Service.SystemModule.DepartmentId == departmentId &&
                            x.Service.Initialservice
                            )
                        .Select(x => new ServiceStatuisDDLDto
                        {
                            Id = x.Id,
                            Name = isArabic ? x.NameAr : x.NameEn
                        })
                        .ToListAsync(),

                _ =>
                    await uow.GetRepository<PlanStatus>()
                        .GetAllActiveNonDeleted()
                        .Select(x => new ServiceStatuisDDLDto
                        {
                            Id = x.Id,
                            Name = isArabic ? x.NameAr : x.NameEN
                        })
                        .ToListAsync()
            };
        }

        private async Task<string?> GetBackendNameOfSystemModule(Guid systemModuleId)
        {
            return await uow.GetRepository<SystemModule>()
                            .GetAllActiveNonDeleted(x => x.Id == systemModuleId)
                            .Select(x => x.BackendName)
                            .FirstOrDefaultAsync();
        }
    }
}
