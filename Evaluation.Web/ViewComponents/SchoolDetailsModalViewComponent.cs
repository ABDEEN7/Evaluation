using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.ViewComponents;


public class SchoolDetailsModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string modalId, string title = "School Details", string body = "This is the modal body.", bool isModal = true, string type = "SCHOOL")
    {
        ViewData["ModalId"] = modalId;
        ViewData["Title"] = title;
        ViewData["Body"] = body;
        ViewBag.IsModal = isModal;
        ViewBag.type = type;
        return View();
    }
}