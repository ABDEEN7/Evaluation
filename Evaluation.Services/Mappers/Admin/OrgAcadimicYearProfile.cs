using AutoMapper;
using Evaluation.DAL.Models.Org;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class OrgAcadimicYearProfile : Profile
{
    public OrgAcadimicYearProfile()
    {
        CreateMap<OrgAcademicYear, OrgAcademicYearDTO>()
                .ForMember(d => d.OrgTree, opt => opt.MapFrom<OrgTreeResolver, Guid?>(src => src.OrgTreeId))
                .ForMember(d => d.ParentOrgTree, opt => opt.MapFrom<OrgTreeResolver, Guid?>(src => src.ParentOrgTreeId))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")));
    }
}
