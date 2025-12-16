using AutoMapper;
using Evaluation.API.ActionFilter;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.SharedHelper.Dtos.Form;
using Evaluation.SharedHelper.Enums;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
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
        List<FormEvalMarixValueDto> FormEvalMarixValues = new List<FormEvalMarixValueDto>();
        FormEvalMarixValues.Add(new FormEvalMarixValueDto() { Id = new Guid(), Name = "Agree", MaxValue = 100 });
        FormEvalMarixValues.Add(new FormEvalMarixValueDto() { Id = new Guid(), Name = "Disagree", MaxValue = 49 });
        return FormEvalMarixValues;
    }
}