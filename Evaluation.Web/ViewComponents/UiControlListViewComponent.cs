
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Evaluation.Web.Models;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Web.ViewComponents
{
    public class UiControlListViewComponent : ViewComponent
    {

       
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RequestInfo requestInfo;
        public UiControlListViewComponent(IHttpContextAccessor httpContextAccessor, RequestInfo requestInfo)
        {
            this.httpContextAccessor = httpContextAccessor;
            var serviceProvider = this.httpContextAccessor.HttpContext?.RequestServices;
            this.requestInfo = requestInfo;
           
        }

        public async Task<IViewComponentResult> InvokeAsync(List<UiControlItemDTO> modeldata)
        {
            var model = new UiControlListVM();
            model.Lang = requestInfo.Lang;
            model.UiControlItemList = modeldata;

            return View(model);
        }
    }
}
