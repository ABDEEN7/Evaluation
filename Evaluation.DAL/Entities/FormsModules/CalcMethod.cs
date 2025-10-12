using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.FormsModules;

public class CalcMethod : BaseEntities
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public decimal MinPercentage { get; set; }
    public decimal MaxPercentage { get; set; }
    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }
}
