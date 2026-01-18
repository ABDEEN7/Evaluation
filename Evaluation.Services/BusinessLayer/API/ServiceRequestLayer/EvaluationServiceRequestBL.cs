using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class EvaluationServiceRequestBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, ServiceRequestService serviceRequestService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<Result<EvaluationRequestCalenderDto>> UpdateEvaluationServiceRequestRequest(EvaluationRequestCalenderDto evaluationRequestCalenderDto)
    {
        if (evaluationRequestCalenderDto == null)
            return Result.Fail<EvaluationRequestCalenderDto>("evaluation service request is null.");

        var evaluationServiceRequest = await serviceRequestService.GetEvaluationServiceRequestById(evaluationRequestCalenderDto.Id);

        evaluationServiceRequest.VisitDateFrom= DateTime.Parse(evaluationRequestCalenderDto.Start);
        evaluationServiceRequest.VisitDateTo = DateTime.Parse(evaluationRequestCalenderDto.End);

        await serviceRequestService.UpdateEvaluationServiceRequest(evaluationServiceRequest);

        return evaluationRequestCalenderDto;
    }

}