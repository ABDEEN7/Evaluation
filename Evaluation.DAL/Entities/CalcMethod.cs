using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class CalcMethod : BaseEntities
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public decimal Min { get; set; }
    public decimal Max { get; set; }
}
