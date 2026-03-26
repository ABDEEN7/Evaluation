using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Models;
using Evaluation.Api.Extensions;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class NdaStatusDepartmentController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public NdaStatusDepartmentController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NDA_STATUS_DEPARTMENT })]
    public async Task<IActionResult> Index()
    {
        var model = new NdaStatusDepartmentVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminNDAStatusDepartment },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_NDA_STATUS_DEPARTMENT });
        var property = typeof(NdaStatusDto).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NDA_STATUS_DEPARTMENT })]
    public async Task<IActionResult> GetAllNdaStatusDepartment(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvNdaStatusDepartmentBL>().GetNdaStatusDepartmentList(page, pageSize);
        return Ok(response);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_NDA_STATUS_DEPARTMENT })]
    public async Task<IActionResult> SaveNdaStatusDepartment()
    {

        NdaStatusDepartmentDto request = Request.Form["request"][0]?.StringToObject<NdaStatusDepartmentDto>();

        var result = new NdaStatusDepartmentDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NDAStatus);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvNdaStatusDepartmentBL>().SaveNdaStatusDepartment(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_NDA_STATUS_DEPARTMENT })]
    public async Task<IActionResult> DeleteNdaStatusDepartment(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvNdaStatusDepartmentBL>()
            .DeleteNdaStatusDepartmentAsync(id);
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_NDA_STATUS_DEPARTMENT })]
    public async Task<IActionResult> UpdateNdaStatusDepartment()
    {
        var request = Request.Form["request"][0]?.StringToObject<NdaStatusDepartmentDto>();
        var result = new NdaStatusDepartmentDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NDAStatus);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvNdaStatusDepartmentBL>().UpdateNdaStatusDepartment(request!);
        }
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_NDA_STATUS_DEPARTMENT })]
    public async Task<IActionResult> UpdateNdaStatusDepartmentOrder()
    {
        var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
        var result = await masterBL.GetAdminService<SrvNdaStatusDepartmentBL>().UpdateNdaStatusDepartmentOrderAsync(model!);
        return Ok(result);
    }
}