namespace Evaluation.Web.Middlewares
{
    public class MSVerifyRedirectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration config;

        public MSVerifyRedirectionMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            this.config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            if (request.Path.HasValue && context.Request.Path == "/signin-oidc")
            {
                var mainLang = GetCookieLang(context);
                var code = context.Request.Query["code"].ToString();
                context.Response.Redirect($"/{mainLang}/Account/MsVerify?code={code}", false);
                return;
            }
            await _next.Invoke(context);
        }

        private string GetCookieLang(HttpContext context)
        {
            var result = string.Empty;
            // Retrieve the cookie value
            if (context.Request.Cookies.TryGetValue("lang", out var cookieValue))
            {
                result = cookieValue;
            }

            var supportedLanguages = config.GetValue<string>("supportedLanguages").Split(',').ToList();
            var defaultLanguage = config.GetValue<string>("defaultLanguage");

            // If language is unsupported or missing, use the default
            if (string.IsNullOrEmpty(result) || !supportedLanguages.Contains(result))
            {
                result = defaultLanguage;
            }

            return result ?? "ar";
        }
    }
}
