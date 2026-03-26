using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Planing;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class EvaluationPartyController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public EvaluationPartyController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALUATIONPARTIES })]
    public async Task<IActionResult> Index()
    {
        var model = new EvaluationPartyVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminEvaluationParties },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES });
        var property = typeof(EvaluationParty).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }
    [HttpGet]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_EVALUATIONPARTIES })]
    public async Task<IActionResult> GetAllEvaluationParty(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvEvaluationPartyBL>().GetEvaluationPartyList(page, pageSize);
        return Ok(response);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES })]
    public async Task<IActionResult> SaveEvaluationParty()
    {

        var request = Request.Form["request"][0]?.StringToObject<EvaluationPartyDto>();

        var result = new EvaluationPartyDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvEvaluationPartyBL>().SaveEvaluationParty(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_EVALUATIONPARTIES })]
    public async Task<IActionResult> DeleteEvaluationParty(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvEvaluationPartyBL>()
            .DeleteEvaluationPartyAsync(id);
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALUATIONPARTIES })]
    public async Task<IActionResult> UpdateEvaluationParty()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationPartyDto>();
        var result = new EvaluationPartyDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_EVALUATIONPARTIES);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvEvaluationPartyBL>().UpdateEvaluationParty(request!);
        }
        return Ok(result);
    }
    [HttpPost]
    [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_EVALUATIONPARTIES })]
    public async Task<IActionResult> UpdateEvaluationPartyOrder()
    {
        var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
        var result = await masterBL.GetAdminService<SrvEvaluationPartyBL>().UpdateEvaluationPartyOrderAsync(model!);
        return Ok(result);
    }
}
