
namespace Evaluation.SharedHelper.Models.Admin
{
    public class DepTargetOrgTreeDTO : EntityBaseDTO
    {
        public Guid DepartmentId { get; set; }
        public string? Department { get; set; }
        public Guid CategoryId { get; set; }
        public string? Category { get; set; }
        public Guid TargetOrgTreeId { get; set; }
        public string? TargetOrgTree { get; set; }
        public string? PredicateFuncion { get; set; }
        public string? DepTargetConfig { get; set; }

    }
}
