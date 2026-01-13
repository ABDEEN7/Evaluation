using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Planing;

public class Category : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string? BackendName { get; set; }
}
