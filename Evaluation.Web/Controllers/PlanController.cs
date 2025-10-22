using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        //TODO: This action is temp until we add translation
        public IActionResult CreateAr()
        {
            return View();
        }
    }
}
