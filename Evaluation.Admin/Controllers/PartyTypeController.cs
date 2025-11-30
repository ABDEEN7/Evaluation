using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.Models.Admin;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Evaluation.Admin.Controllers
{
    [Authorize]
    public class PartyTypeController : Controller
    {
        
        private readonly UserInfo userInfoSession;
        private readonly MasterBL masterBL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PartyTypeController(UserInfo userInfoSession, MasterBL masterBL, IHttpContextAccessor httpContextAccessor)
        {

            this.userInfoSession = userInfoSession;
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
           
        }
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_PARTYTYPE })]
        public async Task<IActionResult> Index()
        {
            var model = new PartyTypeVM(httpContextAccessor);
            await model.LoadAllAData(new string[] { ConstantKeys.AdminPages.AdminPartyType },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_PARTYTYPE },
                new string[] {
                 ConstantKeys.AdminSettings.ADMIN_FILE_EXTENSION,
                 ConstantKeys.AdminSettings.ADMIN_FILE_SIZE,
                 ConstantKeys.AdminSettings.ADMIN_FILE_COUNT
            });
           
            var property = typeof(PartyType).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_PARTYTYPE })]
       
        public async Task<IActionResult> GetAllPartyType([FromBody] AdminSearchDTO request)
        {

            if (null == request)
            {
                request = new AdminSearchDTO
                {
                    PageNum = 0,
                    PageSize = Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE)),
                };
            }
            else
            {
                request.PageNum = request.PageNum ?? 0;
                request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            }
            request.PageNum = request.PageNum ?? 0;
            request.PageSize = request.PageSize ?? Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvPartyTypeBL>().GetPartyTypeList(request);
            return Ok(response);
        }


        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_PARTYTYPE })]
        public async Task<IActionResult> SavePartyType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<PartyTypeDTO>();

            var result = new PartyTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_PARTYTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvPartyTypeBL>().SavePartyType(request!);
                    
                }
                return Ok(new ResponseEntity(result));
           
        }
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_PARTYTYPE })]
        public async Task<IActionResult> UpdatePartyType()
        {
           
                var request = Request.Form["request"][0]?.StringToObject<PartyTypeDTO>();
          
            var result = new PartyTypeDTO();
                bool validateObject = await masterBL.GetAdminService<SrvBaseBL>().ValidateObject(request!, ConstantKeys.AdminPermission.ADD_ADMIN_PARTYTYPE);
                if (validateObject)
                {
                     result = await masterBL.GetAdminService<SrvPartyTypeBL>().UpdatePartyType(request!);
                }
                return Ok(new ResponseEntity(result));
            
        }

       
        [HttpPost]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_PARTYTYPE })]
        public async Task<IActionResult> DeletePartyType(Guid Id)
        {
           
                var result = await masterBL.GetAdminService<SrvPartyTypeBL>().DeletePartyType(Id);
                return Ok(result);
           
        }

    }
}
