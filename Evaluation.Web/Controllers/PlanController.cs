using Evaluation.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var viewModel = new PlanViewModel
            {
                RenderType = "action",
                ActionType = "CREATE",
                PlanId = null,
                OldPlanId = null
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ViewPlan(Guid id)
        {
            return View(id);
        }
        [HttpGet]
        public IActionResult Details(Guid planId)
        {
            ViewBag.PlanId = planId;
            return View();
        }
        [HttpPost]
        public IActionResult Update(Guid planId)
        {
            return View(planId);
        }

    }
}
