using Evaluation.API.ActionFilter;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.EvaluationForm;
using Evaluation.Services.Models.Admin;
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
}
