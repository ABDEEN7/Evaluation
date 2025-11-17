using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Update(Guid planId)
        {
            return View(planId);
        }
    }
}
