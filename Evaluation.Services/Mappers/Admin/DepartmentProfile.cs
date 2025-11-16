using AutoMapper;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.SystemModulesEntities;
using Evaluation.DAL.UnitOfWork;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;



namespace Evaluation.Services.Mappers.Admin
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.TargetOrgTree, opt => opt.MapFrom<TargetOrgTreeResolver, Guid?>(src => src.TargetOrgTreeId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom<CategoryResolver, Guid?>(src => src.TargetOrgTreeId));

        }

    }
    public class TargetOrgTreeResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public TargetOrgTreeResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<OrgTree>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
        }
    }
    public class CategoryResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public CategoryResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var status = _uow.GetRepository<Category>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
        }
    }

}
