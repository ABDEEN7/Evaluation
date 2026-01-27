using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

public class AssignmentsController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
