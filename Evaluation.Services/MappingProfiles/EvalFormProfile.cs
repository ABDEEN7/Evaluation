using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Models;

namespace Evaluation.Services.MappingProfiles;

public class EvalFormProfile : Profile
{
    public EvalFormProfile()
    {
        CreateMap<EvalForm, TemplateFormDto>()
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                 .ForMember(dest => dest.CreateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.CreateById))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.EvalFormType, opt => opt.MapFrom<EvalFormTypeResolver, Guid?>(src => src.EvalFormTypeId))
                .ForMember(dest => dest.FormEvalMatrix, opt => opt.MapFrom<FormEvalMatrixResolver, Guid?>(src => src.FormEvalMatrixId))
                .ForMember(dest => dest.EvaluationParty, opt => opt.MapFrom<EvaluationPartyResolver, Guid?>(src => src.EvaluationPartyId))
                
                 .ForMember(dest => dest.CalcMethod, opt => opt.MapFrom(src => src.CalcMethod.BackendName))
                  .ForMember(dest => dest.AllowRename, opt => opt.MapFrom(src => src.AllowRename))
                  .ForMember(dest => dest.HasMuliEvaluation, opt => opt.MapFrom(src => src.HasMuliEvaluation))
                  .ForMember(dest => dest.EvalCountOfColumnsValue, opt => opt.MapFrom(src => src.CountOfColumnsValue));

    }
}
public class EvalFormTypeResolver : IMemberValueResolver<object, object, Guid?, string?>
{
    private readonly UnitOfWork _uow;
    private readonly RequestInfo _requestInfo;

    public EvalFormTypeResolver(UnitOfWork uow, RequestInfo requestInfo)
    {
        _uow = uow;
        _requestInfo = requestInfo;
    }

    public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
    {
        if (!sourceMember.HasValue) return "";
        var status = _uow.GetRepository<EvalFormType>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

        return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
    }
}

public class FormEvalMatrixResolver : IMemberValueResolver<object, object, Guid?, string?>
{
    private readonly UnitOfWork _uow;
    private readonly RequestInfo _requestInfo;

    public FormEvalMatrixResolver(UnitOfWork uow, RequestInfo requestInfo)
    {
        _uow = uow;
        _requestInfo = requestInfo;
    }

    public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
    {
        if (!sourceMember.HasValue) return "";
        var status = _uow.GetRepository<FormEvalMatrix>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

        return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
    }
}
public class EvaluationPartyResolver : IMemberValueResolver<object, object, Guid?, string?>
{
    private readonly UnitOfWork _uow;
    private readonly RequestInfo _requestInfo;

    public EvaluationPartyResolver(UnitOfWork uow, RequestInfo requestInfo)
    {
        _uow = uow;
        _requestInfo = requestInfo;
    }

    public string? Resolve(object source, object destination, Guid? sourceMember, string? destMember, ResolutionContext context)
    {
        if (!sourceMember.HasValue) return "";
        var status = _uow.GetRepository<EvaluationParty>()
                         .GetAllNonDeleted()
                         .FirstOrDefault(x => x.Id == sourceMember);

        return _requestInfo.Lang == "ar" ? status?.NameAr ?? string.Empty : status?.NameEn ?? string.Empty;
    }
}



