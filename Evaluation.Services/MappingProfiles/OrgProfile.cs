using AutoMapper;

using Evaluation.DAL.Models.Org;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Dtos.OrgDto;

namespace Evaluation.Services.MappingProfiles;

public class OrgProfile : Profile
{
    public OrgProfile()
    {
        CreateMap<OrgTree, OrgDetailsDto>()
        .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(d => d.Name, opt => opt.MapFrom<LocalizedOrgNameResolver>());


        CreateMap<School, OrgDetailsDto>()
        .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(d => d.Name, opt => opt.MapFrom<LocalizedSchoolNameResolver>())
            .ReverseMap();

		CreateMap<Employee, OrgDetailsDto>()
	  .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
	  .ForMember(d => d.Name, opt => opt.MapFrom<LocalizedEmployeeNameResolver>())
	  //.ForMember(d => d.JobTitle,
		 // opt => opt.MapFrom<JobTitleResolver, Guid?>(src => src.JobTitleId))
	  //.ForMember(d => d.UserGender,
		 // opt => opt.MapFrom<UserGenderResolver, Guid?>(src => src.UserGenderId))
	  .ForMember(d => d.EmployeeNo, opt => opt.MapFrom(src => src.EmployeeNo))
	  .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
	  .ForMember(d => d.QID, opt => opt.MapFrom(src => src.QID))
	  .ForMember(d => d.NationalityCode, opt => opt.MapFrom(src => src.NationalityCode))
	  .ForMember(d => d.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
	  .ForMember(d => d.JoinDate, opt => opt.MapFrom(src => src.JoinDate.ToString("yyyy-MM-dd")))
	  .ReverseMap();

		CreateMap<Organization, OrgDetailsDto>()
        .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(d => d.Name, opt => opt.MapFrom<LocalizedOrganizationNameResolver>())
            .ReverseMap();
    }
}
public class LocalizedOrgNameResolver : IValueResolver<OrgTree, OrgDetailsDto, string>
{
    public string Resolve(OrgTree src, OrgDetailsDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.NameAr : src.NameEn;
    }
}

public class LocalizedSchoolNameResolver : IValueResolver<School, OrgDetailsDto, string>
{
    public string Resolve(School src, OrgDetailsDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.NameAr : src.NameEn;
    }
}

public class LocalizedEmployeeNameResolver : IValueResolver<Employee, OrgDetailsDto, string>
{
    public string Resolve(Employee src, OrgDetailsDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.NameAr : src.NameEn;
    }
}

public class LocalizedOrganizationNameResolver : IValueResolver<Organization, OrgDetailsDto, string>
{
    public string Resolve(Organization src, OrgDetailsDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.NameAr : src.NameEn;
    }
}