using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class EvaluationRequestService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<EvaluationRequest>> GetEvaluationRequests()
    {
        return unitOfWork.GetRepository<EvaluationRequest>()
                    .GetAllActiveNonDeleted()
                    .Include(d => d.Plan)
                    .Include(d => d.OrgTree)
                    .Include(d => d.DepEvaluationType)
                    .ToList();
    }

    public async Task<EvaluationRequest> GetEvaluationRequestById(Guid Id)
    {
        return await unitOfWork.GetRepository<EvaluationRequest>()
                    .GetByIDActiveNonDeleted(Id);
    }

    public async Task<EvaluationRequest> UpdateEvaluationRequest(EvaluationRequest evaluationRequest)
    {
        unitOfWork.GetRepository<EvaluationRequest>().Update(evaluationRequest);
        await uow.CommitAsync();

        return evaluationRequest;
    }

}
