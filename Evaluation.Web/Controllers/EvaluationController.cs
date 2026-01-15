using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    [Route("{language}")]
    [Route("{language}/Evaluation")]
    public class EvaluationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
       
    }
}
