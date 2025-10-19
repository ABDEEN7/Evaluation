using Evaluation.DAL.Entities.Authentication;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.DepartementEntites;

namespace Evaluation.DAL.Entities.Planing;

public class UserDepartment : EntityBase
{
    public Guid UserId { get; set; }
    public MinistryUser? User { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}
