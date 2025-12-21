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

		[HttpGet]
		public IActionResult RenderSchoolDetails(Guid orgTreeId)
		{
			var schoolHtml = $@"
					<div class='row'>
						<div class='col-md-6'><strong>School ID:</strong> {orgTreeId}</div>
					</div> ";

			return ViewComponent(
				"SchoolDetailsModal",
				new
				{
					modalId = "schoolDetailsContent",
					title = "بيانات المدرسة",
					body = schoolHtml
				}
			);
		}

	}
}
