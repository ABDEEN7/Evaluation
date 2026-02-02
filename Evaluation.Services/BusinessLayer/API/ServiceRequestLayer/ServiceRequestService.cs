using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class ServiceRequestService(IServiceScopeFactory serviceScopeFactory,
    CacheDataProvider cacheDataProvider,
    UnitOfWork unitOfWork,
    LoggingServices loggingServices,
    IMapper mapper,
    UserInfo userInfo,
    IServiceProvider serviceProvider,
    RequestInfo requestInfo
    ) : ApiBase(serviceScopeFactory, cacheDataProvider, unitOfWork, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<ServiceRequest>> GetServiceRequestsByEvaluationRequestIds(List<Guid> Ids)
    {
        return await unitOfWork.GetRepository<ServiceRequest>()
            .GetAllActiveNonDeleted()
            .Where(x => Ids.Contains(x.EvaluationRequestId.Value))
            .Include(d => d.Status)
            .ToListAsync();
    }

    public async Task<ServiceRequest> UpdateEvaluationServiceRequest(ServiceRequest serviceRequest)
    {
        unitOfWork.GetRepository<ServiceRequest>().Update(serviceRequest);
        await uow.CommitAsync();

        return serviceRequest;
    }
    public async Task<ServiceRequest> GetEvaluationServiceRequestById(Guid Id)
    {
        return await unitOfWork.GetRepository<ServiceRequest>()
                    .GetByIDActiveNonDeleted(Id);
    }
    public async Task<ServiceRequest> DeleteEvaluationEvaluationRequestById(Guid id)
    {
        var evaluationRequest = await GetEvaluationServiceRequestById(id);
        if (evaluationRequest == null)
            throw new BusinessException(ConstantKeys.ExceptionMessage.RequestEvaluationNotExist);

        unitOfWork.GetRepository<ServiceRequest>().Delete(evaluationRequest);
        return evaluationRequest;
    }

}