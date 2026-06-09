using Evaluation.Services.Integration;
using Evaluation.SharedHelper.Dtos.NsisIntegrationDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class NSISIntegrationController(NSISService nsisService, HRNSISService hrNsisService) : ControllerBase
{
    private readonly NSISService _nsisService = nsisService;
    private readonly HRNSISService _hrNsisService = hrNsisService;

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
    public async Task<NSISSchool> GetNSISSchoolByIdAsync(Guid Id)
    {
        var result = await _nsisService.GetSchoolbyIdAsync(Id);
        return result;

    }

	[HttpPost]
	public async Task<IActionResult> SyncSchools(string schoolCategory,string schoolOrgTypeBackendName)
	{
		var result = await _hrNsisService.SyncAllSchoolsAsync(schoolCategory,schoolOrgTypeBackendName);

		return Ok(new
		{
			Success = result,
			Message = result
				? "Schools synchronized successfully."
				: "No schools found to synchronize."
		});
	}
}
