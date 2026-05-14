using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

public class ReassignController : Controller
{
    public IActionResult ReassignPage()
    {
        return View();
    }
}
