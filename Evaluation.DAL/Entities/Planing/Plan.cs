using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.Planing;

public class Plan : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int DepartmentId { get; set; }
    public int AcademicYearId { get; set; }
    public Department Department { get; set; }
    public AcademicYear AcademicYear { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDateDate { get; set; }
    public string ExpectedListJson { get; set; }

}
