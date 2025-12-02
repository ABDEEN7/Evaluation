using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Evaluation.API.ActionFilter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CheckRolePermisionFilterAttribute : ActionFilterAttribute
    {
        private readonly bool redirectToAccessDenied;

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

            var actionName = context.ActionDescriptor.RouteValues["action"];
            var controllerName = context.ActionDescriptor.RouteValues["controller"];

            // ✅ Skip permission check for students ONLY on SetAllNotificationsAsRead
            if (userBasicInfo.UserType == "Student" &&
                controllerName == "Notification")
            {
                await next();
                return;
            }

            bool CheckPermision = false;

            if (PermisionNames != null && PermisionNames.Count() > 0)
            {

                foreach (var item in PermisionNames)
                {
                    var output = new StringBuilder();

                    output.Append($"Permission : {item}");



                    CheckPermision = IsPermissionAvailabe(userBasicInfo.PermissionList, item);
                    if (CheckPermision)
                    {
                        CheckPermision = true;
                        base.OnActionExecuting(context);
                        await next();
                        break;
                    }
                }
            }


            if (!CheckPermision)
            {
                if (redirectToAccessDenied)
                {
                    context.Result = new ForbidResult();
                }

                else
                    throw new UnauthorizedAccessException("UnAuthorized Data");
            }

        }


        public bool IsPermissionAvailabe(List<string> Permisions, string Key)
        {

            var isAvl = Permisions?.FirstOrDefault(c => c == Key);
            if (null == isAvl) return false;
            else return true;
        }
    }
}
