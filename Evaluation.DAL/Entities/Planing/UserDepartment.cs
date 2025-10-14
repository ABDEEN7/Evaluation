using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Planing;

public class UserDepartment : EntityBase
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}
