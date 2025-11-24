using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.FormLayer;

public class FormBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, FormService formService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<Result<List<FormItemDto>>> GetFormItems(Guid FormId)
    {
        var formItems = await formService.GetFormItems();

        return mapper.Map<List<FormItemDto>>(formItems.Where(s => s.EvalFormId == FormId).ToList());
    }

    public async Task<Result<FormEvaluationDto>> SaveEvaluationForm(FormEvaluationDto formEvaluationDto)
    {
        //Mapping
        //Save Data
        return Result.Ok(formEvaluationDto);
    }
}