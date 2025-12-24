using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{

	public class EvaluationPlanController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
        public IActionResult Calendar()
        {
            return View();
        }


	}
}
