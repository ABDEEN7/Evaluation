using System.Reflection.PortableExecutable;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;

namespace Evaluation.DAL.Models.Planing;

public class PlanServiceRequest : EntityBase
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public bool IsEnableDeleted { get; set; }
    public Guid AcadmicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public bool HasOnePlan { get; set; }
}
