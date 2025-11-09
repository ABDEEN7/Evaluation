using Evaluation.DAL.Entities.Org;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Web.ViewComponents;

public class SchoolDetailsModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string modalId = "schoolDetailsModal", string title = "School Details", string body = "This is the modal body.")
    {
        ViewData["ModalId"] = modalId;
        ViewData["Title"] = title;
        ViewData["Body"] = body;

        return View();
    }
}