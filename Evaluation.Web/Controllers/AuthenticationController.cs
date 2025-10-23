using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
