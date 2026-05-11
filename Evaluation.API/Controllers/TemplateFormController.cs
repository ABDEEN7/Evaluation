using Evaluation.Api.Extensions;
using Evaluation.API.ActionFilter;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.Admin;
using Evaluation.Services.BusinessLayer.API.EvaluationForm;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]/{depRouting}/[action]")]
public class TemplateFormController : ControllerBase
{
    private readonly MasterBL _masterBl;
    public TemplateFormController(MasterBL masterBl)
    {
        _masterBl = masterBl;
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_TemplateFormS })]
    public async Task<IActionResult> GetAllTemplateForm([FromQuery] SearchTemplateForm Page)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetEvaluationForm(Page));
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_FORMITEMS })]
    public async Task<IActionResult> GetAllTemplateFormItems(Guid TemplateFormId)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetAllTemplateFormItems(TemplateFormId));
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_FORMSCOPES })]
    public async Task<IActionResult> GetAllFormScope(Guid formIdValue)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetAllFormScope(formIdValue));
    }

    [HttpGet]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.VIEW_WEB_FORMITEMS })]
    public async Task<IActionResult> GetAllFormItemsFromDepartment(Guid TemplateFormId)
    {
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().GetAllFormItemsFromDepartment(TemplateFormId));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_TemplateFormS })]
    public async Task<IActionResult> SaveTemplateForm()
    {
        var request = Request.Form["request"][0]?.StringToObject<TemplateFormDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveEvaluationForm(request!));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_TemplateFormS })]
    public async Task<IActionResult> UpdateTemplateForm()
    {
        var request = Request.Form["request"][0]?.StringToObject<TemplateFormDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateEvaluationForm(request!));
    }
    [HttpPost]
    // [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_TemplateFormS })]
    public async Task<IActionResult> DeleteTemplateForm(Guid Id)
    {

        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().DeleteEvaluationForm(Id!));

    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.ADD_WEB_FORMITEMS })]
    public async Task<IActionResult> SaveTemplateFormItem()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormItemDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveEvaluationFormItem(request!));
    }

    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.EDIT_WEB_FORMITEMS })]
    public async Task<IActionResult> UpdateTemplateFormItem()
    {
        var request = Request.Form["request"][0]?.StringToObject<EvaluationFormItemDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateEvaluationFormItem(request!));
    }
    [HttpPost]
    //[CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.WebPermissions.DELETE_WEB_FORMITEMS })]
    public async Task<IActionResult> DeleteTemplateFormItem(Guid Id)
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
    [HttpGet]
    public async Task<IActionResult> GetEvalFormItemLists(Guid formId)
    {
        Dictionary<string, object> response = new Dictionary<string, object>();
        var PartyTypeList = await _masterBl.GetApiService<EvaluationFormBL>().GetPartyTypeListAsync();
        var FormItemList = await _masterBl.GetApiService<EvaluationFormBL>().GetFormItemListAsync(formId);
        var CalcMethodsList = await _masterBl.GetApiService<EvaluationFormBL>().GetCalcMethodsListAsync();
        response.Add("PartyTypeList", PartyTypeList);
        response.Add("FormItemList", FormItemList);
        response.Add("CalcMethodsList", CalcMethodsList);
        return Ok(new ResponseEntity(response));
    }
   
    [HttpGet]
    public async Task<IActionResult> GetAllFormItemConfig(Guid? evalFormId)
    {
        var data = await _masterBl.GetApiService<EvaluationFormBL>().GetAllFormItemConfig(evalFormId);
        return Ok(data);
    }
    [HttpPost]
    public async Task<IActionResult> SaveFormItemConfig()
    {
        var request = Request.Form["request"][0]?.StringToObject<List<CreateFormItemConfigDto>>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().SaveFormItemConfig(request!));
    }
    [HttpPost]
    public async Task<IActionResult> UpdateFormItemConfig()
    {
        var request = Request.Form["request"][0]?.StringToObject<CreateFormItemConfigDto>();
        return Ok(await _masterBl.GetApiService<EvaluationFormBL>().UpdateFormItemConfig(request!));
    }
}
