using Azure.Core;
using Evaluation.Api.Extensions;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.EvaluationForm;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
public class EvaluationFormController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public EvaluationFormController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_EVALFORMS })]
    public async Task<IActionResult> GetAllEvalForm(int Page = 1)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetEvaluationForm(Page));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_EVALFORMS })]
    public async Task<IActionResult> SaveEvaluationForm()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveEvaluationForm(request!));
    }

    [HttpPost]
   //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_EVALFORMS })]
    public async Task<IActionResult> UpdateEvaluationForm()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateEvaluationForm(request!));
    }
    [HttpPost]
   // [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_EVALFORMS })]
    public async Task<IActionResult> DeleteEvaluationForm(Guid Id)
    {

        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().DeleteEvaluationForm(Id!));

    }

}
