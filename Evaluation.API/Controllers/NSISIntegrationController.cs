using Evaluation.Services.Integration;
using Evaluation.SharedHelper.Dtos.NsisIntegrationDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class NSISIntegrationController(NSISService nsisService) : ControllerBase
{
    private readonly NSISService _nsisService = nsisService;

    [HttpPost()]
    public async Task<AuthenticationResponse> TestNSISAuthToken()
    {
        var result = await _nsisService.GetTokenAsync();
        return result;

    }

    [HttpGet()]
    public async Task<List<SchoolDto>> GetNSISSchoolAsync()
    {
        var result = await _nsisService.GetSchoolsAsync();
        return result;

    }

    [HttpGet()]
    public async Task<SchoolDto> GetNSISSchoolByIdAsync(Guid Id)
    {
        var result = await _nsisService.GetSchoolbyIdAsync(Id);
        return result;

    }
}
