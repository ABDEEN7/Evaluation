using Evaluation.API.Models;
using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly MasterBL masterBL;
		private readonly RequestInfo requestInfo;
		private readonly UserInfo userInfo;

		public UserController(MasterBL masterBL, RequestInfo requestInfo, UserInfo userInfo)
		{
			this.masterBL = masterBL;
			this.requestInfo = requestInfo;
			this.userInfo = userInfo;
		}
		


		[HttpGet]
		public async Task<ApiResponse<UserProfileDTO>> UserDetails()
		{
			var result = await masterBL.GetApiService<UserBL>().GetUserDetails();
			var response = new ApiResponse<UserProfileDTO>
			{
				Data = result
			};
			return response;
		}
	}
}
