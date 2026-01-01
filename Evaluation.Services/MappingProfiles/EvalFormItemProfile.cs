using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.Services.MappingProfiles;

public class EvalFormItemProfile : Profile
{
    public EvalFormItemProfile()
    {
        CreateMap<FormItem, EvaluationFormItemDto>();
    }
}

