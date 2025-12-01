using AutoMapper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.Extensions.DependencyInjection;





namespace Evaluation.Services.Mappers.Admin
{
    public class ActionConditionProfile : Profile
    {
        public ActionConditionProfile()
        {
            CreateMap<ActionCondition, ActionConditionDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                 .ForMember(dest => dest.Ref, opt => opt.MapFrom<ActionConditionRefResolver>());
        }

    }
    public class ActionConditionRefResolver : IValueResolver<ActionCondition, ActionConditionDTO, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RequestInfo _requestInfo;

        public ActionConditionRefResolver(IServiceProvider serviceProvider, RequestInfo requestInfo)
        {
            _serviceProvider = serviceProvider;
            _requestInfo = requestInfo;
        }

        public string Resolve(ActionCondition source, ActionConditionDTO destination, string destMember, ResolutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedUow = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var field = scopedUow.GetRepository<Field>()
                    .GetAllActiveNonDeleted()
                    .FirstOrDefault(x => x.Id == source.RefID);

            return field == null
                ? string.Empty
                : _requestInfo.Lang == "ar" ? field.TitleAr : field.TitleEn;
        }
    }


}
