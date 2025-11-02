using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities;

public class SectionDepartment : EntityBase
{
    public Guid SectionId { get; set; }
    public Section? Section { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}