using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

public class FormController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
