using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Org;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

[Authorize]
public class OrgAcademicYearController : Controller
{
    private readonly UserInfo userInfoSession;
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public OrgAcademicYearController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {

        this.userInfoSession = userInfoSession;
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;

    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ORG_ACADEMIC_YEAR })]
    public async Task<IActionResult> Index()
    {
        var model = new OrgAcademicYearVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminOrgAcademicYear },
            new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ORG_ACADEMIC_YEAR },
            new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
        });

        var property = typeof(OrgAcademicYear).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }


    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ORG_ACADEMIC_YEAR })]
    public async Task<IActionResult> GetAllOrgAcademicYear(int Page = 1)
    {
        var PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvOrgAcademicYearBL>().GetOrgAcademicYearList(Page, PageSize);
        return Ok(response);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ORG_ACADEMIC_YEAR })]
    public async Task<IActionResult> SaveOrgAcademicYear()
    {

        var request = Request.Form["request"][0]?.StringToObject<OrgAcademicYearDTO>();
        var result = new OrgAcademicYearDTO();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ORG_ACADEMIC_YEAR);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvOrgAcademicYearBL>().SaveOrgAcademicYear(request!);

        }
        return Ok(new ResponseEntity(result));

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ORG_ACADEMIC_YEAR })]
    public async Task<IActionResult> UpdateOrgAcademicYear()
    {

        var request = Request.Form["request"][0]?.StringToObject<OrgAcademicYearDTO>();
        var result = new OrgAcademicYearDTO();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_ORG_ACADEMIC_YEAR);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvOrgAcademicYearBL>().UpdateOrgAcademicYear(request!);
        }
        return Ok(new ResponseEntity(result));

    }


    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ORG_ACADEMIC_YEAR })]
    public async Task<IActionResult> DeleteOrgAcademicYear(Guid Id)
    {

        var result = await masterBL.GetAdminService<SrvOrgAcademicYearBL>().DeleteOrgAcademicYear(Id);
        return Ok(result);

    }
}
