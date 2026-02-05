using AutoMapper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class PartyTypeProfile : Profile
    {
        public PartyTypeProfile()
        {
            CreateMap<PartyType, PartyTypeDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.Department, opt => opt.MapFrom<PartyTypeSystemModuleResolver, Guid?>(src => src.DepartmentId))
                .ForMember(dest => dest.UserPartyType,
           opt => opt.MapFrom<UserPartyTypeResolver>());

        }

    }

    public class PartyTypeSystemModuleResolver : IMemberValueResolver<object, object, Guid?, string?>
    {
        private readonly UnitOfWork _uow;
        private readonly RequestInfo _requestInfo;

        public PartyTypeSystemModuleResolver(UnitOfWork uow, RequestInfo requestInfo)
        {
            _uow = uow;
            _requestInfo = requestInfo;
        }

        public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return "";
            var department = _uow.GetRepository<Department>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

            return _requestInfo.Lang == "ar" ? department?.NameAr ?? string.Empty : department?.NameEn ?? string.Empty;
        }
    }

 
    public class UserPartyTypeResolver
   : IValueResolver<PartyType, PartyTypeDTO, Guid[]?>
    {
        private readonly UnitOfWork _uow;

        public UserPartyTypeResolver(UnitOfWork uow)
        {
            _uow = uow;
        }

        public Guid[]? Resolve(
            PartyType source,
            PartyTypeDTO destination,
            Guid[]? destMember,
            ResolutionContext context)
        {
            return _uow.GetRepository<UserPartyType>()
                       .GetAllActiveNonDeleted()
                       .Where(x => x.PartyTypeId == source.Id)
                       .Select(x => x.UserId)
                       .ToArray(); // return array instead of List
        }
    }
}
