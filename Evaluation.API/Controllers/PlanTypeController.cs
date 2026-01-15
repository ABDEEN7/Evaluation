using Evaluation.API.ActionFilter;
using Evaluation.API.Extensions;
using Evaluation.DAL;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;
[Route("api/[controller]/[action]")]
public class PlanTypeController(MasterBL masterBl) : ControllerBase
{
    [HttpGet]
    //[CheckRolePermisionFilter(true, ConstantKeys.WebPermission.GET_WEB_PLAN_TYPE_REQUEST)]
    public async Task<IActionResult> GetPlanTypes()
    {
        var planTypes = await masterBl.GetApiService<PlanServiceRequestServices>().GetPlanTypes();
        return Ok(new { result = planTypes });
    }
}
