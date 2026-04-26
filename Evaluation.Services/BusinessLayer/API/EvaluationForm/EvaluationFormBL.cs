using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.BusinessLayer.API.EvaluationForm;

public class EvaluationFormBL(IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider,
        UnitOfWork uow, LoggingServices loggingServices, IMapper mapper, UserInfo userInfo,
        IServiceProvider serviceProvider, RequestInfo requestInfo, EvaluationFormService evaluationFormService)
        : ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
{
    public List<TemplateFormDto> GetEvaluationForm(SearchTemplateForm pagination)
    {
        var result = evaluationFormService.GetEvaluationFormList(pagination);
        return result;
    }

    public async Task<List<EvaluationFormItemDto>> GetAllTemplateFormItems(Guid EvalformId)
    {
        var result = await evaluationFormService.GetEvaluationFormItemList(EvalformId);
        return result;
    }

    public async Task<List<FormScopeDTO>> GetAllFormScope(Guid formIdValue)
    {
        var result = await evaluationFormService.GetAllFormScopeList(formIdValue);
        return result;
    }
    public async Task<List<DropdownItem>> GetAllFormItemsFromDepartment(Guid EvalformId)
    {
        var result = await evaluationFormService.GetAllFormItemsFromDepartment(EvalformId);
        return result;
    }

    public async Task<TemplateFormDto> SaveEvaluationForm(TemplateFormDto model)
    {
        var result = new TemplateFormDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_EVALFORMS);
        if (validateObject)
        {
            result = await evaluationFormService.SaveEvaluationForm(model!);

        }
        return result;
    }
    public async Task<TemplateFormDto> UpdateEvaluationForm(TemplateFormDto model)
    {
        var result = new TemplateFormDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_EVALFORMS);
        if (validateObject)
        {
            result = await evaluationFormService.UpdateEvaluationForm(model!);

        }
        return result;
    }
    public async Task<TemplateFormDto> DeleteEvaluationForm(Guid Id)
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

    public async Task<FormScopeDTO> SaveFormScope(FormScopeDTO model)
    {
        var result = new FormScopeDTO();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_FORMSCOPES);
        if (validateObject)
        {
            result = await evaluationFormService.SaveFormScope(model!);

        }
        return result;
    }
    public async Task<FormScopeDTO> UpdateFormScope(FormScopeDTO model)
    {
        var result = new FormScopeDTO();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_FORMSCOPES);
        if (validateObject)
        {
            result = await evaluationFormService.UpdateFormScope(model!);

        }
        return result;
    }
    public async Task<FormScopeDTO> DeleteFormScope(Guid Id)
    {
        var result = await evaluationFormService.DeleteFormScope(Id!);
        return result;
    }
    public async Task<IReadOnlyList<DropdownItem>> GetPartyTypeListAsync()
    {
        return await uow.GetRepository<PartyType>()
            .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId)
            .Select(x => new DropdownItem
            {
                Id = x.Id,
                Name = requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<DropdownItem>> GetFormItemListAsync(Guid formId)
    {
        return await uow.GetRepository<FormItem>()
            .GetAllActiveNonDeleted(x => x.EvalFormId == formId)
            .Select(x => new DropdownItem
            {
                Id = x.Id,
                Name = requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
            })
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<IReadOnlyList<DropdownItem>> GetCalcMethodsListAsync()
    {
        return await uow.GetRepository<CalcMethod>()
            .GetAllActiveNonDeleted(x => x.DepartmentId == requestInfo.DepId)
            .Select(x => new DropdownItem
            {
                Id = x.Id,
                Name = requestInfo.Lang == "ar" ? x.NameAr : x.NameEn
            })
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<FormItemConfigDto>> GetAllFormItemConfig(Guid? evalFormId)
    {
        var result = await evaluationFormService.GetAllFormItemConfigAsync(evalFormId);
        return result;
    }
    public async Task<List<FormItemConfigDto>> SaveFormItemConfig(List<FormItemConfigDto> model)
    {
        var result = new List<FormItemConfigDto>();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_FORMITEMCONFIG);
        if (validateObject)
        {
            result = await evaluationFormService.SaveFormItemConfig(model);

        }
        return result;
    }
    public async Task<FormItemConfigDto> UpdateFormItemConfig(FormItemConfigDto model)
    {
        var result = new FormItemConfigDto();
        bool validateObject = await ValidateObject(model!, ConstantKeys.WebPermissions.ADD_WEB_FORMITEMCONFIG);
        if (validateObject)
        {
            result = await evaluationFormService.UpdateFormItemConfig(model);
        }
        return result;
    }
}