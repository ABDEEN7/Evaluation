using Azure.Core;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Models.API;
using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Mail;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/{depRouting}/[action]")]
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
            var assignUsersJson = Request?.Form!["users"].FirstOrDefault();
            var teamUsersJson = Request?.Form!["teamUsers"].FirstOrDefault();
            Guid requestId = Guid.TryParse(Request?.Form!["requestId"], out var tempId) ? tempId : Guid.Empty;
            Guid? evaluationRequestId = Guid.TryParse(Request?.Form!["evaluationRequestId"], out var EvlId) ? EvlId : null;

            var assignUsers = !string.IsNullOrEmpty(assignUsersJson)
                ? JsonConvert.DeserializeObject<List<AssignUserDTO?>>(assignUsersJson)!
                : new List<AssignUserDTO?>();

            var teamUsers = !string.IsNullOrEmpty(teamUsersJson)
                ? JsonConvert.DeserializeObject<List<EvalTeamRequestDto>>(teamUsersJson)!
                : new List<EvalTeamRequestDto>();

            string fieldValuesJson = Request?.Form!["fieldValues"]!;

            if (!string.IsNullOrEmpty(fieldValuesJson))
            {
                dto.FieldValues = JsonConvert.DeserializeObject<List<FieldValueDTO?>>(fieldValuesJson);
            }

            dto.RequestId = requestId;

            var response = await _serviceRequestBL.HandleServiceRequestAsync(dto, planId, evaluationRequestId, serviceId, actionName, fieldValuesJson, assignUsers, teamUsers, files, dto.ActionRemarks!, saveAsDraft);
            return response;

        }

        [HttpGet]
        public async Task<string> GetAttachmentUrl(Guid attachmentId, Guid requestId, Guid schId)
        {
            return await _serviceRequestBL.GetAttachmentUrlAsync(attachmentId, requestId, schId);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveNda([FromBody] NdaApproveRequest dto)
        {
            var result = await _serviceRequestBL.ApproveNda(dto);
            return Ok(result);
        }
    }

}
