using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities;

public class DepartmentUser : BaseEntities // i change the name of DepUser to this name 
{
    public int DepartmentId { get; set; }
    public int UserId { get; set; }
    public Department? Department { get; set; }
    public User? User { get; set; }
    
}
