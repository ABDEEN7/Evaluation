

namespace Evaluation.SharedHelper.Models.Admin
{
    public class SystemModuleDTO : EntityBaseDTO
    {
        public Guid? DepartmentId { get; set; }
        public string? Department { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string DescriptionAr { get; set; } = null!;
        public string DescriptionEn { get; set; } = null!;
        public string Routing { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string SchNoDefinition { get; set; } = null!;
        public string? ButtonAr { get; set; }
        public string? ButtonEn { get; set; }
        public int OrderNo { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Button { get; set; }
        public string? Url { get; set; }
    }
}
