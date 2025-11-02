using Evaluation.DAL.Helper;
using Evaluation.SharedHelper.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;

namespace Evaluation.SharedHelper.Middlewares
{
    public class PopulateRequestInfoMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<PopulateRequestInfoMiddleware> _logger;

        public PopulateRequestInfoMiddleware(RequestDelegate next, ILogger<PopulateRequestInfoMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestInfo requestInfo)
        {
            var segments = context.Request.Path.Value.Split('/');
            var lang = "ar";
            if (segments.Length>0)
            {
                 lang = segments[1]?? AppSettings.DefaultLanguage;
            }


            // Set language from header, default to "ar"
            requestInfo.Lang = lang;

            // Set page number from body (if POST/PUT) or query string
            if (context.Request.Method == HttpMethods.Post || HttpMethods.Put.Equals(context.Request.Method))
            {
                requestInfo.PageNumber = await GetPageNumberFromBody(context) ?? GetPageNumberFromQuery(context);
            }
            else
            {
                requestInfo.PageNumber = GetPageNumberFromQuery(context);
            }

            // Set User-Agent from request headers
            requestInfo.UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";

            // Get the user IP address
            requestInfo.UserIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            // Get controller and action from route values
            requestInfo.Controller = context.GetRouteValue("controller")?.ToString() ?? "Unknown";
            requestInfo.Action = context.GetRouteValue("action")?.ToString() ?? "Unknown";

            // Retrieve the Authorization header and check if it exists
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            // If the header is null or doesn't start with "Bearer ", handle it appropriately
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var currentToken = authHeader.Replace("Bearer ", "");
                requestInfo.Token = currentToken;
            }

            //_logger.LogInformation($"RequestContext: Lang={requestContext.Lang}, Page={requestContext.PageNumber}, UserAgent={requestContext.UserAgent}, UserIp={requestContext.UserIp}");

            // Call the next middleware in the pipeline
            await _next(context);
        }

        private int? GetPageNumberFromQuery(HttpContext context)
        {
            return int.TryParse(context.Request.Query["pageNumber"].FirstOrDefault(), out var page) ? page : (int?)null;
        }

        private async Task<int?> GetPageNumberFromBody(HttpContext context)
        {
            if (context.Request.ContentLength > 0 && context.Request.ContentType.Contains("application/json"))
            {
                context.Request.EnableBuffering(); // Allows reading the body multiple times
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset the stream position for next middleware
                if (!string.IsNullOrEmpty(body))
                {
                    //var bodyData = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                    //if (bodyData != null && bodyData.TryGetValue("pageNumber", out var pageValue) && int.TryParse(pageValue.ToString(), out var page))
                    //{
                    //    return page;
                    //}
                }
            }
            return null;
        }
    }
}
