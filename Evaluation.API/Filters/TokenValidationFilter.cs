using Evaluation.Services.BusinessLayer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Filters
{
    public class TokenValidationFilter : IActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // This is where the code runs before the action is executed

            // Retrieve the token from the Authorization header (if present)
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", "");

                if (!string.IsNullOrEmpty(token))
                {
                    // Get the Authentication service from the service provider
                    var masterBL = _serviceProvider.GetRequiredService<MasterBL>();

                    // Fetch the user token information using the token
                    var userToken = masterBL.GetApiService<AuthenticationBL>().GetUserToken(token).Result;  // Synchronous wait for async task

                    // If the token is not found, return a 404 response
                    if (userToken != null)
                    {
                        // If the token is deprecated, return a 403 Forbidden response
                        if (userToken.Deprecated)
                        {
                            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Result = new JsonResult("Token is deprecated");
                            return;
                        }
                    }

                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This is where the code runs after the action has been executed, but before the result is sent to the client
            // You can modify the response if needed here.
        }
    }
}
