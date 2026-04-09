using Azure.Core;
using Evaluation.Api.Extensions;
using Evaluation.API.ActionFilter;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.EvaluationForm;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;


[Route("api/[controller]/{depRouting}/[action]")]
[ApiController]
public class EvaluationFormController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public EvaluationFormController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_EVALFORMS })]
    public async Task<IActionResult> GetAllEvalForm(int Page = 1)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetEvaluationForm(Page));
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_FORMITEMS })]
    public async Task<IActionResult> GetAllEvalFormItems(Guid EvalformId)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetAllEvalFormItems(EvalformId));
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_FORMSCOPES })]
    public async Task<IActionResult> GetAllFormScope(Guid formIdValue)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetAllFormScope(formIdValue));
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_FORMITEMS })]
    public async Task<IActionResult> GetAllFormItemsFromDepartment(Guid EvalformId)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetAllFormItemsFromDepartment(EvalformId));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_EVALFORMS })]
    public async Task<IActionResult> SaveEvaluationForm()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveEvaluationForm(request!));
    }

    [HttpPost]
   //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_EVALFORMS })]
    public async Task<IActionResult> UpdateEvaluationForm()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateEvaluationForm(request!));
    }
    [HttpPost]
   // [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_EVALFORMS })]
    public async Task<IActionResult> DeleteEvaluationForm(Guid Id)
    {

        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().DeleteEvaluationForm(Id!));

    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_FORMITEMS })]
    public async Task<IActionResult> SaveEvaluationFormItem()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormItemDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveEvaluationFormItem(request!));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_FORMITEMS })]
    public async Task<IActionResult> UpdateEvaluationFormItem()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormItemDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateEvaluationFormItem(request!));
    }
    [HttpPost]
     //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_FORMITEMS })]
    public async Task<IActionResult> DeleteEvaluationFormItem(Guid Id)
    {

        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().DeleteEvaluationFormItem(Id!));

    }
    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_SUBFORMITEMS })]
    public async Task<IActionResult> SaveEvaluationSubFormItem()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormSubItemDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveEvaluationSubFormItem(request!));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_SUBFORMITEMS })]
    public async Task<IActionResult> UpdateEvaluationSubFormItem()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormSubItemDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateEvaluationSubFormItem(request!));
    }
    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_SUBFORMITEMS })]
    public async Task<IActionResult> DeleteEvaluationSubFormItem(Guid Id)
    {

        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().DeleteEvaluationSubFormItem(Id!));

    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_FORMSCOPES })]
    public async Task<IActionResult> SaveFormScope()
    {
        var request = Request.Form["request"][0]?.StringToObject<FormScopeDTO>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveFormScope(request!));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_FORMSCOPES })]
    public async Task<IActionResult> UpdateFormScope()
    {
        var request = Request.Form["request"][0]?.StringToObject<FormScopeDTO>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateFormScope(request!));
    }
    [HttpPost]
    // [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_FORMSCOPES })]
    public async Task<IActionResult> DeleteFormScope(Guid Id)
    {

        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().DeleteFormScope(Id!));

    }

}
