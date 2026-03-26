using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

internal class EvaluationPartyProfile : Profile
{
    public EvaluationPartyProfile()
    {
        CreateMap<EvaluationParty, EvaluationPartyDto>();
    }
}
