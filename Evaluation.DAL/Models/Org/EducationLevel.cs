using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.Org;

public class EducationLevel : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public int OrderNo { get; set; }
}
