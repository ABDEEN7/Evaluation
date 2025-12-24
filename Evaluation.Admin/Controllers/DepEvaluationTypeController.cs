using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Admin.Controllers;

public class DepEvaluationTypeController : Controller
{
    private readonly MasterBL masterBL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public DepEvaluationTypeController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
    {
        this.masterBL = masterBL;
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> Index()
    {
        var model = new DepEvaluationTypeVM(httpContextAccessor);
        await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminDepEvaluationType },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_DepEvaluationType });
        var property = typeof(DepEvaluationType).GetProperty("OrderNo");
        model.containsOrderNo = property != null ? true : false;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepEvaluationType(int page = 1)
    {
        var pageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>
            ().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
        var response = await masterBL.GetAdminService<SrvDepEvaluationTypeBL>().GetDepEvaluationTypeList(page, pageSize);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> SaveDepEvaluationType()
    {

        var request = Request.Form["request"][0]?.StringToObject<DepEvaluationTypeDto>();

        var result = new DepEvaluationTypeDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_DepEvaluationType);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvDepEvaluationTypeBL>().SaveDepEvaluationType(request!);
        }
        return Ok(result);

    }
    [HttpPost]
    public async Task<IActionResult> DeleteDepEvaluationType(Guid id)
    {
        var result = await masterBL
            .GetAdminService<SrvDepEvaluationTypeBL>()
            .DeleteDepEvaluationTypeAsync(id);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDepEvaluationType()
    {
        var request = Request.Form["request"][0]?.StringToObject<DepEvaluationTypeDto>();
        var result = new DepEvaluationTypeDto();
        bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.EDIT_ADMIN_DepEvaluationType);
        if (validateObject)
        {
            result = await masterBL.GetAdminService<SrvDepEvaluationTypeBL>().UpdateDepEvaluationType(request!);
        }
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDepEvaluationTypeOrder()
    {
        var model = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
        var result = await masterBL.GetAdminService<SrvJobTitleBL>().UpdateJobTitleOrderAsync(model!);
        return Ok(result);
    }
}
