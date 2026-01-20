using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Dtos.OrgDto;

namespace Evaluation.Services.MappingProfiles;

public class OrgProfile : Profile
{
    public OrgProfile()
    {
        CreateMap<School, OrgDetailsDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ReverseMap();

        CreateMap<Employee, OrgDetailsDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(d => d.JobTitle, opt => opt.MapFrom<JobTitleResolver, Guid?>(src => src.JobTitleId))
            .ForMember(d => d.UserGender, opt => opt.MapFrom<UserGenderResolver, Guid?>(src => src.UserGenderId))
            .ReverseMap();

        CreateMap<Organization, OrgDetailsDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ReverseMap();
    }
}