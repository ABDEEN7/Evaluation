using Evaluation.DAL.DTOs;
using Evaluation.Services.Integration;
using Evaluation.Services.Models.SMTP;
using Evaluation.Services.Special;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{depRouting}/[action]")]
    public class TestController :  ControllerBase
    {
        private readonly HRService _hrService;
		private readonly IEmailServices EmailServices;

		public TestController(
		   HRService hrService,
		   IEmailServices EmailServices)
		{
			_hrService = hrService;
			this.EmailServices = EmailServices;
		}
		[HttpGet]
        public async Task<List<HREmployeeInfoDto>> GetHREmployeesDetails(int page)
        {
            return await _hrService.GetAllHRUsersAsync(page);
        }

        [HttpGet]
        public async Task<List<HREmployeeInfoDto>> GetHREmployees(long? qID = null, string email = null, string orgno = null)
        {
            return await _hrService.GetHRUsersAsync(qID, email, orgno);
        }

        [HttpGet]
        public async Task<bool> AddUpdateOrgTree(string? hrCode = null, long? qID = null)
        {
            return await _hrService.AddUpdateOrgTree(hrCode, qID);
        }

        [HttpPost]
        public async Task<bool> AddUpdateAllSchools()
        {
            return await _hrService.AddUpdateAllSchools();
        }


        [HttpGet]
        public async Task<List<HROrganizationInfoDto>> GetHROrgDetailsAsync(int page)
        {
            return await _hrService.GetAllHROrgAsync(page);
        }

        [HttpGet]
        public async Task<List<HROrganizationInfoDto>> GetAllHRSchoolsAsync()
        {
            return await _hrService.GetAllHRSchoolsAsync();
        }

		[HttpGet]
		public async Task<bool> SendTestEmail(string email)
		{
			
			return await EmailServices.SendTestEmail(email);
		}
	}
}
