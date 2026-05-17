using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    [Route("{language=ar}/[controller]")]
    public class TemplateFormController : Controller
    {
        [HttpGet("")]
        public IActionResult Default(string? id)
        {
            id = (id == "en" || id == "ar" || id == null) ? "default" : id;
            ViewBag.Webgroup = id;
            return View();
        }

        [HttpGet("{depRouting}")]
        public IActionResult Index(string depRouting)
        {
            return View();
        }
    }
}