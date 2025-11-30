using AutoMapper;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class NavbarProfile : Profile
    {
        public NavbarProfile()
        {
            CreateMap<Navbar, NavbarDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue? src.UpdateById: src.CreateById))
              .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.Parent, opt => opt.MapFrom<NavbarResolver, Guid?>(src => src.ParentId))
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom<PermissionBackendNameResolver, Guid?>(src => src.PermissionId));

        }

    }

    public class NavbarResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public NavbarResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<Navbar>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.TitleAr : status?.TitleEn;
        }
    }
    public class PermissionBackendNameResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public PermissionBackendNameResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<Permission>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return status?.BackendName;
        }
    }
   

}
