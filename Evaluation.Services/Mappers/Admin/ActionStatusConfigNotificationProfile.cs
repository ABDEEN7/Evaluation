using AutoMapper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;




namespace Evaluation.Services.Mappers.Admin
{
    public class ActionStatusConfigNotificationProfile : Profile
    {
        public ActionStatusConfigNotificationProfile()
        {
            CreateMap<ActionStatusConfigNotification, ActionStatusConfigNotificationDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
              .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                    .ForMember(dest => dest.PartyType,
        opt => opt.MapFrom<PartyTypeResolver, Guid?>(src => src.PartyTypeId))
                 .ForMember(dest => dest.EmailTemplate, opt => opt.MapFrom<EmailTemplateResolver, Guid?>(src => src.EmailTemplateId))
                 .ForMember(dest => dest.SMSTemplate, opt => opt.MapFrom<SMSTemplateResolver, Guid?>(src => src.SMSTemplateId))
                 .ForMember(dest => dest.NotificationTemplate, opt => opt.MapFrom<NotificationTemplateResolver, Guid?>(src => src.NotificationTemplateId));
        }

    }
    public class PartyTypeResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public PartyTypeResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<PartyType>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
        }
    }
    public class EmailTemplateResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public EmailTemplateResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {

            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<EmailTemplate>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.TitleAr : status?.TitleEn;
        }
    }

    public class SMSTemplateResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public SMSTemplateResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<SMSTemplate>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.TitleAr : status?.TitleEn;
        }
    }
    public class NotificationTemplateResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public NotificationTemplateResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<NotificationTemplate>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.SubjectAr : status?.SubjectEn;
        }
    }

   






}
