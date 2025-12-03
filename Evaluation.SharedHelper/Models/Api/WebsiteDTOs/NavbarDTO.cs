
namespace Evaluation.SharedHelper.Models.Api.WebsiteDTOs
{
    public class NavbarDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsInternal { get; set; }
        public Guid? ParentId { get; set; }
        public string Url { get; set; }
        public int? OrderNo { get; set; }
        public string? Target { get; set; }
        public List<NavbarDTO> Children { get; set; }
        public string? PermissionBackendName { get; set; }

        public bool IsAccessible { get; set; }
        public bool IsAuthorized { get; set; }
    }
}

