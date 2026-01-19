using Evaluation.DAL.Migrations;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.Models.API;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.ServiceDTOs;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/{depRouting}/[action]")]
	public class FormRenderController : ControllerBase
	{
		private readonly ServiceRequestBL _serviceRequestBL;
		private readonly FormRenderBL _formRenderBL;

		public FormRenderController(ServiceRequestBL serviceRequestBL, FormRenderBL formRenderBL)
		{
			_serviceRequestBL = serviceRequestBL;
			_formRenderBL = formRenderBL;
		}
		

		[HttpGet]
		public async Task<IList<CssClassesDTO>> GetCssClasses()
		{
			return await _formRenderBL.GetCssClassesAsync();
		}

		[HttpGet]
		public async Task<ServiceDTO> GetService(Guid serviceId)
		{
			return await _formRenderBL.GetServiceAsync(serviceId);
		}
		[HttpGet]
		public async Task<ServiceDTO> GetCreatePlanService(Guid serviceId)
		{
			return await _formRenderBL.GetCreatePlanService(serviceId);
		}
		[HttpGet]
		public async Task<ServiceDTO> GetCreateEvaluationPartyService(Guid DepartementId, Guid serviceId)
		{
			return await _formRenderBL.GetCreateEvaluationPartyService(DepartementId, serviceId);
		}
		[HttpGet]
		public async Task<List<ServiceDTO>> GetServicesWebApp(string? moduelName)
		{
			return await _formRenderBL.GetServicesWebAppAsync(moduelName);
		}

		[HttpGet]
		public async Task<ActionResult<List<DropDownValueDTO>>> GetDropDownValuesByTypeId(
			[FromQuery] Guid? requestId,
			[FromQuery] Guid? schId,
			[FromQuery] Guid? dropDownTypeId)
		{
			if (dropDownTypeId is null)
			{
				return BadRequest("dropDownTypeId is required.");
			}

			var values = await _formRenderBL.GetDropDownValuesByTypeIdAsync(
				requestId,
				schId,
				dropDownTypeId.Value);

			return Ok(values);
		}

		[HttpGet]
		public async Task<ActionResult<List<DropDownValueDTO>>> GetDropDownValuesById(
			[FromQuery] Guid? requestId,
			[FromQuery] Guid? schId,
			[FromQuery] Guid? dropDownTypeId,
			[FromQuery] string value)
		{
			if (dropDownTypeId is null || string.IsNullOrEmpty(value))
			{
				return BadRequest("dropDownTypeId and value are required.");
			}

			var values = await _formRenderBL.GetDropDownValuesByIdAsync(
				requestId,
				schId,
				dropDownTypeId.Value,
				Guid.Parse(value));

			return Ok(values);
		}

		[HttpGet]
		public async Task<IEnumerable<TempLateDocDTO>> GetActionTemplatesByStatus(Guid requestId)
		{
			return await _formRenderBL.GetActionTemplatesByStatusAsync(requestId);
		}

		[HttpGet]
		public async Task<ServiceRequestDTO> GetActionField(
			[FromQuery] Guid serviceId,
			[FromQuery] string actionBackendKey,
			[FromQuery] Guid? requestId = null,
			[FromQuery] Guid? scholarshipId = null)
		{
			return await _formRenderBL.GetActionFieldAsync(serviceId, actionBackendKey, requestId, scholarshipId);
		}
	}
}

