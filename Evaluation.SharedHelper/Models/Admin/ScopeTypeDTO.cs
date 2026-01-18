using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;

namespace Evaluation.SharedHelper.Models.Admin
{
    public class ScopeTypeDTO : EntityBaseDTO
    {
        public string? BackendName { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public Guid? ParentId { get; set; }
        public string? Parent { get; set; }
        public Guid DepartmentId { get; set; }
        public string? Department { get; set; }
        public int OrderNo { get; set; }
    }
}