using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.SharedHelper.Dtos.OrgDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]
public class OrgController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public OrgController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    public async Task<OrgDetailsDto> GetOrgDetails([FromQuery] Guid OrgID)
    {
        var details = await _masterBl.GetApiService<OrgBL>().GetOrgDetails(OrgID);
        return details;
    }
    [HttpGet]
    public async Task<IActionResult> GetParentOrgTree()
    {
        return Ok(new { result = await _masterBl.GetApiService<OrgBL>().GetParentOrgTreeAsync() });
    }
}
