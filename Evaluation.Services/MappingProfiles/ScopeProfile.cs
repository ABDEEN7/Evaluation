using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.SharedHelper.Models.Api;

namespace Evaluation.Services.MappingProfiles;

public class ScopeProfile : Profile
{
    public ScopeProfile()
    {
        CreateMap<Scope, ScopeDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.NameEn));
    }
}
