using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;
[Route("{language=ar}/[controller]")]
public class CalendarController : Controller
{
    [HttpGet("{depRouting}/Calendar")]
    public IActionResult Calendar(string depRouting)
    {
        return View();
    }
}