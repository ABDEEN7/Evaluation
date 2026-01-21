using AutoMapper;
using Evaluation.DAL.Models.Template;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class NotificationTemplateProfile : Profile
    {
        public NotificationTemplateProfile()
        {
            CreateMap<NotificationTemplate, NotificationTemplateDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.SystemModule, opt => opt.MapFrom<PartyTypeSystemModuleResolver, Guid?>(src => src.SystemModuleId));

        }
     

    }
   
    
}
