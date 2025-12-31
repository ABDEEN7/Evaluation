using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

public class TeamMembersController : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
}
