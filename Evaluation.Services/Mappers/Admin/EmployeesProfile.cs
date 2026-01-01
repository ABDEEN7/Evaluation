using AutoMapper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Website;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;



namespace Evaluation.Services.Mappers.Admin
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeesDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                 .ForMember(dest => dest.OrgClass, opt => opt.MapFrom<OrgClassResolver, Guid?>(src => src.OrgClassId))
                   .ForMember(dest => dest.UserGender, opt => opt.MapFrom<UserGenderResolver, Guid?>(src => src.UserGenderId))
                    .ForMember(dest => dest.JobTitle, opt => opt.MapFrom<JobTitleResolver, Guid?>(src => src.JobTitleId));

        }

    }

    public class JobTitleResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public JobTitleResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {


            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var user = scopedUow.GetRepository<JobTitle>()
                                .GetAllNonDeleted()
                                .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? user?.NameAr ?? string.Empty : user?.NameEn ?? string.Empty;
        }
    }

    public class UserGenderResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public UserGenderResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string ?destMember, ResolutionContext context)
        {


            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var user = scopedUow.GetRepository<UserGender>()
                                .GetAllNonDeleted()
                                .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? user?.NameAr ?? string.Empty : user?.NameEn ?? string.Empty;
        }
    }

}
