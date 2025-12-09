using System.ComponentModel.Design;

namespace Evaluation.Web.Models;

public class EvalFormVm
{
    public Guid FormId { get; set; }
    public List<EvalItemVm> Items { get; set; } = new();

}
public class EvalItemVm
{
    public Guid ItemId { get; set; }
    // 1 → main row   |   1.1, 1.2 → sub rows
    public int Order { get; set; }
    public int? SubOrder { get; set; }
    public bool IsHasEvidence { get; set; }
    public SelectionType TypeOfSelection { get; set; }
    public string Criterion { get; set; } = "";
}
public class SubOrder
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public SelectionType TypeOfSelection { get; set; }
    public string Criterion { get; set; }

}
public enum SelectionType
{
    Scale1To5 = 1,
    ApproveReject = 2
}