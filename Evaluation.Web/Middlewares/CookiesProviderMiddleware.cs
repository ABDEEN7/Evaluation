using Evaluation.Web.Special;
using System.Globalization;
using System.Net;

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
            SetWebApiBaseURL(context);
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
			var url = config.GetValue<string>("baseApiUrl");
			if (string.IsNullOrWhiteSpace(url)) return;
			cookieService.SetCookie("webApiBaseURL", url);
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
