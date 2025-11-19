using AutoMapper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class ActionStatusConfigurationProfile : Profile
    {
        public ActionStatusConfigurationProfile()
        {
            CreateMap<ActionStatusConfiguration, ActionStatusConfigurationDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.ServiceAction,
        opt => opt.MapFrom<ServiceActionResolver, Guid?>(src => src.ServiceActionId))
                 .ForMember(dest => dest.CurrentStatus,
        opt => opt.MapFrom<ServiceStatusResolver, Guid?>(src => src.CurrentStatusId))
    .ForMember(dest => dest.NextStatus,
        opt => opt.MapFrom<ServiceStatusResolver, Guid?>(src => src.NextStatusId))
    .ForMember(dest => dest.NotificationCount,
        opt => opt.MapFrom<NotificationCountResolver, Guid>(src => src.Id));
        }

    }
    
   
    public class ServiceActionResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public ServiceActionResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<ServiceAction>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
        }
    }

    public class ServiceStatusResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public ServiceStatusResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<ServiceStatus>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
        }
    }
    public class NotificationCountResolver
    : IMemberValueResolver<ActionStatusConfiguration, object, Guid, int?>
    {
        private readonly UnitOfWork _uow;

        public NotificationCountResolver(UnitOfWork uow)
        {
            _uow = uow;
            
        }
        public int? Resolve(
    ActionStatusConfiguration source,
    object destination,
    Guid sourceMember,
    int? destMember,
    ResolutionContext context)
{
    var countList = _uow.GetRepository<ActionStatusConfigNotification>()
                        .GetAllNonDeleted()
                        .Where(x => x.ActionStatusConfigurationId == sourceMember);

    return countList.Count();
}

    }







}
