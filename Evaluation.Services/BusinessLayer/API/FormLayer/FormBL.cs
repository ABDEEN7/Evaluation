using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.ScopeLayer;
using Evaluation.Services.Enums;
using Evaluation.Services.MappingProfiles;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Dtos.Form;
using Evaluation.SharedHelper.Dtos.Shared;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Server;
using ValidationResult = Evaluation.SharedHelper.Dtos.Shared.ValidationResult;

namespace Evaluation.Services.BusinessLayer.API.FormLayer;

public class FormBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, FormService formService, EvaluationRequestService _evaluationRequestService, ScopeRepostiory scopeRepostiory)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public async Task<Result<FormDto>> GetFormItems(
    Guid formId,
    Guid academicYearId)
    {
        var evalForm = await formService.GetEvalForm(
            formId,
            IncludeCalcMethod: true);

        var mappedEvalForm =
            mapper.Map<TemplateFormDto>(evalForm);

        var formItems =
            await formService.GetFormItems(formId);

        var formItemValues =
            await formService.GetFormItemValues(formId);

        var scopeAcademicYears =
            await scopeRepostiory
                .GetScopeAcademicYearListByAcademicYearId(
                    academicYearId);

        var tree = ScopeTreeBuilder.BuildTree(
            requestInfo.Lang,
            scopeAcademicYears,
            formItems,
            formItemValues
            );

        return new FormDto
        {
            EvalForm = mappedEvalForm,
            Tree = tree
        };
    }

    public async Task<Result<FormDto>> GetFormItemsWithValues(Guid FormId, Guid AcademicYearId, Guid EvaluationRequestId)
    {
        var lang = requestInfo.Lang;
        var evalForm = await formService.GetEvalForm(FormId, IncludeCalcMethod: true);
        var mappedEvalForm = mapper.Map<TemplateFormDto>(evalForm);

        var formItems = await formService.GetFormItems(FormId);
        var mappedData = mapper.Map<List<FormItemDto>>(formItems, opt => opt.Items["lang"] = lang);

        var formItemsValues = await formService.GetFormItemsValuesByEvaluationRequestId(EvaluationRequestId);

        // foreach (var item in formItems)
        // {
        //     var relatedItemDtos = new List<RelatedItemDto>();
        //
        //     foreach (var relatedFromItem in item.RelatedFrom)
        //     {
        //         if (relatedFromItem.RelatedItemId != Guid.Empty)
        //         {
        //             var formItemValue = await formService.GetFormItemValueByItemId(relatedFromItem.RelatedItemId);
        //
        //             relatedItemDtos.Add(new RelatedItemDto()
        //             {
        //                 Id = relatedFromItem.RelatedItemId,
        //                 Note = formItemValue?.Note,
        //                 Value = formItemValue?.ActualValue?.ToString(),
        //                 Name = lang == "ar" ? relatedFromItem.RelatedItem!.NameAr : relatedFromItem.RelatedItem!.NameEn
        //             });
        //         }
        //     }
        //     mappedData.Where(md => md.Id == item.Id).FirstOrDefault().RelatedItems = relatedItemDtos;
        // }

        //return new FormDto() { EvalForm = mappedEvalForm , Items = mappedData};


        List<ScopeAcademicYear> scopeAcademicYears = await scopeRepostiory.GetScopeAcademicYearListByAcademicYearId(AcademicYearId);

        var tree = ScopeTreeBuilder.BuildTree(requestInfo.Lang, scopeAcademicYears, formItems, formItemsValues);

        return new FormDto() { EvalForm = mappedEvalForm, Tree = tree };
    }

	public async Task<Result<FormDto>> GetFinalFormItemsWithValues(Guid evaluationRequestId)
	{
		var lang = requestInfo.Lang;

		var evaluationRequest = await uow.GetRepository<EvaluationRequest>()
			.GetAllQueryFiltered(x => x.Id == evaluationRequestId)
			.FirstOrDefaultAsync();

		if (evaluationRequest == null)
			throw new BusinessException(ConstantKeys.ExceptionMessage.InActiveData);

		var finalForm = await uow.GetRepository<EvalForm>()
			.GetAll(x => x.IsFinalEval == true)
			.FirstOrDefaultAsync();

		if (finalForm == null)
			throw new BusinessException("Final form doesn't exist!");

		var formId = finalForm.Id;
        var academicYearId = Guid.Parse("646471F1-3063-41D8-95C6-A43094850612");//evaluationRequest.AcademicYearId;

		var evalForm = await formService.GetEvalForm(formId, IncludeCalcMethod: true);
		var mappedEvalForm = mapper.Map<TemplateFormDto>(evalForm);

		var formItems = await formService.GetFormItems(formId);

		var formItemsValues = await formService
			.GetFormItemsValuesByEvaluationRequestId(evaluationRequestId);

		var mappedData = mapper.Map<List<FormItemDto>>(formItems, opt =>
		{
			opt.Items["lang"] = lang;
		});

		foreach (var item in formItems)
		{
			var relatedItemDtos = new List<RelatedItemDto>();

			foreach (var relatedFromItem in item.RelatedFrom)
			{
				if (relatedFromItem.RelatedItemId == Guid.Empty)
					continue;

				var formItemValue = formItemsValues
					.FirstOrDefault(x => x.FormItemId == relatedFromItem.RelatedItemId);

				relatedItemDtos.Add(new RelatedItemDto
				{
					Id = relatedFromItem.RelatedItemId,
					Note = formItemValue?.Note,
					Value = formItemValue?.ActualValue?.ToString(),
					Name = lang == "ar"
						? relatedFromItem.RelatedItem!.NameAr
						: relatedFromItem.RelatedItem!.NameEn
				});
			}

			var mappedItem = mappedData.FirstOrDefault(md => md.Id == item.Id);
			if (mappedItem != null)
				mappedItem.RelatedItems = relatedItemDtos;
		}

		var scopeAcademicYears = await scopeRepostiory
			.GetScopeAcademicYearListByAcademicYearId(academicYearId);

		var tree = ScopeTreeBuilder.BuildTree(
            requestInfo.Lang,
            scopeAcademicYears,
			formItems,
			formItemsValues);

		return new FormDto
		{
			EvalForm = mappedEvalForm,
			Tree = tree
		};
	}
	public async Task<Result<ValidationResult>> ValidateEvaluationForm(FormEvaluationDto formEvaluationDto)
    {
        return await Validate(formEvaluationDto);
    }


    private async Task<ValidationResult> Validate(FormEvaluationDto dto)
    {
        var result = new ValidationResult();

        if (dto == null || dto.Id == Guid.Empty)
            throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidJson);

        var formItems = await formService.GetFormItems(dto.Id);

        foreach (var item in dto.Items)
        {
            var formItem = formItems.Where(f => f.Id == item.Id).FirstOrDefault();//await formService.GetFormItem(item.Id);
            var evalForm = await formService.GetEvalForm(dto.Id);
            var formEvalMatrixValues = await formService.GetFormEvalMatrixValues(evalForm.FormEvalMatrixId);

            
            if (item.ValueId != null)
            {
                if(!formEvalMatrixValues.Any(x=>x.Id == item.ValueId))
                    result.Errors.Add(new ItemError() { ItemId = item.Id, Message = ConstantKeys.ExceptionMessage.TheSelectedValueIsNotRecognized, ItemPropertyType = ItemPropertyType.Select });
            }
            else 
            {
                result.Errors.Add(new ItemError() { ItemId = item.Id, Message = ConstantKeys.ExceptionMessage.Requiredfield, ItemPropertyType = ItemPropertyType.Select });
            }

            if (item.Note == null && formItem.HasNote)
            {
                result.Errors.Add(new ItemError() { ItemId = item.Id, Message = ConstantKeys.ExceptionMessage.Requiredfield, ItemPropertyType = ItemPropertyType.Note });

            }

            foreach (var sub in item.SubItems)
            {
                var currentSubItem = formItem?.SubFormItems?.Where(s => s.Id == sub.Id).FirstOrDefault();

                if (sub.Note == null && currentSubItem.HasNote)
                {
                    result.Errors.Add(new ItemError() { ItemId = sub.Id, Message = ConstantKeys.ExceptionMessage.Requiredfield, ItemPropertyType = ItemPropertyType.Note });
                }

                if (sub.ValueId != null)
                {
                    if (!currentSubItem?.DropDownType?.FieldDropDownValues?.Any(x => x.Id == sub.ValueId) != null)
                    {
                        if (!currentSubItem.DropDownType.FieldDropDownValues.Any(x => x.Id == sub.ValueId))
                            result.Errors.Add(new ItemError() { ItemId = sub.Id, Message = ConstantKeys.ExceptionMessage.TheSelectedValueIsNotRecognized, ItemPropertyType = ItemPropertyType.Select });
                    }
                    else
                    {
                        result.Errors.Add(new ItemError() { ItemId = sub.Id, Message = ConstantKeys.ExceptionMessage.TheSelectedValueIsNotRecognized, ItemPropertyType = ItemPropertyType.Select });
                    }

                }
                else
                {
                    result.Errors.Add(new ItemError() { ItemId = sub.Id, Message = ConstantKeys.ExceptionMessage.Requiredfield, ItemPropertyType = ItemPropertyType.Select });
                }
            }

        }

        return result;
    }

	public async Task<Result<FormEvaluationDto>> SaveEvaluationForm(FormEvaluationDto formEvaluationDto)
	{
		if (formEvaluationDto == null)
			return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.FormDataIsNull);

        var formValidation = await Validate(formEvaluationDto);
        if (!formValidation.IsValid)
            return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.FormDataIsNotValid);

        var userId = userInfo.UserId;
		if (userId == null)
			return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.UserNotFound);

		var evalForm = await formService.GetEvalForm(formEvaluationDto.Id);
		if (evalForm == null)
			return Result.Fail<FormEvaluationDto>("Eval form not found");

		if (evalForm.IsFinalEval &&
			(formEvaluationDto.EvaluationRequestId == null || formEvaluationDto.EvaluationRequestId == Guid.Empty))
		{
			throw new BusinessException(ConstantKeys.ExceptionMessage.InvalidJson);
		}
        var form = new FormEvaluationValue();

		try
        {
		 form = mapper.Map<FormEvaluationValue>(formEvaluationDto);

        }
        catch (Exception ex)
        {

            throw;
        }

		if (evalForm.HasOneValue)
		{
			foreach (var item in form.Items)
			{
				var formItemsValue = await formService.GetFormItemValue(item.Id);

				if (formItemsValue != null)
				{
					formItemsValue.ActualValue = item.ActualValue;
					formItemsValue.Note = item.Note;
					formItemsValue.EvaluationRequestId = formEvaluationDto.EvaluationRequestId;
					formItemsValue.ServiceRequestId = formEvaluationDto.ServiceRequestId;

					uow.GetRepository<FormItemValue>().Update(formItemsValue);
				}
				else
				{
					item.UserId = userId.Value;
					item.EvaluationRequestId = formEvaluationDto.EvaluationRequestId;
					item.ServiceRequestId = formEvaluationDto.ServiceRequestId;

					uow.GetRepository<FormItemValue>().Insert(item);
				}
			}
            if(form.SubItems !=null)
            {
				foreach (var item in form.SubItems)
				{
					var subFormItemsValue = await formService.GetSubFormItemValue(item.Id);

					if (subFormItemsValue != null)
					{
						subFormItemsValue.FieldDropDownValueId = item.FieldDropDownValueId;
						subFormItemsValue.Note = item.Note;

						uow.GetRepository<SubFormItemValue>().Update(subFormItemsValue);
					}
					else
					{
						item.UserId = userId.Value;

						uow.GetRepository<SubFormItemValue>().Insert(item);
					}
				}

			}

		}
		else
		{
			foreach (var item in form.Items)
			{
				item.UserId = userId.Value;
				item.EvaluationRequestId = formEvaluationDto.EvaluationRequestId;
				item.ServiceRequestId = formEvaluationDto.ServiceRequestId;
			}

			foreach (var item in form.SubItems)
			{
				item.UserId = userId.Value;
			}

			await formService.SaveFormItemsAndSubs(form);
		}

		if (evalForm.IsFinalEval)
		{
			var calculationResult = await CalculateFormResult(formEvaluationDto);

			var evaluationRequest =
				await _evaluationRequestService.GetEvaluationRequestByIdAsync(formEvaluationDto.EvaluationRequestId.Value);

			evaluationRequest.FormEvalMatrixValueId = calculationResult.Value.Id;
			evaluationRequest.EvalDays = calculationResult.Value.NextEvalDays;
			evaluationRequest.FollowUpDays = calculationResult.Value.NextFollowUpDays;
			evaluationRequest.EvaluationDate = DateOnly.FromDateTime(DateTime.Now);

            evaluationRequest.NextFollowUpDate =
                DateOnly.FromDateTime(DateTime.Now.AddDays(calculationResult.Value.NextFollowUpDays));

            evaluationRequest.NextEvaluationDate =
				DateOnly.FromDateTime(DateTime.Now.AddDays(calculationResult.Value.NextEvalDays));

			evaluationRequest.FinalEvalValue = calculationResult.Value.Value;

			uow.GetRepository<EvaluationRequest>().Update(evaluationRequest);
		}

		return Result.Ok(formEvaluationDto);
	}


	public async Task<Result<FormEvaluationDto>> UpdateEvaluationForm(FormEvaluationDto formEvaluationDto)
    {
        if (formEvaluationDto == null)
            return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.FormDataIsNull);

        var userId = userInfo.UserId;

        if (userId == null)
            return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.UserNotFound);

        var form = mapper.Map<FormEvaluationValue>(formEvaluationDto);

        foreach (var item in form.Items)
        {
            var formItemsValue = await formService.GetFormItemValue(item.Id);
            if (formItemsValue != null)
            {
                formItemsValue.ActualValue = item.ActualValue;
                formItemsValue.Note = item.Note;
            }
			uow.GetRepository<FormItemValue>().Update(formItemsValue);
		}

        foreach (var item in form.SubItems)
        {
            var subFormItemsValue = await formService.GetSubFormItemValue(item.Id);
            if (subFormItemsValue != null)
            {
                subFormItemsValue.FieldDropDownValueId = item.FieldDropDownValueId;
                subFormItemsValue.Note = item.Note;
            }
			 uow.GetRepository<SubFormItemValue>().Update(subFormItemsValue);
        }

        return Result.Ok(formEvaluationDto);
    }

    public async Task<Result<FormEvaluationDto>> RenameFormItems(FormEvaluationDto formEvaluationDto)
    {

        if (formEvaluationDto == null)
            return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.FormDataIsNull);

        var userId = userInfo.UserId;

        if (userId == null)
            return Result.Fail<FormEvaluationDto>(ConstantKeys.ExceptionMessage.UserNotFound);

        var form = mapper.Map<FormEvaluationValue>(formEvaluationDto);

        var evalForm = await formService.GetEvalForm(formEvaluationDto.Id);

        if (evalForm != null)
        {
            foreach (var item in form.Items)
            {
                item.UserId = userId.Value;
            }

            await formService.SaveFormItemsAndSubs(form);
        }

        return Result.Ok(formEvaluationDto);
    }


    public async Task<Result<List<FormItemDto>>> GetForm(Guid FormId)
    {
        var formItems = await formService.GetFormItems(FormId);

        return mapper.Map<List<FormItemDto>>(formItems.ToList());
    }

    public async Task<Result<List<FormEvalMarixValueDto>>> GetFormEvalMarixValues(Guid FormId)
    {

        var evalForm = await formService.GetEvalForm(FormId);
        var formEvalMatrixValues = await formService.GetFormEvalMatrixValues(evalForm.FormEvalMatrixId);

        return mapper.Map<List<FormEvalMarixValueDto>>(formEvalMatrixValues, opt => opt.Items["lang"] = requestInfo.Lang);
    }

    public async Task<Result<CalculationFormResult>> CalculateFormResult(FormEvaluationDto formEvaluationDto)
    {
        var evalForm = await formService.GetEvalForm(formEvaluationDto.Id, IncludeCalcMethod: true);
        var formEvalMatrixValues = await formService.GetFormEvalMatrixValues(evalForm.FormEvalMatrixId);

        CalculationFormResult result = new();

        switch (evalForm.CalcMethod.BackendName)
        {
            case CalcMethodsEnum.AVERAGE:

                decimal total = 0;
                foreach (var item in formEvaluationDto.Items)
                {
                    if (evalForm.HasMuliEvaluation)
                    {
                        total += (formEvalMatrixValues.Where(v => v.Id == item.ValueId).Select(v => v.ActualMatrixValue).FirstOrDefault() * (item.WeightPercentage / 100));
                    }
                    else
                    {
                        total += (formEvalMatrixValues.Where(v => v.Id == item.ValueId).Select(v => v.ActualMatrixValue).FirstOrDefault());
                    }
                }

                if (evalForm.HasMuliEvaluation)
                {
                    result.Value = Math.Round((total / (formEvaluationDto.Items.Count / evalForm.CountOfColumnsValue)), 2);
                }
                else
                {
                    result.Value = Math.Round((total / formEvaluationDto.Items.Count ), 2);
                }

                var evalMatrixValue = formEvalMatrixValues.Where(v => v.MinValue <= result.Value && v.MaxValue >= result.Value);


                bool isArabic = string.Equals(requestInfo.Lang, "ar", StringComparison.OrdinalIgnoreCase);

                result.Name = evalMatrixValue
                    .Select(v => isArabic ? v.NameAr : v.NameEn)
                    .FirstOrDefault();

                result.Id = evalMatrixValue.Select(v => v.Id).FirstOrDefault();

                result.NextEvalDays = evalMatrixValue.Select(v => v.NextEvalDays).FirstOrDefault();
                result.NextFollowUpDays = evalMatrixValue.Select(v => v.NextFollowUpDays).FirstOrDefault();

                break;
            case CalcMethodsEnum.SUM:
                break;
            case CalcMethodsEnum.WithoutCalc:
                break;     
        }

        return result;
    }

}