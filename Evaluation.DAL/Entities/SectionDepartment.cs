using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities;

public class SectionDepartment : EntityBase
{
    public int SectionId { get; set; }
    public int DepartmentId { get; set; }
}