using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
	public class EvaluationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

        [HttpGet("Calendar")]
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
