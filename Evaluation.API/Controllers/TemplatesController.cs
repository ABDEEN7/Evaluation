using Evaluation.Services.BusinessLayer;
using Evaluation.Services.BusinessLayer.API.Template;
using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers
{

	[ApiController]
	[Route("api/[controller]/{depRouting}/[action]")]
	public class TemplatesController(MasterBL masterBl, RequestInfo requestInfo) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetLetterDocument(Guid requestId, Guid templateId)
		{
			var pdfByteArray = await masterBl
				.GetApiService<TemplateBl>()
				.GetLetterTemplate(requestId, templateId, requestInfo.Lang);

			if (pdfByteArray == null || pdfByteArray.Length == 0)
				return NotFound("The requested document could not be generated.");

			return File(pdfByteArray, "application/pdf", "document.pdf");
		}
	}

}
