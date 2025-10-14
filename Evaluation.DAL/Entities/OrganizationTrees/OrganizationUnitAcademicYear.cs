using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.OrganizationTrees;

public class OrganizationUnitAcademicYear : EntityBase
{
    public int OrganizationUnitId { get; set; }
    public int ParentId { get; set; }
    public int AcademicYearId { get; set; }
}
