using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Evaluation.Admin.ActionFilter
{

    public class PopulateResponseInfoFilter : IActionFilter
    {
        private readonly ResponseInfo responseInfo;
        private readonly string defaultLanguage;
        private readonly List<string> supportedLanguages;

        public PopulateResponseInfoFilter(ResponseInfo responseInfo, IConfiguration configuration)
        {
            this.responseInfo = responseInfo;

            // Get default language and supported languages from configuration
            defaultLanguage = configuration.GetValue<string>("defaultLanguage") ?? "ar"; // Default to "en" if not set
            supportedLanguages = configuration.GetValue<string>("supportedLanguages")?.Split(',').ToList()
                ?? new List<string> { "ar" }; // Default to English if not set
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Populate ResponseInfo before the action executes
            responseInfo.Path = context.HttpContext.Request.Path;
            responseInfo.Method = context.HttpContext.Request.Method;
            responseInfo.ResponseTime = DateTime.UtcNow;

            // Extract language from the URL path (assumed to be the first segment)
            var segments = context.HttpContext.Request.Path.Value?.Split('/');
            if (segments?.Length > 1)
            {
                var lang = segments[1]; // The second segment is expected to be the language code
                                        // Validate against supported languages
                responseInfo.Lang = supportedLanguages.Contains(lang) ? lang : defaultLanguage;
            }
            else
            {
                responseInfo.Lang = defaultLanguage; // Use the default language if not found
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No additional logic needed after the action executes
        }
    }


}
