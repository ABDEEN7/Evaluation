using AutoMapper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class NdaStatusProfile : Profile
{
    public NdaStatusProfile()
    {
        CreateMap<NdaStatus, NdaStatusDto>();
        CreateMap<NdaStatusDepartment, NdaStatusDepartmentDto>();
    }
}
