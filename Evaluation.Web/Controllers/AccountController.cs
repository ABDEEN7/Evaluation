using Evaluation.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Web.Controllers
{

    //[Route("[controller]")]
    public class AccountController : Controller
    {

        [HttpGet()]
        public IActionResult Login(string redirectUrl = null)
        {
            var model = new LoginViewModel { RedirectUrl = redirectUrl };
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            var model = new ResetPasswordViewModel { ResetPasswordLogId = id };
            return View(model);
        }


        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }


        public IActionResult VerifyNewEmail(string id)
        {
            var model = new VerifyNewEmailViewModel { ChangeEmailRequestId = id, };
            return View(model);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangeMobile()
        {
            return View();
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

        [HttpGet]
        public IActionResult MobileResetPassword()
        {
            return View();
        }


    }
}
