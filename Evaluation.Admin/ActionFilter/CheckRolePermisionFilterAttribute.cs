using Evaluation.Admin.Extensions;
using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Evaluation.Admin.ActionFilter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CheckRolePermisionFilterAttribute : ActionFilterAttribute
    {
        private readonly bool redirectToAccessDenied;

        // protected readonly UserBasicInfo userBasicInfo;
        //protected readonly IServiceProvider serviceProvider;

        public CheckRolePermisionFilterAttribute(bool redirectToAccessDenied = false, params string[] PermisionNames)
        {
            this.redirectToAccessDenied = redirectToAccessDenied;
            this.PermisionNames = PermisionNames;

        }

        public string[] PermisionNames { get; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            UserInfo userBasicInfo = serviceProvider.GetRequiredService<UserInfo>();

            MasterBL mbl = serviceProvider.GetRequiredService<MasterBL>();

            bool CheckPermision = false;

            if (PermisionNames != null && PermisionNames.Count() > 0)
            {

                foreach (var item in PermisionNames)
                {
                    var output = new StringBuilder();

                    output.Append($"Permission : {item}");



                    CheckPermision = userBasicInfo.PermissionList?.IsPermissionAvailabe(item) ?? false;
                    if (CheckPermision)
                    {
                        CheckPermision = true;
                        base.OnActionExecuting(context);
                        await next();
                        break;
                    }
                }
            }
            var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

            if (!CheckPermision)
            {
                if (redirectToAccessDenied)
                {
                    var lang="ar";
                    var languagefromurl = returnUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    if(languagefromurl.Length>0)
                    {
                        lang = languagefromurl[0];
                    }


                    var redirectUrl = $"/{lang}/AccessDenied?returnUrl={Uri.EscapeDataString(returnUrl)}";
                    context.HttpContext.Response.Redirect(redirectUrl);
                }

                else
                    throw new UnauthorizedAccessException("UnAuthorized Data");
            }
            
        }
    }
}
