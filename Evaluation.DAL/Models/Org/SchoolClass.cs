using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class SchoolClass : EntityBase
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Grade { get; set; }
    public int Duration { get; set; }
    public string Type { get; set; }
    public string Code { get; set; }
    public Guid CourseId { get; set; }
    public SchoolCourse Course { get; set; }
    public List<SchoolTerm> Terms { get; set; }
    public Guid SchoolId { get; set; }
    public School School { get; set; }
}
