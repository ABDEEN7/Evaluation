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
                ActionType = "EDIT",
                PlanId = new Guid("1FB93348-FF57-47B7-889E-F7C5D9449FFF"),
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

        [HttpGet]
        public IActionResult CreatePartial()
        {
            var model = new PlanViewModel
            {
                ActionType = "CREATE",
                RenderType = "action"
            };

            return PartialView("_Create", model);
        }

    }
}
