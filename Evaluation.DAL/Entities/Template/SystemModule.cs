using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.Template;

public class SystemModule : EntityBase
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public ModuleType? ModuleType { get; set; }
    public Guid ModuleTypeId { get; set; }
}