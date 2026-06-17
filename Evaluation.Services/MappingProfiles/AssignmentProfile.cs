using AutoMapper;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.SharedHelper.Consts;
using Evaluation.SharedHelper.Dtos.OrgDto;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Models;

namespace Evaluation.Services.MappingProfiles;

public class AssignmentProfile : Profile
{
    public AssignmentProfile()
    {
        CreateMap<Team, TeamDto>().
            ForMember(x => x.Name, opt => opt.MapFrom(src => src.NameEn)).
            ReverseMap();

		CreateMap<MinistryUser, AssignmentDto>()
		.ForMember(x => x.Name,
			opt => opt.MapFrom<TeamResolver, Guid>(src => src.Id))
		.ForMember(x => x.JobTitle,
			opt => opt.MapFrom<JobTitleResolver, Guid>(src => src.Id))
		.ForMember(x => x.UserPartyTypes,
			opt => opt.MapFrom(src => src.UserPartTypes))
		.ForMember(x => x.ScopeIds,
			opt => opt.MapFrom(src =>
				src.UserTeams!
					.SelectMany(ut => ut.UserTeamScope)
					.Select(uts => uts.ScopeId)
					.Distinct()
			))
		.ReverseMap();
		CreateMap<UserPartyType, UserPartyTypeDto>()
            .ReverseMap();

        CreateMap<PartyType, PartyTypeDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom<PartyTypeResolver, Guid>(src => src.Id))
            .ReverseMap();
    }
}
public class PartyTypeResolver : IMemberValueResolver<PartyType, PartyTypeDto, Guid, string?>
{
    private readonly RequestInfo _requestInfo;

    public PartyTypeResolver(RequestInfo requestInfo)
    {
        _requestInfo = requestInfo;
    }

    public string? Resolve(
        PartyType source,
        PartyTypeDto destination,
        Guid sourceMember,
        string? destMember,
        ResolutionContext context)
    {
        return LanguageStatic.SelectLang(
            _requestInfo.Lang,
            source.NameAr,
            source.NameEn
        );
    }
}
public class TeamResolver : IMemberValueResolver<MinistryUser, AssignmentDto, Guid, string?>
{
    private readonly RequestInfo _requestInfo;

    public TeamResolver(RequestInfo requestInfo)
    {
        _requestInfo = requestInfo;
    }

    public string? Resolve(
        MinistryUser source,
        AssignmentDto destination,
        Guid sourceMember,
        string? destMember,
        ResolutionContext context)
    {
        return LanguageStatic.SelectLang(
            _requestInfo.Lang,
            source.NameAr,
            source.NameEn
        );
    }
}
public class JobTitleResolver : IMemberValueResolver<MinistryUser, AssignmentDto, Guid, string?>
{
    private readonly RequestInfo _requestInfo;

    public JobTitleResolver(RequestInfo requestInfo)
    {
        _requestInfo = requestInfo;
    }

    public string? Resolve(
        MinistryUser source,
        AssignmentDto destination,
        Guid sourceMember,
        string? destMember,
        ResolutionContext context)
    {
        return LanguageStatic.SelectLang(
            _requestInfo.Lang,
            source.JobTitleAr,
            source.JobTitleEn
        );
    }
}

