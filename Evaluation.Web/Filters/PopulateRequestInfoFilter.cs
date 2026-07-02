using Evaluation.DAL.Helper;
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
            defaultLanguage = configuration.GetValue<string>("defaultLanguage") ?? "en";
            supportedLanguages = configuration.GetValue<string>("supportedLanguages")?.Split(',').ToList()
                ?? new List<string> { "en" };
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var segments = context.HttpContext.Request.Path.Value.Split('/');
            if (segments.Length > 1)
            {
                var lang = segments[1];
                requestInfo.Lang = supportedLanguages.Contains(lang) ? lang : defaultLanguage;
            }
            else
            {
                requestInfo.Lang = defaultLanguage;
            }
            base.OnActionExecuting(context);
        }
    }
}
