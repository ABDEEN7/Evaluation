using Microsoft.AspNetCore.Http;

namespace Evaluation.Web.Middlewares
{
    public class SecurityLayerMiddleware
    {

        private readonly RequestDelegate _next;

        public SecurityLayerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            string userAgent = context.Request.Headers["User-Agent"].ToString().ToLower();
            var headers = context.Request.Headers;
            string[] AllowUserAgent = new string[] { "chrome/", "edg/", "safari/", "firefox/" };

            bool isAllowed = AllowUserAgent.Any(agent => userAgent.Contains(agent));


            // Check if the request is an AJAX request
            bool isAjaxRequest = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (!isAllowed)
            {
                if (isAjaxRequest)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access Forbidden: Your user-agent is not allowed.");
                }
                else
                {
                    context.Response.Redirect("/Error");
                }
                return;
            }
            var csp = "default-src 'self' data:; " +
                       "script-src * 'unsafe-inline' 'unsafe-eval'; " +
                       "style-src * 'unsafe-inline'; " +
                       "img-src *; " +
                       "font-src 'self' data:; " +
                       "connect-src *; " +
                       "frame-src *; " +
                       "object-src *; " +
                       "media-src *; " +
                       "worker-src *; " +
                       "child-src *;";




            headers.TryAdd("Content-Security-Policy", csp);
            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("X-Frame-Options", "SAMEORIGIN");
            headers.TryAdd("X-Xss-Protection", "1; mode=block");

            headers.Remove("X-Powered-By");
            headers.Remove("Server");
            headers.Remove("Sec-Ch-Ua-Platform");

            // Call the next middleware in the pipeline
            await _next(context);

        }


    }
}
