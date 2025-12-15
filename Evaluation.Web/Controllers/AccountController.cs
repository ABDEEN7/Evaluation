using Evaluation.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Web.Controllers
{

    //[Route("[controller]")]
    public class AccountController : Controller
    {

        [HttpGet()]
        public IActionResult Login(string? redirectUrl = null)
        {
            var model = new LoginViewModel { RedirectUrl = redirectUrl };
            return View(model);
        }




        [HttpGet]
        public IActionResult UserProfile()
        {
            return View();
        }


        [HttpGet]
        //[Route("MSVerify/{code}")]
        public IActionResult MSVerify(string code) //code
        {
            ViewData["code"] = code;
            return View();
        }

       


    }
}
