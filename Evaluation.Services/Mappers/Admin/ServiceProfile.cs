using AutoMapper;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.SystemModulesEntities;
using Evaluation.DAL.UnitOfWork;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;



namespace Evaluation.Services.Mappers.Admin
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, ServiceDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.SystemModule, opt => opt.MapFrom<SystemModuleResolver, Guid?>(src => src.SystemModuleId))
                .ForMember(dest => dest.ServiceInitiatorPartyType,
           opt => opt.MapFrom<ServiceInitiatorPartyTypesResolver>())
                .ForMember(dest => dest.ServiceRequestShowPartyType,
           opt => opt.MapFrom<ServiceRequestShowPartyTypeResolver>());

        }

    }
    public class SystemModuleResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public SystemModuleResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<SystemModule>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
        }
    }
    public class UserProfileResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public UserProfileResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";

            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var user = scopedUow.GetRepository<MinistryUser>()
                                .GetAllNonDeleted()
                                .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? user?.NameAr ?? string.Empty : user?.NameEn ?? string.Empty;
        }
    }
    public class ServiceInitiatorPartyTypesResolver
    : IValueResolver<Service, ServiceDTO, Guid[]?>
    {
        private readonly UnitOfWork _uow;

        public ServiceInitiatorPartyTypesResolver(UnitOfWork uow)
        {
            _uow = uow;
        }

        public Guid[]? Resolve(
            Service source,
            ServiceDTO destination,
            Guid[]? destMember,
            ResolutionContext context)
        {
            return _uow.GetRepository<ServiceInitiatorPartyType>()
                       .GetAllActiveNonDeleted()
                       .Where(x => x.serviceId == source.Id)
                       .Select(x => x.PartyTypeId)
                       .ToArray(); // return array instead of List
        }
    }

    public class ServiceRequestShowPartyTypeResolver
   : IValueResolver<Service, ServiceDTO, Guid[]?>
    {
        private readonly UnitOfWork _uow;

        public ServiceRequestShowPartyTypeResolver(UnitOfWork uow)
        {
            _uow = uow;
        }

        public Guid[]? Resolve(
            Service source,
            ServiceDTO destination,
            Guid[]? destMember,
            ResolutionContext context)
        {
            return _uow.GetRepository<ServiceRequestShowPartyType>()
                       .GetAllActiveNonDeleted()
                       .Where(x => x.serviceId == source.Id)
                       .Select(x => x.PartyTypeId)
                       .ToArray(); // return array instead of List
        }
    }
}
