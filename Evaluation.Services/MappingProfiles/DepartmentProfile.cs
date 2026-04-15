using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.DepartementEntites;

namespace Evaluation.Services.MappingProfiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom<LocalizedNameResolver>())
            .ForMember(d => d.Desc, opt => opt.MapFrom<LocalizedDescResolver>())
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.DepIcon, opt => opt.MapFrom(src => src.DepIcon))
            //.ForMember(d => d.TypeId, opt => opt.MapFrom(src => src.CategoryId))
            //.ForMember(d => d.TypeName, opt => opt.MapFrom(src => src.Category.NameEn))
            .ForMember(d => d.RoutingPath, opt => opt.MapFrom(src => src.RoutingPath))
            .ForMember(d => d.ImgBlobUrl, opt => opt.MapFrom(src => src.WebsiteAttachment.BlobUrl))
            .ForMember(d => d.DepImage, opt => opt.MapFrom<LocalizedDepImageResolver>())
            .ReverseMap();
    }
}
public class LocalizedNameResolver : IValueResolver<Department, DepartmentDto, string>
{
    public string Resolve(Department src, DepartmentDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.NameAr : src.NameEn;
    }
}

public class LocalizedDescResolver : IValueResolver<Department, DepartmentDto, string>
{
    public string Resolve(Department src, DepartmentDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.DescAr : src.DescEn;
    }
}

public class LocalizedDepImageResolver : IValueResolver<Department, DepartmentDto, string>
{
    public string Resolve(Department src, DepartmentDto dest, string destMember, ResolutionContext context)
    {
        var lang = context.Items["lang"]?.ToString();
        return lang == "ar" ? src.DepImageBlobUrlAr : src.DepImageBlobUrlEn;
    }
}