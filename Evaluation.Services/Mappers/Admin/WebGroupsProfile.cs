using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Dtos.WebSiteDto;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class WebGroupsProfile : Profile
    {
        public WebGroupsProfile()
        {
            CreateMap<WebGroup, WebGroupsDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.DepWebGroup,
           opt => opt.MapFrom<DepWebGroupResolver>());

            CreateMap<WebGroup, WebGroupsDto>()
             .ForMember(dest => dest.BackendName, opt => opt.MapFrom(src => src.BackendName))
             .ForMember(dest => dest.RoutingPath, opt => opt.MapFrom(src => src.RoutingPath))
             .ForMember(dest => dest.Desc, opt => opt.MapFrom<LocalizedDescResolver>());

        }

    }

    public class LocalizedDescResolver : IValueResolver<WebGroup, WebGroupsDto, string>
    {
        public string Resolve(WebGroup src, WebGroupsDto dest, string destMember, ResolutionContext context)
        {
            var lang = context.Items["lang"]?.ToString();
            return lang == "ar" ? src.DescAr : src.DescEn;
        }
    }


    public class DepWebGroupResolver
   : IValueResolver<WebGroup, WebGroupsDTO, Guid[]?>
    {
        private readonly UnitOfWork _uow;

        public DepWebGroupResolver(UnitOfWork uow)
        {
            _uow = uow;
        }

        public Guid[]? Resolve(
            WebGroup source,
            WebGroupsDTO destination,
            Guid[]? destMember,
            ResolutionContext context)
        {
            return _uow.GetRepository<DepWebGroup>()
                       .GetAllActiveNonDeleted()
                       .Where(x => x.WebGroupId == source.Id)
                       .Select(x => x.DepartmentId)
                       .ToArray(); // return array instead of List
        }
    }
}
