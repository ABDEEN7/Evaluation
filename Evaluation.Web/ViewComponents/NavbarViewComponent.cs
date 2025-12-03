using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
