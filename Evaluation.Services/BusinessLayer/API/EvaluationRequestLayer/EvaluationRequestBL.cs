using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.BusinessLayer.API.SchooLayer;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.EntityFrameworkCore;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

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

        var mappedEvaluationRequest = mapper.Map<List<EvaluationRequestCalenderDto>>(evaluationRequestResult);
        var mappedServiceRequest = mapper.Map<List<EvaluationRequestCalenderDto>>(serviceRequestResult);

        foreach (var item in mappedEvaluationRequest)
        {
            item.Url = $"https://localhost:7221/en/evaluationplan{requestInfo.DepRouting}?Evlid={item.Id}";
        }

        
        //TODO
        //foreach (var item in mappedServiceRequest)
        //{
        //    item.Url = $"https://localhost:7221/en/evaluationplan/{requestInfo.DepRouting}?Evlid={item.Id}";
        //}

        evaluationRequestCalenderDtos.AddRange(mappedEvaluationRequest);
        evaluationRequestCalenderDtos.AddRange(mappedServiceRequest);

        return evaluationRequestCalenderDtos;
    }
    public async Task<Result<EvaluationRequestCalenderDto>> UpdateEvaluationRequest(EvaluationRequestCalenderDto evaluationRequestCalenderDto)
    {
        if (evaluationRequestCalenderDto == null)
            return Result.Fail<EvaluationRequestCalenderDto>("evaluation request is null.");

        //var evaluationRequest = await evaluationRequestService.GetEvaluationRequestById(evaluationRequestCalenderDto.Id);

        //evaluationRequest.FromDate = DateTime.Parse(evaluationRequestCalenderDto.Start);
        //evaluationRequest.ToDate = DateTime.Parse(evaluationRequestCalenderDto.End).AddDays(-1);//remove extra day that added on show

        //await evaluationRequestService.UpdateEvaluationRequest(evaluationRequest);

        return evaluationRequestCalenderDto;
    }

}