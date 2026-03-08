using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Evaluation.Services.Models.Admin
{
    public class AdminBase
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly UnitOfWork uow;
        protected readonly LoggingServices loggingServices;
        protected readonly IMapper mapper;
        protected readonly UserInfo userInfo;
        protected readonly IServiceScopeFactory serviceScopeFactory;
        protected readonly RequestInfo _requestInfo;

        public AdminBase(IServiceProvider serviceProvider, UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, IServiceScopeFactory serviceScopeFactory, RequestInfo requestInfo)
        {
            this.serviceProvider = serviceProvider;
            this.userInfo = userInfo;
            this.uow = uow;
            this.loggingServices = loggingServices;
            this.mapper = mapper;
            this.userInfo = userInfo;
            this.serviceScopeFactory = serviceScopeFactory;
            this._requestInfo = requestInfo;
        }


        protected async Task<IMapper> CreateMapperForAdmin<T, K>(Action<IMapperConfigurationExpression>? additionalMappings = null)
          where T : EntityBase where K : EntityBaseDTO
        {
            var mapperConfigService = serviceProvider.GetRequiredService<MapperConfigServices>();
            var mapperConfig = await mapperConfigService.GetMapperConfigAsync(); // Fetch the MapperConfig
            var userprofile = uow.GetRepository<MinistryUser>()
                  .GetAllActiveNonDeleted()
                  .Select(x => new
                  {
                      x.Id,
                      Name = _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
                  })
                  .ToDictionary(x => x.Id, x => x.Name);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, K>()
                    .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate.ToString(mapperConfig.DateFormat))) // Convert DateTime to string
                    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString(mapperConfig.DateFormat) : src.CreateDate.ToString(mapperConfig.DateFormat))) // Convert DateTime to string
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)) // Default to true if null
                    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)) // Default to false if null
                    .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => (src.CreateBy!=null?(_requestInfo.Lang == "ar" ? src.CreateBy.NameAr : src.CreateBy.NameEn):""))) // Mapping for Arabic name
                   .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateById.HasValue
                        ? (userprofile.ContainsKey(src.UpdateById!.Value) ? userprofile[src.UpdateById!.Value] : null)
                        : (userprofile.ContainsKey(src.CreateById) ? userprofile[src.CreateById] : null)));

                // Reverse mapping
                cfg.CreateMap<K, T>()
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.UpdateDate, opt => opt.Ignore()) // Ignore during reverse mapping
                    .ForMember(dest => dest.CreateDate, opt => opt.Ignore()) // Ignore during reverse mapping
                    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) // Ignore during reverse mapping
                    .ForMember(dest => dest.CreateById, opt => opt.Ignore()) // Ignore during reverse mapping
                    .ForMember(dest => dest.UpdateById, opt => opt.Ignore()) // Ignore during reverse mapping
                    .ForMember(dest => dest.CreateBy, opt => opt.Ignore()) // Ignore during reverse mapping
                    .ForMember(dest => dest.UpdateBy, opt => opt.Ignore()); // Ignore during reverse mapping
                //var map = cfg.CreateMap<T, K>();

                // Additional configurations can be applied here
                additionalMappings?.Invoke(cfg); // Invoke the additional mappings if provided
            });

            return config.CreateMapper();
        }


        protected async Task<IMapper> CreateMapperForAdminCustom<T, K>(
            List<(Expression<Func<K, object>> destinationMember, Action<IMemberConfigurationExpression<T, K, object>> memberOptions)>? additionalSourceMappings = null,
            List<(Expression<Func<T, object>> destinationMember, Action<IMemberConfigurationExpression<K, T, object>> memberOptions)>? additionalDestMappings = null
        ) where T : EntityBase where K : EntityBaseDTO
        {
            var mapperConfigService = serviceProvider.GetRequiredService<MapperConfigServices>();

            // Fetch the MapperConfig with error handling
            MapperConfig mapperConfig = await mapperConfigService.GetMapperConfigAsync();
            var userprofile = uow.GetRepository<MinistryUser>()
                  .GetAllActiveNonDeleted()
                  .Select(x => new
                  {
                      x.Id,
                      Name = _requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
                  })
                  .ToDictionary(x => x.Id, x => x.Name);

            var config = new MapperConfiguration(cfg =>
            {
                // Source mapping configuration
                var mapSrc = cfg.CreateMap<T, K>()
                    .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate.ToString(mapperConfig.DateFormat)))
                    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString(mapperConfig.DateFormat) : src.CreateDate.ToString(mapperConfig.DateFormat))) // Convert DateTime to string
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                    .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => (src.CreateBy!=null?(_requestInfo.Lang == "ar" ? src.CreateBy.NameAr : src.CreateBy.NameEn):"")))
                    .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdateById != null
                        ? (userprofile.ContainsKey(src.UpdateById.Value) ? userprofile[src.UpdateById.Value] : null)
                        : (userprofile.ContainsKey(src.CreateById) ? userprofile[src.CreateById] : null)));

                additionalSourceMappings?.ForEach((conf) => mapSrc.ForMember(conf.destinationMember, conf.memberOptions));

                // Reverse mapping configuration
                var mapDest = cfg.CreateMap<K, T>()
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                    .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                    .ForMember(dest => dest.CreateById, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdateById, opt => opt.Ignore())
                    .ForMember(dest => dest.CreateBy, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdateBy, opt => opt.Ignore());

                // Additional destination mapping configurations
                additionalDestMappings?.ForEach((conf) => mapDest.ForMember(conf.destinationMember, conf.memberOptions));
            });

            return config.CreateMapper();
        }


        public async Task<string> GenerateBackendName(string titleEn, Guid serviceId, string Type)
        {
            string cleanText = Regex.Replace(titleEn, "[^a-zA-Z]", "");
            bool isOnlyAlphabets = Regex.IsMatch(cleanText, @"^[A-Za-z]+$");
            if (isOnlyAlphabets)
            {
                var servicePrefix = await GetServicePrefix(serviceId);
                return $"{servicePrefix}_{Type}_{cleanText}";
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.TITLE_CANNOT_BE_UNICODE);
            }

        }
        public async Task<string> GenerateBackendNameBySystemModule(string titleEn, Guid SystemModuleId, string Type)
        {
            string cleanText = Regex.Replace(titleEn, "[^a-zA-Z]", "");
            bool isOnlyAlphabets = Regex.IsMatch(cleanText, @"^[A-Za-z]+$");
            if (isOnlyAlphabets)
            {
                var servicePrefix = await GetSystemModulePrefix(SystemModuleId);
                return $"{servicePrefix}_{Type}_{cleanText}";
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.TITLE_CANNOT_BE_UNICODE);
            }
        }
      
        public async Task<string> GenerateBackendNameByTitle(string title)
        {
            string cleanText = Regex.Replace(title, "[^a-zA-Z]", "");
            bool isOnlyAlphabets = Regex.IsMatch(cleanText, @"^[A-Za-z]+$");
            if (isOnlyAlphabets)
            {
                return await Task.Run(() =>
                {
                    return cleanText;
                });
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.TITLE_CANNOT_BE_UNICODE);
            }

        }
        private async Task<string> GetSystemModulePrefix(Guid SystemModuleId)
        {

            var SystemModule = await uow.GetRepository<SystemModule>()
                    .GetAll(x => x.Id == SystemModuleId)
                    .FirstOrDefaultAsync();
            if (SystemModule == null)
            {
                throw new ArgumentException("SystemModule not found with the provided ID.");
            }
            return $"{SystemModule.BackendName}";

        }
        private async Task<string> GetDepartmentPrefix(Guid DepartmentId)
        {

            var Department = await uow.GetRepository<Department>()
                    .GetAll(x => x.Id == DepartmentId)
                    .FirstOrDefaultAsync();
            if (Department == null)
            {
                throw new ArgumentException("Department not found with the provided ID.");
            }
            return $"{Department.BackendName}";

        }
        public async Task<string> GenerateBackendNameByDepartment(string titleEn, Guid DepartmentId, string Type)
        {
            string cleanText = Regex.Replace(titleEn, "[^a-zA-Z]", "");
            bool isOnlyAlphabets = Regex.IsMatch(cleanText, @"^[A-Za-z]+$");
            if (isOnlyAlphabets)
            {
                var servicePrefix = await GetDepartmentPrefix(DepartmentId);
                return $"{servicePrefix}_{Type}_{cleanText}";
            }
            else
            {
                throw new BusinessException(ConstantKeys.ExceptionMessage.TITLE_CANNOT_BE_UNICODE);
            }
        }

        private async Task<string> GetServicePrefix(Guid serviceId)
        {

            var service = await uow.GetRepository<Service>()
                .GetAll(x => x.Id == serviceId)
                .Include(x => x.SystemModule).FirstOrDefaultAsync();
            if (service == null)
            {
                throw new ArgumentException("Service not found with the provided ID.");
            }
            return $"{service.SystemModule?.BackendName}_{service.PrefixCode}";

        }

        public async Task<string> GetUiMessage(string key)
        {

            using (var newuow = serviceProvider.CreateScopedUow())
            {

                var result = await newuow.GetRepository<UiControl>()
                                   .GetAllNonDeleted()
                                   .Where(c => c.BackendName == key)
                                   .Select(c => _requestInfo.Lang == "ar" ? c.ValueAr : c.ValueEn)
                                   .FirstOrDefaultAsync();
                return result ?? string.Empty;
            }
        }

    }
}
