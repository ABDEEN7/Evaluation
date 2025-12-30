using AutoMapper;
using Azure.Core;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.EvaluationForm;

public class EvaluationFormBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, EvaluationFormService evaluationFormService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<List<EvaluationFormDto>> GetEvaluationForm(int Page)
    {
        var result = await evaluationFormService.GetEvaluationFormList(Page);
        return result;
    }

    public async Task<EvaluationFormDto> SaveEvaluationForm(EvaluationFormDto model)
    {
        var result = new EvaluationFormDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_EVALFORMS);
        if (validateObject)
        {
            result = await evaluationFormService.SaveEvaluationForm(model!);

        }
        return result;
    }
    public async Task<EvaluationFormDto> UpdateEvaluationForm(EvaluationFormDto model)
    {
        var result = new EvaluationFormDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_EVALFORMS);
        if (validateObject)
        {
            result = await evaluationFormService.UpdateEvaluationForm(model!);

        }
        return result;
    }
    public async Task<EvaluationFormDto> DeleteEvaluationForm(Guid Id)
    {
        var result = await evaluationFormService.DeleteEvaluationForm(Id!);
        return result;
    }

}