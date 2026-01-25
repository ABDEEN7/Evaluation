using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.Planing;

public class UserDepartment : EntityBase, IAuditLogEntity
{
    public Guid UserId { get; set; }
    public MinistryUser? User { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}
