using AutoMapper;
using Evaluation.API.ActionFilter;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormsModules;
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
    public async Task<Result<List<FormItemDto>>> GetItems([FromQuery] Guid formId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormItems(formId);
    }

    [HttpPost]
    public async Task<Result<FormEvaluationDto>> SaveEvaluationForm([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().SaveEvaluationForm(formEvaluation);
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

}