using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

[Route("{language=ar}/[controller]/{depRouting}")]
public class DepartmentHolidayController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
