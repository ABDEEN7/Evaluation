using AutoMapper;
using Evaluation.API.ActionFilter;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.FormLayer;
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
    [CheckRolePermisionFilter(true, PermisionNames: [ConstantKeys.WebPermissions.GET_FORM_ITEMS])]

    public async Task<Result<List<FormItemDto>>> GetItems([FromQuery] Guid formId)
    {
        return await _masterBl.GetApiService<FormBL>().GetFormItems(formId);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: [ConstantKeys.WebPermissions.SAVE_EVALUATION])]

    public async Task<Result<FormEvaluationDto>> SaveEvaluation([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().SaveEvaluationForm(formEvaluation);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: [ConstantKeys.WebPermissions.UPDATE_EVALUATION])]

    public async Task<Result<FormEvaluationDto>> UpdateEvaluation([FromBody] FormEvaluationDto formEvaluation)
    {
        return await _masterBl.GetApiService<FormBL>().UpdateEvaluationForm(formEvaluation);
    }
}