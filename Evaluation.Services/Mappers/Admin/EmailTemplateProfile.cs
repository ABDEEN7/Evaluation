using AutoMapper;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.DAL.Models.Template;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;




namespace Evaluation.Services.Mappers.Admin
{
    public class EmailTemplateProfile : Profile
    {
        public EmailTemplateProfile()
        {
            CreateMap<EmailTemplate, EmailTemplateDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
             .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.EmailProfile, opt => opt.MapFrom<EmailProfilesResolver, Guid?>(src => src.EmailProfileId))
                .ForMember(dest => dest.Service, opt => opt.MapFrom<ServiceResolver, Guid?>(src => src.ServiceId))
                .ForMember(dest => dest.FielsFromRequest, opt => opt.MapFrom(src => src.FielsFromRequest != null
     ? src.FielsFromRequest
    .Split(",", StringSplitOptions.RemoveEmptyEntries)
    .Select(g => Guid.Parse(g.Trim()))
    .ToArray()
     : null))
                     .ForMember(dest => dest.FielsFromEvaluation, opt => opt.MapFrom(src => src.FielsFromEvaluation != null
     ? src.FielsFromEvaluation
    .Split(",", StringSplitOptions.RemoveEmptyEntries)
    .Select(g => Guid.Parse(g.Trim()))
    .ToArray()
     : null));
        }

    }
    public class ServiceResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public ServiceResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";

            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var status = scopedUow.GetRepository<Service>()
                                  .GetAllNonDeleted()
                                  .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr : status?.NameEn;
        }
    }
    public class EmailProfilesResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public EmailProfilesResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<EmailProfile>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return status ?.BackendName??string.Empty;
        }
    }
    
}
