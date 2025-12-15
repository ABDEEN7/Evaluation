using AutoMapper;
using Evaluation.DAL.Models.Org;
using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.Services.MappingProfiles;

public class SchoolPlanProfile: Profile
{
    public SchoolPlanProfile()
    {
        CreateMap<SchoolLevel, SchoolLevelDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.EducationLevel.NameEn))
                .ForMember(d => d.BackendName, opt => opt.MapFrom(src => src.EducationLevel.BackendName))
                .ForMember(d => d.SchoolId, opt => opt.MapFrom(src => src.SchoolId));

        // Map School → ResponseSchools
        CreateMap<School, ResponseSchoolsPlans>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(d => d.SchoolLevel, opt => opt.MapFrom(src => src.SchoolLevel))
            .ReverseMap();
    }
}
