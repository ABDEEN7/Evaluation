using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Models.API;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class ServiceRequestController : ControllerBase
	{
		private readonly ServiceRequestBL _serviceRequestBL;

		public ServiceRequestController(ServiceRequestBL serviceRequestBL)
		{
			_serviceRequestBL = serviceRequestBL;
		}

		[HttpPost]
		public async Task<WebAppPlanRequestsDTO> GetPlanRequests([FromBody] FilterRequestsDTO data)
		{
			return await _serviceRequestBL.GetPlanRequestsAsync(data);
		}
		[HttpPost]
		public async Task<WebAppEvaluationRequestsDTO> GetEvaluationRequests([FromBody] FilterRequestsDTO data)
		{
			return await _serviceRequestBL.GetEvaluationRequestsAsync(data);
		}
		[HttpGet]
		public async Task<ServiceRequestDTO> GetApplicationDetails(Guid requestId)
		{
			return await _serviceRequestBL.GetApplicationDetailsAsync(requestId);
		}

		[HttpPost]
		public async Task<ServiceRequestDTO> HandleRequest(
			[FromForm] ActionFormDTO dto,
			[FromQuery] string actionName,
			[FromForm] Guid serviceId,
			[FromQuery] Guid? planId,
			[FromForm] bool saveAsDraft)
		{
			var files = Request.Form?.Files;
			var assignUsersJson = Request?.Form!["users"].FirstOrDefault();
			Guid requestId = Guid.TryParse(Request?.Form!["requestId"], out var tempId) ? tempId : Guid.Empty;

			var assignUsers = !string.IsNullOrEmpty(assignUsersJson)
				? JsonConvert.DeserializeObject<List<AssignUserDTO?>>(assignUsersJson)!
				: new List<AssignUserDTO?>();

			string fieldValuesJson = Request?.Form!["fieldValues"]!;

			if (!string.IsNullOrEmpty(fieldValuesJson))
			{
				dto.FieldValues = JsonConvert.DeserializeObject<List<FieldValueDTO?>>(fieldValuesJson);
			}

			dto.RequestId = requestId;

			return await _serviceRequestBL.HandleServiceRequestAsync(dto, planId, serviceId,actionName,fieldValuesJson,assignUsers,files, dto.ActionRemarks!, saveAsDraft);
		}

		[HttpGet]
		public async Task<string> GetAttachmentUrl(Guid attachmentId, Guid requestId, Guid schId)
		{
			return await _serviceRequestBL.GetAttachmentUrlAsync(attachmentId, requestId, schId);
		}

	}

}
