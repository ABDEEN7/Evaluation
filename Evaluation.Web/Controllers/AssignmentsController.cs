using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

public class AssignmentsController : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
}
