using Evaluation.API.Extensions;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.PlanLayer;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

public class PlanTypeController(MasterBL masterBl) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPlanTypes()
    {
        var planTypes = await masterBl.GetApiService<PlanServiceRequestServices>().GetPlanType();
        return planTypes.ToActionResult();
    }
}
