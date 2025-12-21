using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API;

public class EvaluationRequestBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, EvaluationRequestService evaluationRequestService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<EvaluationRequestCalenderDto>> GetEvaluationRequestsForCalender()
    {
        var result = await evaluationRequestService.GetEvaluationRequests();
        return mapper.Map<List<EvaluationRequestCalenderDto>>(result);
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