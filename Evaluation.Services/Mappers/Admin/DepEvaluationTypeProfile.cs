using AutoMapper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Master;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class DepEvaluationTypeProfile : Profile
{
    public DepEvaluationTypeProfile()
    {
        CreateMap<DepEvaluationType, DepEvaluationTypeDto>();
    }
}
