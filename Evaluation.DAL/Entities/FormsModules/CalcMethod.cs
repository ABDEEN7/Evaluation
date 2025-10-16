using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.FormsModules;

public class CalcMethod : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public decimal Min { get; set; }
    public decimal Max { get; set; }
}
