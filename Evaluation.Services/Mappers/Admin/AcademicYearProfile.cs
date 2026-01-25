using AutoMapper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;





namespace Evaluation.Services.Mappers.Admin
{
    public class AcademicYearProfile : Profile
    {
        public AcademicYearProfile()
        {
            CreateMap<AcademicYear, AcademicYearDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                 .ForMember(dest => dest.Department, opt => opt.MapFrom<DepartmentResolver, Guid?>(src => src.DepartmentId));
        }

    }
    public class DepartmentResolver : IMemberValueResolver<object, object, Guid?, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public DepartmentResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string Resolve(object source, object destination, Guid? sourceMember, string destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";

            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var user = scopedUow.GetRepository<Department>()
                                .GetAllNonDeleted()
                                .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? user?.NameAr ?? string.Empty : user?.NameEn ?? string.Empty;
        }
    }


}
