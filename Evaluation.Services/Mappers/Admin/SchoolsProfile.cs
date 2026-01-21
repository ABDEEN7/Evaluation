using AutoMapper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;



namespace Evaluation.Services.Mappers.Admin
{
    public class SchoolsProfile : Profile
    {
        public SchoolsProfile()
        {
            CreateMap<School, SchoolsDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                 .ForMember(dest => dest.OrgClass, opt => opt.MapFrom<OrgClassResolver, Guid?>(src => src.OrgClassId))
                   .ForMember(dest => dest.Type, opt => opt.MapFrom<SchoolTypeResolver, Guid?>(src => src.TypeId));

        }

    }

    public class OrgClassResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public OrgClassResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {


            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var user = scopedUow.GetRepository<OrgClass>()
                                .GetAllNonDeleted()
                                .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? user?.NameAr ?? string.Empty : user?.NameEn ?? string.Empty;
        }
    }

    public class SchoolTypeResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public SchoolTypeResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {


            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var user = scopedUow.GetRepository<SchoolType>()
                                .GetAllNonDeleted()
                                .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? user?.NameAr ?? string.Empty : user?.NameEn ?? string.Empty;
        }
    }

}
