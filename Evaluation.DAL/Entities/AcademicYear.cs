using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class AcademicYear : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Year { get; set; }
    public int DepartmentId { get; set; }
}
