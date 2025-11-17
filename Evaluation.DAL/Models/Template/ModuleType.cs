using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Template;

public class ModuleType : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid? ParentId { get; set; }
    public ModuleType? Parent { get; set; }
}
