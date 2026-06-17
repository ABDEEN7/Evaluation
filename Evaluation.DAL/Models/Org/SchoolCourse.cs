
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class SchoolCourse : EntityBase
{
    public string NameAr { get; set; } = null!;//  Arabic , English , Math
    public string NameEn { get; set; } = null!;
    public string? IntegrationCode { get; set; }
    public int OrderNo { get; set; } = 0;
    //TODO: Add Grades
}
