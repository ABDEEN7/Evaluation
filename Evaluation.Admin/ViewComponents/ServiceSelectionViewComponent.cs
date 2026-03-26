using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.ViewComponents
{
    public class ServiceSelectionViewComponent : ViewComponent
    {


        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RequestInfo requestInfo;
        private readonly MasterBL _masterBL;
        public ServiceSelectionViewComponent(IHttpContextAccessor httpContextAccessor,RequestInfo requestInfo, MasterBL masterBL)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.requestInfo = requestInfo;
            _masterBL = masterBL;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool ShowServiceDefault=true)
        {
            var model = new ServiceSelectionVM(httpContextAccessor);

            await model.LoadAllAData();
            model.Departments = await _masterBL.GetAdminService<SrvBaseBL>().GetAllDepartments();
            model.SystemModules = await _masterBL.GetAdminService<SrvBaseBL>().GetAllSystemModules();
            model.Services = await _masterBL.GetAdminService<SrvBaseBL>().GetAllServices();
            model.ShowServiceDefault = ShowServiceDefault;
            return View(model);
        }
    }
}
