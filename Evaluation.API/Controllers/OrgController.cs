using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormLayer;
using Evaluation.SharedHelper.Dtos.OrgDto;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
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
}
