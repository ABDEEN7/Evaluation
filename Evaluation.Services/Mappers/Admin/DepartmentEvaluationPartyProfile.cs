using AutoMapper;
using Evaluation.DAL.Models.Planing;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

internal class DepartmentEvaluationPartyProfile : Profile
{
    public DepartmentEvaluationPartyProfile()
    {
        CreateMap<DepartmentEvaluationParty, DepartmentEvaluationPartyDto>();
    }
}
