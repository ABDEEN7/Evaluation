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
        public IActionResult Update(Guid planId)
        {
            return View(planId);
        }
		
	}
}
