using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class EvaluationRequestBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, EvaluationRequestService evaluationRequestService, ServiceRequestService serviceRequestService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{

    public async Task<List<EvaluationRequestCalenderDto>> GetEvaluationRequestsForCalender(string[] monthes)
    {
        var evaluationRequestResult = await evaluationRequestService.GetEvaluationRequests(monthes);
        var serviceRequestResult = await serviceRequestService.GetServiceRequestsByEvaluationRequestIds(evaluationRequestResult.Select(er => er.Id).ToList());

        List<EvaluationRequestCalenderDto> evaluationRequestCalenderDtos = new List<EvaluationRequestCalenderDto>();

        evaluationRequestCalenderDtos.AddRange(mapper.Map<List<EvaluationRequestCalenderDto>>(evaluationRequestResult));
        evaluationRequestCalenderDtos.AddRange(mapper.Map<List<EvaluationRequestCalenderDto>>(serviceRequestResult));

        return evaluationRequestCalenderDtos;
    }
    public async Task<Result<EvaluationRequestCalenderDto>> UpdateEvaluationRequest(EvaluationRequestCalenderDto evaluationRequestCalenderDto)
    {
        if (evaluationRequestCalenderDto == null)
            return Result.Fail<EvaluationRequestCalenderDto>("evaluation request is null.");

        var evaluationRequest = await evaluationRequestService.GetEvaluationRequestById(evaluationRequestCalenderDto.Id);

        evaluationRequest.FromDate = DateTime.Parse(evaluationRequestCalenderDto.Start);
        evaluationRequest.ToDate = DateTime.Parse(evaluationRequestCalenderDto.End).AddDays(-1);//remove extra day that added on show

        await evaluationRequestService.UpdateEvaluationRequest(evaluationRequest);

        return evaluationRequestCalenderDto;
    }

}