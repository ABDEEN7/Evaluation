using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.Master;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class JobTitleController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public JobTitleController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_JOBTITLE })]
    public async Task<IActionResult> Index()
    {
        var model = new JobTitleVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminJobTitle },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_JOBTITLE });
        var property = typeof(JobTitle).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_JOBTITLE })]
    public async Task<IActionResult> GetAllJobTitle(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvJobTitleBL>().GetJobTitleList(page, pageSize);
        return Ok(response);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_JOBTITLE })]
    public async Task<IActionResult> SaveJobTitle()
    {

        var request = Request.Form["request"][0]?.StringToObject<JobTitleDto>();

        var result = new JobTitleDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_JOBTITLE);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvJobTitleBL>().SaveJobTitle(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_JOBTITLE })]
    public async Task<IActionResult> DeleteJobTitle(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvJobTitleBL>()
            .DeleteJobTitleAsync(id);
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_JOBTITLE })]
    public async Task<IActionResult> UpdateJobTitle()
    {
        var request = Request.Form["request"][0]?.StringToObject<JobTitleDto>();
        var result = new JobTitleDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.EDIT_ADMIN_JOBTITLE);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvJobTitleBL>().UpdateJobTitle(request!);
        }
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateJobTitleOrder()
    {
        var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
        var result = await masterBL.GetAdminService<SrvJobTitleBL>().UpdateJobTitleOrderAsync(model!);
        return Ok(result);
    }
}
