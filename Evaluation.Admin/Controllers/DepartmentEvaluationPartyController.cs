using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.Planing;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class DepartmentEvaluationPartyController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public DepartmentEvaluationPartyController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPARTMENTEVALUATIONPARTY })]
    public async Task<IActionResult> Index()
    {
        var model = new DepartmentEvaluationPartyVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDepartmentEvaluationParty },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENTEVALUATIONPARTY });
        var property = typeof(DepartmentEvaluationParty).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_DEPARTMENTEVALUATIONPARTY })]
    public async Task<IActionResult> GetAllDepartmentEvaluationParty(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvDepartmentEvaluationPartyBL>().GetDepartmentEvaluationPartyList(page, pageSize);
        return Ok(response);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENTEVALUATIONPARTY })]
    public async Task<IActionResult> SaveDepartmentEvaluationParty()
    {

        var request = Request.Form["request"][0]?.StringToObject<DepartmentEvaluationPartyDto>();

        var result = new DepartmentEvaluationPartyDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENTEVALUATIONPARTY);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvDepartmentEvaluationPartyBL>().SaveDepartmentEvaluationParty(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_DEPARTMENTEVALUATIONPARTY })]
    public async Task<IActionResult> DeleteDepartmentEvaluationParty(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvDepartmentEvaluationPartyBL>()
            .DeleteDepartmentEvaluationPartyAsync(id);
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEPARTMENTEVALUATIONPARTY })]
    public async Task<IActionResult> UpdateDepartmentEvaluationParty()
    {
        var request = Request.Form["request"][0]?.StringToObject<DepartmentEvaluationPartyDto>();
        var result = new DepartmentEvaluationPartyDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DEPARTMENTEVALUATIONPARTY);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvDepartmentEvaluationPartyBL>().UpdateDepartmentEvaluationParty(request!);
        }
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_DEPARTMENTEVALUATIONPARTY })]
    public async Task<IActionResult> UpdateDepartmentEvaluationPartyOrder()
    {
        var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
        var result = await masterBL.GetAdminService<SrvDepartmentEvaluationPartyBL>().UpdateDepartmentEvaluationPartyOrderAsync(model!);
        return Ok(result);
    }
}
