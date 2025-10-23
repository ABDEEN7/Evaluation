using Evaluation.Web.Special;
using System.Globalization;

namespace Evaluation.Web.Middlewares
{
    public class CookiesProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration config;

        public CookiesProviderMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            this.config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            SetWebAppBaseURL(context);
            SetLanguage(context);
            await _next(context);
        }

        private void SetWebAppBaseURL(HttpContext context)
        {
            var cookieService = context.RequestServices.GetRequiredService<CookieServices>();

            var pathSegments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
            var mainLang = pathSegments.Count > 0 ? pathSegments[0] : string.Empty;
            var url = config.GetValue<string>("baseAppUrl");
            // Retrieve supported and default languages from configuration
            var supportedLanguages = config.GetValue<string>("supportedLanguages").Split(',').ToList();
            var defaultLanguage = config.GetValue<string>("defaultLanguage");
            // If language is unsupported or missing, use the default
            if (string.IsNullOrEmpty(mainLang) || !supportedLanguages.Contains(mainLang))
            {
                mainLang = defaultLanguage;
            }
            url = url.Replace("{lang}", mainLang);
            cookieService.SetCookie("webAppBaseURL", url);

        }

        private void SetWebApiBaseURL(HttpContext context)
        {
            var cookieService = context.RequestServices.GetRequiredService<CookieServices>();

            cookieService.SetCookie("webApiBaseURL", config.GetValue<string>("baseApiUrl"));
        }

        private void SetLanguage(HttpContext context)
        {
            var cookieService = context.RequestServices.GetRequiredService<CookieServices>();

            var pathSegments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
            var supportedLanguages = config.GetValue<string>("supportedLanguages").Split(',').ToList();
            var mainLang = string.Empty;
            if (pathSegments.Count > 0)
            {
                mainLang = pathSegments[0];
                // If language is unsupported or missing, use the default
                if (supportedLanguages.Contains(mainLang))
                {
                    CultureInfo.CurrentCulture = new CultureInfo(mainLang);
                    CultureInfo.CurrentUICulture = new CultureInfo(mainLang);
                    cookieService.SetCookie("lang", mainLang);
                }
            }
        }
    }
}
