using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.Org;

namespace Evaluation.Services.MappingProfiles;
public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(d => d.EmployeeNo, opt => opt.MapFrom(src => src.EmployeeNo))
            .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();
    }
}
