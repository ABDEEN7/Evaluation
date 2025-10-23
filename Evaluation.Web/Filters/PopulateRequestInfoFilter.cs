using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Evaluation.Web.Filters
{
    public class PopulateRequestInfoFilter : ActionFilterAttribute
    {
        private readonly RequestInfo requestInfo;
        private readonly string defaultLanguage;
        private readonly List<string> supportedLanguages;

        public PopulateRequestInfoFilter(RequestInfo requestInfo, IConfiguration configuration)
        {
            this.requestInfo = requestInfo;

            // Get default language and supported languages from configuration
            defaultLanguage = configuration.GetValue<string>("defaultLanguage") ?? "en"; // Default to "en" if not set
            supportedLanguages = configuration.GetValue<string>("supportedLanguages")?.Split(',').ToList()
                ?? new List<string> { "en" }; // Default to English if not set
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            // Extract language from the URL path (assumed to be the first segment)
            var segments = context.HttpContext.Request.Path.Value.Split('/');
            if (segments.Length > 1)
            {
                var lang = segments[1]; // The second segment is expected to be the language code
                                        // Validate against supported languages
                requestInfo.Lang = supportedLanguages.Contains(lang) ? lang : defaultLanguage;
            }
            else
            {
                requestInfo.Lang = defaultLanguage; // Use the default language if not found
            }

            base.OnResultExecuted(context);
        }



    }
}
