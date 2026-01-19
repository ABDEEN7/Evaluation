
using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

[Authorize]
public class ScopeTypeController : Controller
{
    private readonly UserInfo userInfoSession;
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ScopeTypeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {

        this.userInfoSession = userInfoSession;
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;

    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCOPE_TYPE })]
    public async Task<IActionResult> Index()
    {
        var model = new ScopeTypeVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminScopeType },
            new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_TYPE },
            new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
        });

        var property = typeof(ScopeType).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }


    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SCOPE_TYPE })]
    public async Task<IActionResult> GetAllScopeType(int Page = 1)
    {
        var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvScopeTypeBL>().GetScopeTypeList(Page, PageSize);
        return Ok(response);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_TYPE })]
    public async Task<IActionResult> SaveScopeType()
    {

        var request = Request.Form["request"][0]?.StringToObject<ScopeTypeDTO>();
        var result = new ScopeTypeDTO();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_TYPE);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvScopeTypeBL>().SaveScopeType(request!);

        }
        return Ok(new ResponseEntity(result));

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SCOPE_TYPE })]
    public async Task<IActionResult> UpdateScopeType()
    {

        var request = Request.Form["request"][0]?.StringToObject<ScopeTypeDTO>();
        var result = new ScopeTypeDTO();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_SCOPE_TYPE);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvScopeTypeBL>().UpdateScopeType(request!);
        }
        return Ok(new ResponseEntity(result));

    }


    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SCOPE_TYPE })]
    public async Task<IActionResult> DeleteScopeType(Guid Id)
    {

        var result = await masterBL.GetAdminService<SrvScopeTypeBL>().DeleteScopeType(Id);
        return Ok(result);

    }
}
