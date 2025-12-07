using Evaluation.DAL.Models.Planing;
using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Dtos.PlanDto;

public class ApproverPlan
{
    public Plan Plan { get; set; }
    public List<SelectedSchool>? Schools { get; set; }
}
