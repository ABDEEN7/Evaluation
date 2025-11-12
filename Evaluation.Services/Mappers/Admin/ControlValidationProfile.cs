using AutoMapper;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.SharedHelper.Models.Admin;


namespace Evaluation.Services.Mappers
{
    public class ControlValidationProfile : Profile
    {
        public ControlValidationProfile()
        {
            CreateMap<ControlValidation, ControlValidationDTO>()
                 .ForMember(dest => dest.ControlName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
