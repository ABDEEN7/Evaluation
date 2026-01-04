using AutoMapper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;

namespace Evaluation.Services.MappingProfiles;

public class EvaluationRequestAssignmentProfile : Profile
{
    public EvaluationRequestAssignmentProfile()
    {
        CreateMap<EvaluationRequestAssignment, EvalTeamRequestDto>();
    }
}
