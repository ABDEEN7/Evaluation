
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class SchoolCourse : EntityBase
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Code { get; set; }

    //TODO: Add Grades
}
