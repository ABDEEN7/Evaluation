using AutoMapper;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class SMSTemplateProfile : Profile
    {
        public SMSTemplateProfile()
        {
            CreateMap<SMSTemplate, SMSTemplateDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.SMSProfile, opt => opt.MapFrom<SMSProfileResolver, Guid?>(src => src.SMSProfileId));

        }

    }
    public class SMSProfileResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public SMSProfileResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<SMSProfile>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return status ?.BackendName ?? string.Empty;
        }
    }
    
}
