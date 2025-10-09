using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.OrganizationTrees;

public class OrganizationUnitAcademicYear : BaseEntities
{
    public int OrganizationUnitId { get; set; }
    public int ParentId { get; set; }
    public int AcademicYearId { get; set; }
}
