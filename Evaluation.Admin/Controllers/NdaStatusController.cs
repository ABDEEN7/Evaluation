using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class NdaStatusController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public NdaStatusController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NDAStatus })]
    public async Task<IActionResult> Index()
    {
        var model = new NdaStatusVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminNDAStatus },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_NDAStatus });
        var property = typeof(NdaStatusDto).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_NDAStatus })]
    public async Task<IActionResult> GetAllNDAStatus(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvNdaStatusBL>().GetNdaStatusList(page, pageSize);
        return Ok(response);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_NDAStatus })]
    public async Task<IActionResult> SaveNDAStatus()
    {

        var request = Request.Form["request"][0]?.StringToObject<NdaStatusDto>();

        var result = new NdaStatusDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NDAStatus);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvNdaStatusBL>().SaveNdaStatus(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_NDAStatus })]
    public async Task<IActionResult> DeleteNDAStatus(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvNdaStatusBL>()
            .DeleteNdaStatusAsync(id);
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_NDAStatus })]
    public async Task<IActionResult> UpdateNDAStatus()
    {
        var request = Request.Form["request"][0]?.StringToObject<NdaStatusDto>();
        var result = new NdaStatusDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_NDAStatus);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvNdaStatusBL>().UpdateNdaStatus(request!);
        }
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.UPDATE_ORDER_ADMIN_NDAStatus })]
    public async Task<IActionResult> UpdateNDAStatusOrder()
    {
        var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
        var result = await masterBL.GetAdminService<SrvNdaStatusBL>().UpdateNdaStatusOrderAsync(model!);
        return Ok(result);
    }
}