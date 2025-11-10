using Evaluation.Admin.ActionFilter;
using Evaluation.Admin.Extensions;
using Evaluation.Admin.Models;
using Evaluation.DAL.Entities.ActionEntities;
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
    public class ServiceStatusController : Controller
    {

        private readonly MasterBL masterBL;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService _blobService;
        public ServiceStatusController(MasterBL masterBL, IHttpContextAccessor httpContextAccessor, AzureBlobStorageService blobService)
        {
            this.masterBL = masterBL;
            this.httpContextAccessor = httpContextAccessor;
            _blobService = blobService;

        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> Index()
        {
            var model = new ServiceStatusVM(httpContextAccessor);

            await model.LoadAllAData(
                new string[] { ConstantKeys.AdminPages.AdminServiceStatus,
                ConstantKeys.AdminPages.AdminActionStatusConfiguration ,
                ConstantKeys.AdminPages.AdminActionStatusConfigurationNotification },
                new string[] { ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION,
                ConstantKeys.AdminPermission.ADD_ADMIN_ACTIONSTATUSCONFIGURATION_NOTIFICATION }
                );

            var ServicesTask = masterBL.GetAdminService<SrvServiceStatusBL>().GetServicesForStatus();
            var PartyTypesListTask = masterBL.GetAdminService<SrvServiceStatusBL>().GetPartyTypesList();
            var ActionTypesTask = masterBL.GetAdminService<SrvServiceActionBL>().GetServiceActionTypes();
            var AllActionTask = masterBL.GetAdminService<SrvServiceStatusBL>().GetServiceAllAction();
            model.Services = await ServicesTask;
            model.PartyTypesList = await PartyTypesListTask;
            model.ActionTypes = await ActionTypesTask;
            ViewBag.AllActions = await AllActionTask;
            var property = typeof(ActionStatusConfiguration).GetProperty("OrderNo");
            model.containsOrderNo = property != null ? true : false;
            return View(model);
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> GetAllServiceStatusList(Guid serviceId, int Page = 1)
        {

            var PageSize =  Convert.ToInt32(masterBL.GetAdminService<SrvSystemSettingBL>().GetSetting(ConstantKeys.AdminSettings.ADMIN_PAGE_SIZE));
            var response = await masterBL.GetAdminService<SrvServiceStatusBL>().GetAllServiceStatusList(serviceId, Page, PageSize);
            return Ok(response);
        }






        [HttpPost()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> UpdateServiceStatusOrder()
        {
            //throw new NotImplementedException();    
            var list = Request.Form["OrderObj"][0]?.StringToObject<List<OrderingDTO>>();
            var result = await masterBL.GetAdminService<SrvServiceStatusBL>().UpdateServiceStatusOrder(list!);
            return Ok(result);

        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> GetStatusDetails(Guid statusId)
        {
            var result = await masterBL.GetAdminService<SrvServiceStatusBL>().GetStatusDetails(statusId);

            return Ok(result);
        }


        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.DELETE_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> DeleteServiceStatus(Guid id)
        {
            //throw new NotImplementedException();    
            var result = await masterBL.GetAdminService<SrvServiceStatusBL>().DeleteServiceStatus(id);
            return Ok(result);
        }



        [HttpPost()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.ADD_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> SaveServiceStatus()
        {
            var model = Request.Form["request"][0]?.StringToObject<ManageServiceStatusDTO>();
            var result = await masterBL.GetAdminService<SrvServiceStatusBL>().SaveServiceStatus(model!);
            return Ok(result);
        }


        [HttpPost()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.EDIT_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> UpdateServiceStatus()
        {
            var model = Request.Form["request"][0]?.StringToObject<ManageServiceStatusDTO>();
            var result = await masterBL.GetAdminService<SrvServiceStatusBL>().UpdateServiceStatus(model!);
            return Ok(result);
        }



        [HttpGet()]
        [CheckRolePermisionFilter(true, PermisionNames: new[] { ConstantKeys.AdminPermission.VIEW_ADMIN_SERVICE_STATUS })]
        public async Task<IActionResult> GetServiceStatusPartyTypeDisplayNameList(Guid serviceStatusId)
        {
            var response = await masterBL.GetAdminService<SrvServiceStatusBL>().GetServiceStatusPartyTypeDisplayNameList(serviceStatusId);
            return Ok(response);
        }

    }
}
