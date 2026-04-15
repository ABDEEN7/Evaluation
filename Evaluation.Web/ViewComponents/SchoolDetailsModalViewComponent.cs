using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Evaluation.Web.ViewComponents;

public class SchoolDetailsModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string modalId = "schoolDetailsModal", string title = "School Details", string body = "This is the modal body.", bool isModal = true)
    {
        ViewData["ModalId"] = modalId;
        ViewData["Title"] = title;
        ViewData["Body"] = body;
		ViewBag.IsModal = isModal;
		return View();
    }
}