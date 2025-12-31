using AutoMapper;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;

namespace Evaluation.Services.MappingProfiles;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamDto>().
            ForMember(x => x.Name, opt => opt.MapFrom(src => src.NameEn)).
            ReverseMap();
        CreateMap<MinistryUser, MemberDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.NameEn))
        .ReverseMap();
    }
}