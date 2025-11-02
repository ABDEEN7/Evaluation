using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Org;

public class OrgAcademicYear : EntityBase
{
    public Guid OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public Guid? ParentOrgId { get; set; }
    public Organization? ParentOrg { get; set; }
    public Guid ParentId { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
}
