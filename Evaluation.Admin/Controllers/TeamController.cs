using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.Planing.TeamsModule;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class TeamController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public TeamController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_TEAM })]
    public async Task<IActionResult> Index()
    {
        var model = new TeamVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminTeam, ConstantKeys.AdminPages.AdminUserTeamScope },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_TEAM, ConstantKeys.AdminPermission.ADD_ADMIN_USERTEAMSCOPE });
        var property = typeof(Team).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }

    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_TEAM })]
    public async Task<IActionResult> GetAllTeam(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvTeamBL>().GetTeamList(page, pageSize);
        return Ok(response);
    }

    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_USERTEAMSCOPE })]
    public async Task<IActionResult> GetAllUserTeamScope(Guid TeamId)
    {

        var response = await masterBL.GetAdminService<SrvTeamBL>().GetAllUserTeamScopeList(TeamId);
        return Ok(response);
    }

    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_TEAM })]
    public async Task<IActionResult> GetUserScope()
    {

        Dictionary<string, object> response = new Dictionary<string, object>();
        var UserList = await masterBL.GetAdminService<SrvTeamBL>().GetUserList();
        var ScopeList = await masterBL.GetAdminService<SrvTeamBL>().GetScopeList();
        response.Add("UserList", UserList);
        response.Add("ScopeList", ScopeList);
        return Ok(new ResponseEntity(response));
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_TEAM })]
    public async Task<IActionResult> SaveTeam()
    {

        var request = Request.Form["request"][0]?.StringToObject<TeamDto>();

        var result = new TeamDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_TEAM);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvTeamBL>().SaveTeam(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_TEAM})]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvTeamBL>()
            .DeleteTeamAsync(id);
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_TEAM })]
    public async Task<IActionResult> UpdateTeam()
    {
        var request = Request.Form["request"][0]?.StringToObject<TeamDto>();
        var result = new TeamDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_TEAM);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvTeamBL>().UpdateTeam(request!);
        }
        return Ok(result);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_USERTEAMSCOPE })]
    public async Task<IActionResult> UpdateUserTeam()
    {
        var request = Request.Form["request"][0]?.StringToObject<UserTeamScopeDTO>();
        var result = new UserTeamScopeDTO();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.EDIT_ADMIN_USERTEAMSCOPE);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvTeamBL>().UpdateUserTeamScope(request!);
        }
        return Ok(result);
    }

    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_USERTEAMSCOPE })]
    public async Task<IActionResult> DeleteUserTeamScope(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvTeamBL>()
            .DeleteUserTeamScopeTeamAsync(id);
        return Ok(result);
    }

}
