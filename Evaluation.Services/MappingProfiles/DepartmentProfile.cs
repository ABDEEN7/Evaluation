using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.Services.MappingProfiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(d => d.Desc, opt => opt.MapFrom(src => src.DescEn))
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.DepIcon, opt => opt.MapFrom(src => src.DepIcon))
            .ForMember(d => d.TypeId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(d => d.TypeName, opt => opt.MapFrom(src => src.Category.NameEn))
            .ReverseMap();
    }
}
