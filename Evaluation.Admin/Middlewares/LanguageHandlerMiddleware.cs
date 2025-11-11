using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Security;

namespace Evaluation.Admin.Middlewares
{
    public class LanguageHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string cookieLang = context.Request.Cookies["AdminLang"] ?? "ar";


            await _next(context);
        }
    }
}
