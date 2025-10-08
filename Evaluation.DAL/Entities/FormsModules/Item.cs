using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities.FormsModules;

// i change the name of FormScopeItem to Item to more clearly 
public class Item : BaseEntities
{
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public bool IsEvaluation{ get; set; }// I see that Weight alone is sufficient without the need for ApplyEvaluation
    public int Weight { get; set; }
    public Guid FormId { get; set; }
    public Guid ScopeId { get; set; }
    public Guid Type { get; set; } // ask mohand give me example 
    public Guid DropDownType { get; set; } // if activate Min Max or not and if mkae questioning or not
    public Guid CalcMethodId { get; set; }
    public CalcMethod? CalcMethod { get; set; }
}