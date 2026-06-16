using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Calendars;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.SharedHelper.Dtos.Form;
using Evaluation.SharedHelper.Dtos.Shared;
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
    public async Task<Result<List<ScopeTreeDto>>> GetScopeStructure()
    {
        return await _masterBl.GetApiService<FormBL>().GetScopeStructure();
    }


    [HttpGet]
    public async Task<Result<FormDto>> GetItems([FromQuery] Guid formId, [FromQuery] Guid academicYearId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormItems(formId, academicYearId);
    }

    [HttpGet]
    public async Task<Result<FormDto>> GetFormItemsWithValues([FromQuery] Guid formId, [FromQuery] Guid academicYearId, [FromQuery] Guid evaluationRequestId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormItemsWithValues(formId, academicYearId, evaluationRequestId);
    }

    [HttpPost]
    public async Task<Result<FormEvaluationDto>> SaveEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().SaveEvaluationForm(formEvaluation);
    }

    [HttpPost]
    public async Task<Result<FormEvaluationDto>> RenameFormItems([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().RenameFormItems(formEvaluation);
    }

    [HttpPost]
    public async Task<Result<FormEvaluationDto>> UpdateEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().UpdateEvaluationForm(formEvaluation);
    }


    [HttpGet]
    public async Task<Result<List<FormEvalMarixValueDto>>> GetFormEvalMarixValues([FromQuery] Guid formId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormEvalMarixValues(formId);
    }


    [HttpPost]
    public async Task<Result<ValidationResult>> ValidateEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().ValidateEvaluationForm(formEvaluation);
    }


    [HttpPost]
    public async Task<Result<CalculationFormResult>> CalculateEvaluationFormResult([FromBody] FormEvaluationDto formEvaluation)
    {

        return await _masterBl.GetApiService<FormBL>().CalculateFormResult(formEvaluation);

    }

}