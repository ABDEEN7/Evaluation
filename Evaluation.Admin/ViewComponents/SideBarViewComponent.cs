
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.ViewComponents
{
    public class SideBarViewComponent : ViewComponent
    {


        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RequestInfo requestInfo;
        private readonly MasterBL _masterBL;
        public SideBarViewComponent(IHttpContextAccessor httpContextAccessor, MasterBL masterBL, RequestInfo requestInfo)
        {
            this.httpContextAccessor = httpContextAccessor;
            var serviceProvider = this.httpContextAccessor.HttpContext?.RequestServices;
            this.requestInfo = requestInfo;
            this._masterBL = masterBL;
        }

       

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = new SideBarVM(httpContextAccessor);

            await model.LoadAllAData();
            model.SideBar = await _masterBL.GetAdminService<SrvSideBarBL>().GetMenuSideBarList();
            var relativePath = model.CurrentLanguage == "ar" ? _masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.AdminLogoAr) : _masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.AdminLogoEn);
            ViewBag.LogoURLSetting = relativePath;
            return View(model);
        }
    }
}
