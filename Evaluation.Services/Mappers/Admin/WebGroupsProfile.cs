using AutoMapper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
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
