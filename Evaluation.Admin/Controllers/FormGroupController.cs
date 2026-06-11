using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class FormGroupController : Controller
    {

        private readonly MasterBL masterBL;

        private readonly IHttpContextAccessor httpContextAccessor;

        public FormGroupController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {


            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;


        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORMGROUP })]
        public async Task<IActionResult> Index()
        {
            var model = new FormGroupVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminFormGroup, ConstantKeys.AdminPages.AdminField,
                ConstantKeys.AdminPages.AdminFieldAttribute,
                ConstantKeys.AdminPages.AdminFieldCondition,
            },
                 new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORMGROUP, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD,
                 ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_ATTRIBUTE,
                 ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_CONDITION,
                  });
            return View(model);
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORMGROUP })]
        [HttpGet]
        public async Task<IActionResult> GetAllFormGroup(Guid serviceId,string FormGrouptype)
        {
           
            var response= await masterBL.GetAdminService<SrvFormGroupBL>().GetFormGroupList(serviceId,FormGrouptype);
            return Ok(response);
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORMGROUP })]
        [HttpGet]
        public async Task<IActionResult> GetFormGroupListByTypeList(Guid serviceid)
        {

            var response= await masterBL.GetAdminService<SrvFormGroupBL>().GetFormGroupListByTypeList(serviceid);
            return Ok(response);
        }

        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        [HttpGet]
        public async Task<IActionResult> GetAllFormGroupFields(Guid FormGroupId)
        {
            var response= await masterBL.GetAdminService<SrvFormGroupBL>().GetAllFormGroupFields(FormGroupId);
            return Ok(response);


        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_ATTRIBUTE })]
        [HttpGet]
        public async Task<IActionResult> GetAllFieldAttribute(Guid FieldId)
        {
            var response= await masterBL.GetAdminService<SrvFormGroupBL>().GetFieldAttributeValueList(FieldId);
            return Ok(response);


        }

        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_ATTRIBUTE })]
        [HttpGet]
        public async Task<IActionResult> GetAllAttribute()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Attribute = await masterBL.GetAdminService<SrvFormGroupBL>().GetAttributeList();
            response.Add("Attribute", Attribute);
            return Ok(new ResponseEntity(response));


        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_CONDITION })]
        [HttpGet]
        public async Task<IActionResult> GetAllFieldCondition(Guid FieldId)
        {
            var response= await masterBL.GetAdminService<SrvFormGroupBL>().GetAllFieldConditionList(FieldId);
            return Ok(response);


        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FORMGROUP })]
        public async Task<IActionResult> SaveFormGroup()
        {
          
               
                var result = new FormGroupDTO();
                var model = Request.Form["request"][0]?.StringToObject<FormGroupDTO>();
               

                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FORMGROUP);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().SaveFormGroup(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FORMGROUP })]
        public async Task<IActionResult> UpdateFormGroup()
        {
            
                var result = new FormGroupDTO();
                var model = Request.Form["request"][0]?.StringToObject<FormGroupDTO>();

              
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FORMGROUP);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().UpdateFormGroup(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FORMGROUP })]
       
        public async Task<IActionResult> UpdateFormGroupOrder([FromBody] List<FormGroupDTO> formGroups)
        {
            var response = await masterBL.GetAdminService<SrvFormGroupBL>().UpdateFormGroupOrder(formGroups, formGroups[0].ServiceId);
            return Ok(new ResponseEntity(response));
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_FORMGROUP })]
        public async Task<IActionResult> DeleteFormGroup(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvFormGroupBL>().DeleteFormGroup(Id);
                return Ok(result);
            
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FORMGROUP })]
        public async Task<IActionResult> GetAllFormGroupType()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var FormGroupType = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllFormGroupType();
            response.Add("FormGroupType", FormGroupType);
            return Ok(new ResponseEntity(response));
        }

        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllFieldType()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var FieldType = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllFieldType();
            response.Add("FieldType", FieldType);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllFieldInfoType()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var fieldInfoType = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllFieldInfoTypes();
            response.Add("FieldInfoType", fieldInfoType);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllSystemField(Guid systemmoduleid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var SystemField = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllSystemField(systemmoduleid);
            response.Add("SystemField", SystemField);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllPartyType(Guid systemmoduleid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var PartyType = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllPartyType(systemmoduleid);
            response.Add("PartyType", PartyType);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllDropDownType()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var DropDownType = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllDropDownType();
            response.Add("DropDownType", DropDownType);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD })]
        public async Task<IActionResult> GetAllParentDropDownField(Guid serviceid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var ParentDropDownField = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllParentDropDownField(serviceid);
            response.Add("ParentDropDownField", ParentDropDownField);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_CONDITION })]
        public async Task<IActionResult> GetAllField(Guid serviceid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Field = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllField(serviceid);
            response.Add("Field", Field);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_CONDITION })]
        public async Task<IActionResult> GetAllEvalForm(Guid systemmoduleid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var EvalForm = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllEvalForm(systemmoduleid);
            response.Add("EvalForm", EvalForm);
            return Ok(new ResponseEntity(response));
        }
        [HttpGet]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_CONDITION })]
        public async Task<IActionResult> GetAllFieldFormGroupList(Guid serviceid)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var Field = await masterBL.GetAdminService<SrvFormGroupBL>().GetAllFieldFormGroupList(serviceid);
            response.Add("Field", Field);
            return Ok(new ResponseEntity(response));
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_FIELD_CONDITION })]
        [HttpGet]
        public async Task<IActionResult> GetFieldDropDownValues(Guid DropDownTypeId)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var FieldDropDownValueList = await masterBL.GetAdminService<SrvFormGroupBL>().GetFieldDropDownValueList(DropDownTypeId);
            response.Add("FieldDropDownValueList", FieldDropDownValueList);
            return Ok(new ResponseEntity(response));
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FIELD })]
        public async Task<IActionResult> SaveField()
        {
           

                var result = new FieldDTO();
                var model = Request.Form["request"][0]?.StringToObject<FieldDTO>();


                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().SaveField(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FIELD })]
        public async Task<IActionResult> UpdateField()
        {
           
                var result = new FieldDTO();
                var model = Request.Form["request"][0]?.StringToObject<FieldDTO>();


                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().UpdateField(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_FIELD })]
        public async Task<IActionResult> DeleteField(Guid Id)
        {
            
                var result = await masterBL.GetAdminService<SrvFormGroupBL>().DeleteField(Id);
                return Ok(result);
           
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_ATTRIBUTE })]
        public async Task<IActionResult> SaveFieldAttribute()
        {
            

                var result = new FieldAttributeValueDTO();
                var model = Request.Form["request"][0]?.StringToObject<FieldAttributeValueDTO>();


                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_ATTRIBUTE);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().SaveFieldAttribute(model!);
                }
                return Ok(new ResponseEntity(result));
            
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FIELD_ATTRIBUTE })]
        public async Task<IActionResult> UpdateFieldAttribute()
        {
           
                var result = new FieldAttributeValueDTO();
                var model = Request.Form["request"][0]?.StringToObject<FieldAttributeValueDTO>();


                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_ATTRIBUTE);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().UpdateFieldAttribute(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_FIELD_ATTRIBUTE })]
        public async Task<IActionResult> DeleteFieldAttribute(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvFormGroupBL>().DeleteFieldAttribute(Id);
                return Ok(result);
           
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_CONDITION })]
        public async Task<IActionResult> SaveFieldCondition()
        {
           

                var result = new FieldViewConditionDTO();
                var model = Request.Form["request"][0]?.StringToObject<FieldViewConditionDTO>();


                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_CONDITION);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().SaveFieldCondition(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_FIELD_CONDITION })]
        public async Task<IActionResult> UpdateFieldCondition()
        {
            
                var result = new FieldViewConditionDTO();
                var model = Request.Form["request"][0]?.StringToObject<FieldViewConditionDTO>();


                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(model!, ConstantKeys.AdminPermission.ADD_ADMIN_FIELD_CONDITION);
                if (validateObject)
                {
                    result = await masterBL.GetAdminService<SrvFormGroupBL>().UpdateFieldCondition(model!);
                }
                return Ok(new ResponseEntity(result));
           
        }

        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_FIELD_CONDITION })]
        public async Task<IActionResult> DeleteFieldCondition(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvFormGroupBL>().DeleteFieldCondition(Id);
                return Ok(result);
            
        }
    }
}
