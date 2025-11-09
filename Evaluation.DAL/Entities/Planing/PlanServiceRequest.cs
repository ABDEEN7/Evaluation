using System.Reflection.PortableExecutable;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Planing;

public class PlanServiceRequest : EntityBase
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public bool IsEnableDeleted { get; set; }
    public Guid AcadmicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public bool HasOnePlan { get; set; }
}
