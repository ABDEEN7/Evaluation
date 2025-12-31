namespace Evaluation.Web.Middlewares
{
    public class LanguageHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration config;

        public LanguageHandlerMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            this.config = config;
        }



        public async Task InvokeAsync(HttpContext context)
        {

            var pathSegments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
            var mainLang = pathSegments.Count > 0 ? pathSegments[0] : string.Empty;

            // Retrieve supported and default languages from configuration
            var supportedLanguages = config.GetValue<string>("supportedLanguages").Split(',').ToList();

            // If language is unsupported or missing, use the default
            if (string.IsNullOrEmpty(mainLang) || !supportedLanguages.Contains(mainLang))
            {
                mainLang = GetCookieLang(context);

                pathSegments.Insert(0, mainLang);             // Prepend the default language

                // Construct the new path
                var defaultWebGroup = config.GetValue<string>("defaultWebGroup");
                var newPath = $"/{string.Join('/', pathSegments)}/{defaultWebGroup}";
                var newUrl = $"{(context.Request.IsHttps ? "https" : "http")}://{context.Request.Host}{newPath}{context.Request.QueryString}";

                // Redirect to the URL with the correct language
                context.Response.Redirect(newUrl);
                return;
            }

            // Proceed to next middleware if language is valid
            await _next(context);

            // Check if the response status code is 404 and request path is not already the error page
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {

                pathSegments = context.Request.Path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
                mainLang = pathSegments.Count > 0 ? pathSegments[0] : string.Empty;

                // Retrieve supported and default languages from configuration
                supportedLanguages = config.GetValue<string>("supportedLanguages").Split(',').ToList();

                // If language is unsupported or missing, use the default
                if (string.IsNullOrEmpty(mainLang) || !supportedLanguages.Contains(mainLang))
                {
                    mainLang = GetCookieLang(context);
                }

                var newUrl = $"{(context.Request.IsHttps ? "https" : "http")}://{context.Request.Host}/{mainLang}";

                // Redirect to the URL with the correct language
                context.Response.Redirect(newUrl);
            }
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
