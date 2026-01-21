using AutoMapper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;

namespace Evaluation.Services.MappingProfiles;

public class EvaluationRequestAssignmentProfile : Profile
{
    public EvaluationRequestAssignmentProfile()
    {
        CreateMap<EvalTeamRequestDto, EvaluationRequestAssignment>()
           .ForMember(dest => dest.MinistryUserId, opt => opt.MapFrom(src => src.UserId))
           .ForMember(dest => dest.EvalRequestAssignmentScopies, opt => opt.Ignore());

        CreateMap<EvaluationRequestAssignment, EvalTeamRequestDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.MinistryUserId));

        CreateMap<EvaluationRequestAssignment, EvaluationRequestAssignmentDto>()
            .ForMember(dest => dest.EvalRequestAssignmentScopies,
                       opt => opt.MapFrom(src => src.EvalRequestAssignmentScopies));

        CreateMap<EvalRequestAssignmentScope, EvalRequestAssignmentScopeDto>();

    }
}
