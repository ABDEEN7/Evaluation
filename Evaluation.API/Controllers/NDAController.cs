using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.NDABL;
using Evaluation.Services.BusinessLayer.API.TeamMemberBL;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/{depRouting}/[action]")]

public class NDAController : ControllerBase
{
    private readonly MasterBL _masterBl;

    public NDAController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }
    [HttpGet]
    public async Task<Result<NDADto>> GetPendingStatus()
    {
        var result =await _masterBl.GetApiService<NdaBL>().GetPendingNda();
        return result;
    }
}
