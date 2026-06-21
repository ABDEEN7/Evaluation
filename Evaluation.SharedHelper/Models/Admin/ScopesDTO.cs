

namespace Evaluation.SharedHelper.Models.Admin
{
    public class ScopesDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public string? Department { get; set; }
        public Guid ScopeTypeId { get; set; }
        public string? ScopeType { get; set; }
        public int OrderNo { get; set; }
        public string? ScopeNumber { get; set; }

    }
}
