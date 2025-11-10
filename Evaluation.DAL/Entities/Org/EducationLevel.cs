using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.Org;

public class EducationLevel : EntityBase
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
    public int OrderNo { get; set; }
}
