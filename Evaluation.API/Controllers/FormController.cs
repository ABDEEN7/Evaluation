using Evaluation.API.ActionFilter;
using Evaluation.DAL.Dtos.Form;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.SharedHelper.Dtos.Form;
using Evaluation.SharedHelper.Dtos.Shared;
using Evaluation.SharedHelper.Enums;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]

public class FormController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public FormController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.GET_FORM_ITEMS)]
    public async Task<Result<FormDto>> GetItems([FromQuery] Guid formId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormItems(formId);
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.SAVE_EVALUATION_FORM)]
    public async Task<Result<FormEvaluationDto>> SaveEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().SaveEvaluationForm(formEvaluation);
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.RENAME_EVALUATION_FORM)]
    public async Task<Result<FormEvaluationDto>> RenameFormItems([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().RenameFormItems(formEvaluation);
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.UPDATE_EVALUATION_FORM)]
    public async Task<Result<FormEvaluationDto>> UpdateEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().UpdateEvaluationForm(formEvaluation);
    }


    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.SAVE_EVALUATION_FORM)]
    public async Task<Result<List<FormEvalMarixValueDto>>> GetFormEvalMarixValues([FromQuery] Guid formId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormEvalMarixValues(formId);
    }


    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VALIDATE_EVALUATION_FORM)]
    public async Task<Result<ValidationResult>> ValidateEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().ValidateEvaluationForm(formEvaluation);
    }


    [HttpPost]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.CALCULATE_EVALUATION_FORM)]
    public async Task<Result<CalculationFormResult>> CalculateEvaluationFormResult([FromBody] FormEvaluationDto formEvaluation)
    {

        return await _masterBl.GetApiService<FormBL>().CalculateFormResult(formEvaluation);

    }

}