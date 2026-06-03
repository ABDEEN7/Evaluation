using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using FluentResults;
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

        var mappedEvaluationRequest = mapper.Map<List<EvaluationRequestCalenderDto>>(evaluationRequestResult);
        var mappedServiceRequest = mapper.Map<List<EvaluationRequestCalenderDto>>(serviceRequestResult);

        foreach (var item in mappedEvaluationRequest)
        {
            item.Url = $"/{requestInfo.Lang}/evaluationplan{requestInfo.DepRouting}?Evlid={item.Id}";
        }

        
        //TODO: waiting until complete the routing url
        //foreach (var item in mappedServiceRequest)
        //{
        //    item.Url = $"https://localhost:7221/en/evaluationplan/{requestInfo.DepRouting}?Evlid={item.Id}";
        //}

        evaluationRequestCalenderDtos.AddRange(mappedEvaluationRequest);
        evaluationRequestCalenderDtos.AddRange(mappedServiceRequest);

        return evaluationRequestCalenderDtos;
    }

    public async Task<WebAppEvaluationRequestsDTO> GetEvaluationRequestsByOrgTreeId(Guid orgTreeId, FilterRequestsDTO model)
    {
        var result = new WebAppEvaluationRequestsDTO();

        var evaluationRequestResult = await evaluationRequestService.GetEvaluationRequestsByOrgTreeId(orgTreeId, model);

		result.Data = evaluationRequestResult
	                .Select(x => new EvaluationRequestDTO
	                {
		                Id = x.Id,
		                RequestNumber = x.RequestNumber,
		                Status = requestInfo.Lang == "ar"? x.ServiceStatus!.NameAr: x.ServiceStatus!.NameEn,
						FromDate = DateOnly.FromDateTime(x.FromDate),
						ToDate = DateOnly.FromDateTime(x.ToDate),
						EvaluationDate = x.EvaluationDate,
		                NextEvaluationDate = x.NextEvaluationDate,
		                EvaluationResult = requestInfo.Lang == "ar" ? x.FormEvalMatrixValue?.NameAr : x.FormEvalMatrixValue?.NameEn
					})
	                .ToList();
		result.TotalDataCount = evaluationRequestResult.Count;
        Int32.TryParse(await cacheDataProvider.GetSystemSettingValue(ConstantKeys.WebAppSettings.PAGE_SIZE_FOR_SCHOOL_EVALUATION_REQUESTS), out int recordsPerPage);
        var pageSize = recordsPerPage;
        result.PageNumber = model.PageNumber.Value;
        result.PageSize = pageSize;
        result.IsRemainingData = result.Data.Count >= result.PageSize;

        return result;
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