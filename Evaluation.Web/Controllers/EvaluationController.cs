using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
	public class EvaluationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
       
    }
}
