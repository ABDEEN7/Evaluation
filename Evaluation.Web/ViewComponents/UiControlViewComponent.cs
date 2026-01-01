
using Microsoft.AspNetCore.Mvc;
using Evaluation.SharedHelper.Models;
using Evaluation.Web.Models;
using Evaluation.SharedHelper.Models.Api;


namespace Evaluation.Web.ViewComponents
{
    public class UiControlViewComponent : ViewComponent
    {

       
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RequestInfo requestInfo;
        public UiControlViewComponent(IHttpContextAccessor httpContextAccessor, RequestInfo requestInfo)
        {
            this.httpContextAccessor = httpContextAccessor;
            var serviceProvider = this.httpContextAccessor.HttpContext?.RequestServices;
            this.requestInfo = requestInfo;
           
        }

        public async Task<IViewComponentResult> InvokeAsync(UiControlItemDTO modeldata)
        {
            var model = new UiControlVM();
            model.Lang = requestInfo.Lang;
            model.ControlItem = modeldata;

            return View(model);
        }
    }
}
