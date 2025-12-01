using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class ServiceActionController : Controller
    {

        private readonly MasterBL masterBL;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;
        public ServiceActionController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor, AzureBlobStorageService blobService)
        {
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            _blobService = blobService;

        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION })]
        public async Task<IActionResult> Index()
        {
            var model = new ServiceActionVM(httpContextAccessor);

            await model.LoadAllAData(
                new string[] { ConstantKeys.AdminPages.AdminEvaluationAction,
                ConstantKeys.AdminPages.AdminActionFieldAttribute,
                ConstantKeys.AdminPages.AdminActionStatusConfiguration ,
                ConstantKeys.AdminPages.AdminActionStatusConfigurationNotification,
                ConstantKeys.AdminPages.AdminActionCondition  },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTION ,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTION_FIELD_ATTRIBUTE,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONCONDITION}
                );

            var ServicesTask = masterBL.GetAdminService<SrvServiceActionBL>().GetServicesForServiceAction();
            var ActionTypesTask = masterBL.GetAdminService<SrvServiceActionBL>().GetServiceActionTypes();


            model.Services = await ServicesTask;
            model.ActionTypes = await ActionTypesTask;

           
            var property = typeof(ServiceAction).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTION })]
        public async Task<IActionResult> GetAllEvaluationActionList(Guid serviceId, int Page = 1)
        {
            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvServiceActionBL>().GetAllServiceActionList(serviceId, Page, PageSize);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION })]
        public async Task<IActionResult> GetActionConditionByAction(Guid actionId)
        {

            var response = await masterBL.GetAdminService<SrvServiceActionBL>().GetActionConditionByActionList(actionId);
            return Ok(response);
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllSystemField(Guid serviceid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var SystemField = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllSystemField(serviceid);
            response.Add("SystemField", SystemField);
            return Ok(new ResponseEntity(response));
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION })]
        public async Task<IActionResult> GetTemplateDocs_PartyTypes_List(Guid serviceId)
        {

            var response = new List<DropdownItem>();

            var templateDocsListTask = masterBL.GetAdminService<SrvServiceActionBL>().GetTemplateDocsList(serviceId);
            var partyTypesListTask = masterBL.GetAdminService<SrvServiceActionBL>().GetPartyTypesList();

            var templateDocsList = await templateDocsListTask;
            var partyTypesList = await partyTypesListTask;

            response.AddRange(templateDocsList);    
            response.AddRange(partyTypesList);    
            return Ok(response);
        }
        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION })]
        public async Task<IActionResult> GetAllpartyType()
        {

            var response = new List<DropdownItem>();

            var partyTypesList =await masterBL.GetAdminService<SrvServiceActionBL>().GetPartyTypesList();
            response.AddRange(partyTypesList);
            return Ok(response);
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION })]
        public async Task<IActionResult> GetEvaluationActionDetails(Guid actionId)
        {
            var result = await masterBL.GetAdminService<SrvServiceActionBL>().GetServiceActionDetails(actionId);

            return Ok(result);
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACTION })]
        public async Task<IActionResult> DeleteEvaluationAction(Guid id)
        {
            //throw new NotImplementedException();    
            var result = await masterBL.GetAdminService<SrvServiceActionBL>().DeleteServiceAction(id);
            return Ok(result);
        }



        [HttpPost()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTION })]
        public async Task<IActionResult> SaveEvaluationAction()
        {
            var model = Request.Form["request"][0]?.StringToObject<ManageServiceActionDTO>();
            var result = await masterBL.GetAdminService<SrvServiceActionBL>().SaveServiceAction(model!);
            return Ok(result);
        }


        [HttpPost()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACTION })]
        public async Task<IActionResult> UpdateEvaluationAction()
        {
            var model = Request.Form["request"][0]?.StringToObject<ManageServiceActionDTO>();
            var result = await masterBL.GetAdminService<SrvServiceActionBL>().UpdateServiceAction(model!);
            return Ok(result);
        }

        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION_FIELD })]
        public async Task<IActionResult> GetActionFieldTree(Guid actionId)
        {
            var result = await masterBL.GetAdminService<SrvServiceActionBL>().GetActionFieldTree(actionId);
            return Ok(result);
        }


        #region  field Attribute

        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION_FIELD_ATTRIBUTE })]
        [HttpGet]
        public async Task<IActionResult> GetAllAttribute()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Attribute = await masterBL.GetAdminService<SrvFormGroupBL>().GetAttributeList();
            response.Add("Attribute", Attribute);
            return Ok(new ResponseEntity(response));


        }

        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_ACTION_FIELD_ATTRIBUTE })]
        [HttpGet]
        public async Task<IActionResult> GetAllActionFieldAttribute(Guid actionfieldid)
        {
            var response= await masterBL.GetAdminService<SrvServiceActionBL>().GetActionFieldAttributeList(actionfieldid);
            return Ok(response);


        }
        [HttpPost()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.UPDATE_ADMIN_ACTION_FIELD })]
        public async Task<IActionResult> UpdateActionFieldList([FromBody] UpdateActionFieldDTO model)
        {
            bool result = await masterBL.GetAdminService<SrvServiceActionBL>().UpdateActionFieldStepList(model);
            return Ok(result);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTION_FIELD_ATTRIBUTE })]
        public async Task<IActionResult> SaveActionFieldAttribute()
        {


            var result = new ActionFieldAttributeDTO();
            var model = Request.Form["request"][0]?.StringToObject<ActionFieldAttributeDTO>();


            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_ATTRIBUTE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceActionBL>().SaveActionFieldAttribute(model!);
            }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACTION_FIELD_ATTRIBUTE })]
        public async Task<IActionResult> UpdateActionFieldAttribute()
        {

            var result = new ActionFieldAttributeDTO();
            var model = Request.Form["request"][0]?.StringToObject<ActionFieldAttributeDTO>();


            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_ATTRIBUTE);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceActionBL>().UpdateActionFieldAttribute(model!);
            }
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACTION_FIELD_ATTRIBUTE })]
        public async Task<IActionResult> DeleteActionFieldAttribute(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvServiceActionBL>().DeleteActionFieldAttribute(Id);
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONCONDITION })]
        public async Task<IActionResult> SaveActionCondition()
        {


            var result = new ActionConditionDTO();
            var model = Request.Form["request"][0]?.StringToObject<ActionConditionDTO>();


            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONCONDITION);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceActionBL>().SaveActionCondition(model!);
            }
            return Ok(result);

        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_ACTIONCONDITION })]
        public async Task<IActionResult> UpdateActionCondition()
        {

            var result = new ActionConditionDTO();
            var model = Request.Form["request"][0]?.StringToObject<ActionConditionDTO>();


            bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONCONDITION);
            if (validateObject)
            {
                result = await masterBL.GetAdminService<SrvServiceActionBL>().UpdateActionCondition(model!);
            }
            return Ok(result);

        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_ACTIONCONDITION })]
        public async Task<IActionResult> DeleteActionCondition(Guid Id)
        {

            var result = await masterBL.GetAdminService<SrvServiceActionBL>().DeleteActionCondition(Id);
            return Ok(result);

        }
        #endregion


    }
}
