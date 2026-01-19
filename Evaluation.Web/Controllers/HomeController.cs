using Evaluation.SharedHelper.Models.Api;
using Evaluation.SharedHelper.Extensions;
using Evaluation.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Evaluation.Web.Controllers
{
    [Route("{language}/[controller]/[action]")]
    [Route("{language}/")]
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id?}")]
        public IActionResult Index(string? id)
        {
            id = (id == "en" || id == "ar" || id == null) ? "default" : id;
            ViewBag.Webgroup = id;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

        [HttpPost]
        public IActionResult UiControlList()
        {
            var model = Request.Form["request"][0]?.StringToObject<List<UiControlItemDTO>>();

            return ViewComponent("UiControlList", model);
        }

       
    }
}
