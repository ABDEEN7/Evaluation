using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
	[Route("{language=ar}/[controller]/{depRouting}")]
	public class EvaluationPlanController : Controller
	{
		[HttpGet("")]
		[HttpGet("Index")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
