using AutoMapper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.SharedHelper.Models.Admin;


namespace Evaluation.Services.Mappers
{
    public class ControlValidationProfile : Profile
    {
        public ControlValidationProfile()
        {
            CreateMap<ControlValidation, ControlValidationDTO>()
                 .ForMember(dest => dest.ControlName, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest => dest.TabulatorConfig, opt => opt.MapFrom(src => src.TabulatorConfigJson))
                   .ForMember(dest => dest.FileCount, opt => opt.MapFrom(src => src.MaxFileCount))
                   .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.MaxFileSize))
                   .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.BackendName));
        }
    }
}
