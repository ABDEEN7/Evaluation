using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.Services.MappingProfiles;

public class FormItemConfigProfile : Profile
{
    public FormItemConfigProfile()
    {
        CreateMap<FormItemConfigDto, FormItemConfig>();
        CreateMap<FormItemConfig, FormItemConfigDto>();
        CreateMap<FormItemConfig, CreateFormItemConfigDto>();
    }
}
