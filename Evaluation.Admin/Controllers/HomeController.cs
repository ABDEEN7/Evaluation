using Evaluation.Admin.Models;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MasterBL _masterBL;
        private readonly RequestInfo _requestInfo;
        public HomeController(ILogger<HomeController> logger, MasterBL masterBL, RequestInfo requestInfo)
        {
            _logger = logger;
            _masterBL = masterBL;
            _requestInfo = requestInfo;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SetLanguage()
        {


            //string lang = _requestInfo.Lang;
            string URLpath = HttpContext.Request.Headers["Referer"].ToString();
            var segments = HttpContext.Request.Path.Value!.Split('/');
            _requestInfo.Lang = _requestInfo.Lang == null ? "ar" : _requestInfo.Lang == "ar" ? "en" : _requestInfo.Lang == "en" ? "ar" : "ar";
            if (segments.Length > 1)
            {
                var lang =  _requestInfo.Lang;
                if (lang == null || lang == "en")
                {
                    if (URLpath.Contains("/ar"))
                    {
                        URLpath = URLpath.Replace("/ar", "/en");
                    }
                    else
                    {
                        URLpath = URLpath + "en";
                    }



                }
                else
                {
                    if (URLpath.Contains("/en"))
                    {
                        URLpath = URLpath.Replace("/en", "/ar");
                    }
                    else
                    {
                        URLpath = URLpath + "ar";
                    }


                }

            }
            return Redirect(URLpath);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult GetSystemLogo()
        {

            var FaviconSetting = _masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.Favicon);

            Dictionary<string, object> response = new Dictionary<string, object>();

            response.Add("FaviconSetting", FaviconSetting);

            return Ok(response);
        }

        public IActionResult GetIdleTimeout()
        {
            // Let's say you fetch this from DB
            int idleMinutes =20; //Convert.ToInt32(_masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.SessionExpireTime)??"20");
            return Ok(idleMinutes);
        }
    }
}
