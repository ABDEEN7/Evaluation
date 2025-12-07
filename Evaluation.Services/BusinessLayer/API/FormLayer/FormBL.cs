using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
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
        if (formEvaluationDto == null)
            return Result.Fail<FormEvaluationDto>("Form data is null.");

        var userId = userInfo.UserId;

        if (userId == null)
            return Result.Fail<FormEvaluationDto>("User ID is missing.");

        var form = mapper.Map<FormEvaluationValue>(formEvaluationDto);

        var evalForm = await formService.GetEvalForm(formEvaluationDto.Id);

        if (evalForm != null)
        {
            if (evalForm.HasOneValue)
            {
                foreach (var item in form.Items)
                {
                    var formItemsValue = await formService.GetFormItemValue(item.Id);
                    if (formItemsValue != null)
                    {
                        formItemsValue.Value = item.Value;
                        formItemsValue.Note = item.Note;
                    }
                    await formService.UpdateFormItemValue(formItemsValue);
                }

                foreach (var item in form.SubItems)
                {
                    var subFormItemsValue = await formService.GetSubFormItemValue(item.Id);
                    if (subFormItemsValue != null)
                    {
                        subFormItemsValue.FieldDropDownValueId = item.FieldDropDownValueId;
                        subFormItemsValue.Note = item.Note;
                    }
                    await formService.UpdateSubFormItemValue(subFormItemsValue);
                }
                await formService.SaveFormItemsAndSubs(form);
            }
        }
        else
        {
            foreach (var item in form.Items)
            {
                item.UserId = userId.Value;
            }

            foreach (var item in form.SubItems)
            {
                item.UserId = userId.Value;
            }

            await formService.SaveFormItemsAndSubs(form);
        }

        return Result.Ok(formEvaluationDto);
    }

    public async Task<Result<FormEvaluationDto>> UpdateEvaluationForm(FormEvaluationDto formEvaluationDto)
    {
        if (formEvaluationDto == null)
            return Result.Fail<FormEvaluationDto>("Form data is null.");

        var userId = userInfo.UserId;

        if (userId == null)
            return Result.Fail<FormEvaluationDto>("User ID is missing.");

        var form = mapper.Map<FormEvaluationValue>(formEvaluationDto);

        foreach (var item in form.Items)
        {
            var formItemsValue = await formService.GetFormItemValue(item.Id);
            if (formItemsValue != null)
            {
                formItemsValue.Value = item.Value;
                formItemsValue.Note = item.Note;
            }
            await formService.UpdateFormItemValue(formItemsValue);
        }

        foreach (var item in form.SubItems)
        {
            var subFormItemsValue = await formService.GetSubFormItemValue(item.Id);
            if (subFormItemsValue != null)
            {
                subFormItemsValue.FieldDropDownValueId = item.FieldDropDownValueId;
                subFormItemsValue.Note = item.Note;
            }
            await formService.UpdateSubFormItemValue(subFormItemsValue);
        }

        return Result.Ok(formEvaluationDto);
    }

    public async Task<Result<List<FormItemDto>>> GetForm(Guid FormId)
    {
        var formItems = await formService.GetFormItems();

        return mapper.Map<List<FormItemDto>>(formItems.Where(s => s.EvalFormId == FormId).ToList());
    }
}