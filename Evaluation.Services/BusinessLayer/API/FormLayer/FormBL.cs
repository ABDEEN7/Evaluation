using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.Form;
using Evaluation.SharedHelper.Dtos.Shared;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Evaluation.Services.BusinessLayer.API.FormLayer;

public class FormBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, FormService formService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<Result<List<FormItemDto>>> GetFormItems(Guid FormId)
    {
        var formItems = await formService.GetFormItems();
        var selectedFormItems = formItems.Where(s => s.EvalFormId == FormId).ToList();
        var mappedData = mapper.Map<List<FormItemDto>>(selectedFormItems);
        foreach (var item in selectedFormItems)
        {
            var relatedItemDtos = new List<RelatedItemDto>();

            foreach (var relatedFromItem in item.RelatedFrom) 
            {
                if (relatedFromItem.RelatedItemId != Guid.Empty)
                {
                    var formItemValue = await formService.GetFormItemValueByItemId(relatedFromItem.RelatedItemId);

                        relatedItemDtos.Add(new RelatedItemDto()
                        {
                            Id = relatedFromItem.RelatedItemId,
                            Note = formItemValue?.Note,
                            Value = formItemValue?.ActualValue?.ToString(),
                            Name = relatedFromItem.RelatedItem.NameAr
                        });
                }
            }
            mappedData.Where(md => md.Id == item.Id).FirstOrDefault().RelatedItems = relatedItemDtos;
        }

        return mappedData;
    }

    public async Task<Result<ValidationResult>> ValidateEvaluationForm(FormEvaluationDto formEvaluationDto)
    {
        return Validate(formEvaluationDto);
    }

    public static ValidationResult Validate(FormEvaluationDto dto)
    {
        var result = new ValidationResult();

        if (dto == null)
        {
            result.Errors.Add("FormEvaluationDto is null.");
            return result;
        }

        if (dto.Id == Guid.Empty)
            result.Errors.Add("Id must not be empty.");

        //TODO: Need to check before if required or not
        if (string.IsNullOrWhiteSpace(dto.Strengths))
            result.Errors.Add("Strengths is required.");

        //TODO: Need to check before if required or not
        if (string.IsNullOrWhiteSpace(dto.Improvements))
            result.Errors.Add("Improvements is required.");

        if (dto.Items == null || !dto.Items.Any())
        {
            result.Errors.Add("Items must contain at least one item.");
            return result;
        }

        for (int i = 0; i < dto.Items.Count; i++)
        {
            var item = dto.Items[i];

            if (item == null)
            {
                result.Errors.Add($"Items[{i}] is null.");
                continue;
            }

            if (item.Id == Guid.Empty)
                result.Errors.Add($"Items[{i}].Id must not be empty.");

            if (!item.ValueId.HasValue)
                result.Errors.Add($"Items[{i}].ValueId is required.");

            if (item.Value < 0)
                result.Errors.Add($"Items[{i}].Value must be greater than or equal to 0.");

            if (item.SubItems == null || !item.SubItems.Any())
            {
                result.Errors.Add($"Items[{i}].SubItems must contain at least one item.");
                continue;
            }

            for (int j = 0; j < item.SubItems.Count; j++)
            {
                var subItem = item.SubItems[j];

                if (subItem == null)
                {
                    result.Errors.Add($"Items[{i}].SubItems[{j}] is null.");
                    continue;
                }

                if (subItem.Id == Guid.Empty)
                    result.Errors.Add($"Items[{i}].SubItems[{j}].Id must not be empty.");

                if (!subItem.ValueId.HasValue)
                    result.Errors.Add($"Items[{i}].SubItems[{j}].ValueId is required.");

                if (subItem.Value == Guid.Empty)
                    result.Errors.Add($"Items[{i}].SubItems[{j}].Value must not be empty.");
            }
        }

        return result;
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
                        formItemsValue.ActualValue = item.ActualValue;
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
                formItemsValue.ActualValue = item.ActualValue;
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

    public async Task<Result<List<FormEvalMarixValueDto>>> GetFormEvalMarixValues(Guid FormId)
    {

        var evalForm = await formService.GetEvalForm(FormId);
        var formEvalMatrixValues = await formService.GetFormEvalMatrixValues(evalForm.FormEvalMatrixId.Value);

        return mapper.Map<List<FormEvalMarixValueDto>>(formEvalMatrixValues);
    }
}