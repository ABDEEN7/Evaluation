using Evaluation.API.Filters;
using Evaluation.API.Models;
using Evaluation.Services.BusinessLayer;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Helper;
using Evaluation.SharedHelper.Models.Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly MasterBL masterBL;
        private readonly RequestInfo requestInfo;

        public AccountController(MasterBL masterBL, RequestInfo requestInfo, UserInfo userInfo)
        {
            this.masterBL = masterBL;
            this.requestInfo = requestInfo;
        }

        [HttpPost("Login")]
        public async Task<ApiResponse<string>> Login(AuthorizationCodeRequest model)
        {
            var data = await masterBL.GetApiService<AuthenticationBL>().LoginAsync(model);

            await masterBL.GetApiService<AuthenticationBL>()
               .RegisterUserToken(data.userId, data.token)
               .ConfigureAwait(false);

            var response = new ApiResponse<string>
            {
                Data = data.token,//token
            };
            return response;
        }
        [HttpPost]
        public async Task<IActionResult> CheckUserAuth([FromForm] string username)
        {
            try
            {
                var redirectUrl = await masterBL.GetApiService<AuthenticationBL>().CheckUserAuth(username);
                return Ok(redirectUrl);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [Authorize]
        [ServiceFilter(typeof(TokenValidationFilter))]
        [HttpGet("RefreshToken")]
        public async Task<ApiResponse<string>> RefreshToken()
        {
            var data = await masterBL.GetApiService<AuthenticationBL>().RefreshTokenAsync(requestInfo.Token);

            await masterBL.GetApiService<AuthenticationBL>()
               .RegisterUserToken(data.userId, data.token)
               .ConfigureAwait(false);

            await masterBL.GetApiService<AuthenticationBL>()
               .DeprecateUserToken(requestInfo.Token)
               .ConfigureAwait(false);

            var response = new ApiResponse<string>
            {
                Data = data.token,//new token
            };
            return response;
        }

        [Authorize]
        [ServiceFilter(typeof(TokenValidationFilter))]
        [HttpPost("Logout")]
        public async Task<ApiResponse<bool>> Logout()
        {
            await masterBL.GetApiService<AuthenticationBL>()
               .DeprecateUserToken(requestInfo.Token)
               .ConfigureAwait(false);

            var response = new ApiResponse<bool>
            {
                Data = true,
            };
            return response;
        }

        [Authorize]
        [ServiceFilter(typeof(TokenValidationFilter))]
        [HttpPost("GetUserPagePermissions")]
        public async Task<ApiResponse<List<PermissionDTO>>> GetUserPagePermissions([FromBody] GetUserPagePermissionsDTO model)
        {
            //should be replace by database query

            var list = await masterBL.GetApiService<AuthenticationBL>().GetUserPagePermissions(model.pageNames);
            var response = new ApiResponse<List<PermissionDTO>>
            {
                Data = list
            };
            return response;
        }



    }
}
