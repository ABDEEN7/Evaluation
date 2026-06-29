using Evaluation.SharedHelper.Extensions;
using Evaluation.SharedHelper.Models.Api;
using Microsoft.AspNetCore.Mvc;
namespace Evaluation.Web.Controllers
{
    [Route("{language=ar}/[controller]/{depRouting}")]
    public class UiControlController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UiControlController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        [HttpPost("UiControlList")]
        public IActionResult UiControlList([FromBody] List<UiControlItemDTO> model)
        {
            return ViewComponent("UiControlList", model);
        }

        
    }
}
