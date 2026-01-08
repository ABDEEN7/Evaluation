using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
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

    public async Task<List<EvaluationFormItemDto> >GetAllEvalFormItems(Guid EvalformId)
    {
        var result = await evaluationFormService.GetEvaluationFormItemList(EvalformId);
        return result;
    }
    public async Task<List<DropdownItem>> GetAllFormItemsFromDepartment(Guid EvalformId)
    {
        var result = await evaluationFormService.GetAllFormItemsFromDepartment(EvalformId);
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
    public async Task<EvaluationFormItemDto> SaveEvaluationFormItem(EvaluationFormItemDto model)
    {
        var result = new EvaluationFormItemDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_FORMITEMS);
        if (validateObject)
        {
            result = await evaluationFormService.SaveEvaluationFormItem(model!);

        }
        return result;
    }
    public async Task<EvaluationFormItemDto> UpdateEvaluationFormItem(EvaluationFormItemDto model)
    {
        var result = new EvaluationFormItemDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_FORMITEMS);
        if (validateObject)
        {
            result = await evaluationFormService.UpdateEvaluationFormItem(model!);

        }
        return result;
    }
    public async Task<EvaluationFormItemDto> DeleteEvaluationFormItem(Guid Id)
    {
        var result = await evaluationFormService.DeleteEvaluationFormItem(Id!);
        return result;
    }

    public async Task<EvaluationFormSubItemDto> SaveEvaluationSubFormItem(EvaluationFormSubItemDto model)
    {
        var result = new EvaluationFormSubItemDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_SUBFORMITEMS);
        if (validateObject)
        {
            result = await evaluationFormService.SaveEvaluationSubFormItem(model!);

        }
        return result;
    }
    public async Task<EvaluationFormSubItemDto> UpdateEvaluationSubFormItem(EvaluationFormSubItemDto model)
    {
        var result = new EvaluationFormSubItemDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_SUBFORMITEMS);
        if (validateObject)
        {
            result = await evaluationFormService.UpdateEvaluationSubFormItem(model!);

        }
        return result;
    }
    public async Task<EvaluationFormSubItemDto> DeleteEvaluationSubFormItem(Guid Id)
    {
        var result = await evaluationFormService.DeleteEvaluationSubFormItem(Id!);
        return result;
    }

}