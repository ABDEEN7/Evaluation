using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.Services.MappingProfiles;

public class FormItemConfigProfile : Profile
{
    public FormItemConfigProfile()
    {
        CreateMap<FormItemConfig, FormItemConfigDto>()
            .ForMember(d => d.FormItemConfig_NameAr, opt => opt.MapFrom(src => src.NameAr))
         .ForMember(d => d.FormItemConfig_NameEn, opt => opt.MapFrom(src => src.NameEn))
         .ForMember(d => d.FormItemConfig_Percentage, opt => opt.MapFrom(src => src.WeightPercentage))
         .ReverseMap();
        CreateMap<FormItemConfig, CreateFormItemConfigDto>();
    }
}
