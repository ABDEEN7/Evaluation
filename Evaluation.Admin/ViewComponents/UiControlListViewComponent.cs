
using Microsoft.AspNetCore.Mvc;
using Evaluation.Admin.Models;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Evaluation.DAL.Helper;


namespace Evaluation.Admin.ViewComponents
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
            var model = new UiControlListVM(httpContextAccessor);

            await model.LoadAllAData();
            model.Lang= requestInfo.Lang;
            model.UiControlItemList = modeldata;
           
            return View(model);
        }
    }
}
