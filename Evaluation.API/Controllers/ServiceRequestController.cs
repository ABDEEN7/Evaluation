using Evaluation.API.ActionFilter;
using Evaluation.Services.Integration;
using Evaluation.Services.Models.API;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities;
using Evaluation.SharedHelper.Models.Api.FormAnalysisDtos;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Evaluation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{depRouting}/[action]")]
	[Authorize]

	public class ServiceRequestController : ControllerBase
    {
        private readonly ServiceRequestBL _serviceRequestBL;
        private readonly QNEDSService _qnedsService;

        public ServiceRequestController(ServiceRequestBL serviceRequestBL, QNEDSService qnedsService)
        {
            _serviceRequestBL = serviceRequestBL;
			_qnedsService = qnedsService;
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
       
        [HttpGet]
		public async Task<EvaluationRequestDTO> GetEvaluationDetails(Guid requestId)
		{
			return await _serviceRequestBL.GetEvaluationDetailsAsync(requestId);
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
			var assignUsersJson = Request?.Form!["teamUsers"].FirstOrDefault();
			var teamUsersJson = Request?.Form!["teamUsers"].FirstOrDefault();
			Guid requestId = Guid.TryParse(Request?.Form!["requestId"], out var tempId) ? tempId : Guid.Empty;
			Guid? evaluationRequestId = Guid.TryParse(Request?.Form!["evaluationRequestId"], out var EvlId) ? EvlId : null;

            var assignUsers = !string.IsNullOrEmpty(assignUsersJson)
                ? JsonConvert.DeserializeObject<List<AssignUserDTO?>>(assignUsersJson)!
                : new List<AssignUserDTO?>();

            var teamUsers = !string.IsNullOrEmpty(teamUsersJson)
                ? JsonConvert.DeserializeObject<List<EvalTeamRequestDto>>(teamUsersJson)!
                : new List<EvalTeamRequestDto>();

            string fieldValuesJson = Request.Form["fieldValues"];

            if (!string.IsNullOrEmpty(fieldValuesJson))
            {
                dto.FieldValues = JsonConvert.DeserializeObject<List<FieldValueDTO?>>(fieldValuesJson);
            }

            dto.RequestId = requestId;

            var response = await _serviceRequestBL.HandleServiceRequestAsync(
                dto,
                planId,
                evaluationRequestId,
                serviceId,
                actionName,
                fieldValuesJson,
                assignUsers,
                teamUsers,
                files,
                dto.ActionRemarks!,
                saveAsDraft);

            return response;
        }

		[HttpGet]
		public async Task<List<JsTreeNodeDto>> GetScopes(Guid partyId)
		{
			return await _serviceRequestBL.GetScopesList(partyId);
		}
		[HttpGet]
		public async Task<List<SupportedFileDto>> GetSupportedFiles(Guid requestId)
		{
			return await _serviceRequestBL.GetSupportedFiles(requestId);
		}
		[HttpPost]
		public async Task<bool> SaveSupportFiles()
		{
			var file = Request.Form.Files[0];
			Guid EvaluationRequestId = Guid.Parse(Request?.Form!["EvaluationRequestId"].FirstOrDefault());
			Guid ScopeId = Guid.Parse(Request?.Form!["ScopeId"].FirstOrDefault());
			return await _serviceRequestBL.SaveSupportFiles(file, EvaluationRequestId, ScopeId);
		}
		[HttpGet]
        public async Task<string> GetAttachmentUrl(Guid attachmentId, Guid requestId, Guid schId)
        {
            return await _serviceRequestBL.GetAttachmentUrlAsync(attachmentId, requestId, schId);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveNda( NdaApproveRequest dto)
        {
            var result = await _serviceRequestBL.ApproveNda(dto);
            return Ok(result);
        }
		[HttpGet]
		public async Task<IActionResult> CanCreateEvaluationPlanRequest()
		{
			var result = await _serviceRequestBL.CanCreateEvaluationPlanRequestAsync();
			return Ok(new { canCreate = result });
		}

		[HttpGet]
		[CheckRolePermisionFilter(true, ConstantKeys.WebPermissions.VIEW_EVALUATION_REQUEST_FormAnalysis)]

		public async Task<IActionResult> FormAnalysis(Guid requestId)
		{
			var result = await _serviceRequestBL.GetFormAnalysisAsync(requestId);
			return Ok(result);
		}

		[HttpGet]
		public async Task<List<OutputAnalysisViewDto>> GetOutputAnalysis(Guid evaluationRequestId)
		{
			return await _qnedsService.GetOutputAnalysisAsync(evaluationRequestId);
		}
	}
}