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
            return View();
        }
		
	}
}
