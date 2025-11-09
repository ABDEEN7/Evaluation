namespace Evaluation.SharedHelper.Models.Admin
{
    public class PermissionDTO : EntityBaseDTO
    {
        public string BackEndName { get; set; } = null!;

        public string? PermissionNameAr { get; set; }

        public string? PermissionNameEn { get; set; }

        public string? Description { get; set; }

    }
}
