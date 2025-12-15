using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers;

public class FormController : Controller
{
    public ActionResult CreatePartial()
    {
        return PartialView("_Create");
    }
}
