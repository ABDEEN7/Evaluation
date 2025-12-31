using System.Diagnostics;
using Evaluation.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    [Route("{language}")]
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
            ViewBag.Webgroup = id;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
