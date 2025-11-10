
using Microsoft.AspNetCore.Mvc;
using Evaluation.Admin.Models;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.DAL.Helper;


namespace Evaluation.Admin.ViewComponents
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
            var model = new UiControlVM(httpContextAccessor);

            await model.LoadAllAData();
            model.Lang= requestInfo.Lang;
            model.ControlItem = modeldata;
           
            return View(model);
        }
    }
}
