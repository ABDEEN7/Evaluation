using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

[Authorize]
public class ControlValidationController : Controller
{

    private readonly UserInfo userInfoSession;
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ControlValidationController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {

        this.userInfoSession = userInfoSession;
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;

    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_CONTROLVALIDATION })]
    public async Task<IActionResult> Index()
    {
        var model = new ControlValidationVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminControlValidation },
            new string[] { ConstantKeys.AdminPermission.EDIT_ADMIN_CONTROLVALIDATION },
            new string[] {
                 ConstantKeys.SystemSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.SystemSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.SystemSettings.ADMIN_FILE_COUNT
        });

        var property = typeof(ControlValidation).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_CONTROLVALIDATION })]

    public async Task<IActionResult> GetAllControlValidation([FromBody] AdminSearchDTO request)
    {

        if (null == request)
        {
            request = new AdminSearchDTO
            {
                PageNum = 0,
                PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.SystemSettings.ADMIN_PAGE_SIZE)),
            };
        }
        else
        {
            request.PageNum = request.PageNum ?? 0;
            request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.SystemSettings.ADMIN_PAGE_SIZE));
        }
        request.PageNum = request.PageNum ?? 0;
        request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.SystemSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvControlValidationBL>().GetControlValidationList(request);
        return Ok(response);
    }



    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_CONTROLVALIDATION })]
    public async Task<IActionResult> UpdateControlValidation()
    {

        var request = Request.Form["request"][0]?.StringToObject<ControlValidationDTO>();
        var result = new ControlValidationDTO();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.EDIT_ADMIN_CONTROLVALIDATION);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvControlValidationBL>().UpdateControlValidation(request!);
        }
        return Ok(new ResponseEntity(result));

    }




}
