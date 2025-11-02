using System.Threading.Tasks;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.Controllers
{
    public class PlanController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
