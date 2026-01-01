using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.Services.MappingProfiles;

public class EvalSubFormItemProfile : Profile
{
    public EvalSubFormItemProfile()
    {
        CreateMap<SubFormItem, EvaluationFormSubItemDto>();
    }
}

