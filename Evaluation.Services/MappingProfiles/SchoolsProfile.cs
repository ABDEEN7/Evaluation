using AutoMapper;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.Services.MappingProfiles;

public class SchoolsProfile : Profile
{
    public SchoolsProfile()
    {
     CreateMap<School, ResponseSchools>()
     .ForMember(dest => dest.SchoolTypeName, opt => opt.MapFrom<SchoolTypeResolver, Guid?>(src => src.TypeId))
     .ReverseMap();
    }
}
