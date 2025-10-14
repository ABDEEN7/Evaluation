using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities;

public class UserDeparment : EntityBase // i change the name of DepUser to this name 
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
}
