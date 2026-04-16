using AutoMapper;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Models.Admin;



namespace Scholarship.Services.Mappers.Admin
{
    public class ServiceStatusConfigurationProfile : Profile
    {
        public ServiceStatusConfigurationProfile()
        {
            CreateMap<ServiceStatusConfiguration, ServiceStatusConfigurationDTO>()
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom<ServiceStatusResolver, Guid?>(src => src.CurrentStatusId))
                .ForMember(dest => dest.NextStatus, opt => opt.MapFrom<ServiceStatusResolver, Guid?>(src => src.NextStatusId));

        }

    }
   
    
}
