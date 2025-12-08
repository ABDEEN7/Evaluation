namespace Evaluation.Web.Models;

public class PlanViewModel
{
    public string RenderType { get; set; }  // "action", "preview", "comparison"
    public string ActionType { get; set; }  // "CREATE", "EDIT", "APPROVE", etc.
    public int? PlanId { get; set; }
    public int? OldPlanId { get; set; }
    public PlanDetailsDto Plan { get; set; }
    public PlanDetailsDto OldPlan { get; set; }
    public bool ShowApprovalButtons { get; set; } = false;
}
public class PlanCreateDto
{
    public string Title { get; set; }
    public int PlanTypeId { get; set; }
    public int? SemesterId { get; set; }
    public string DateRange { get; set; }
    public List<PlanSchoolDto> Schools { get; set; }
}

public class PlanUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PlanTypeId { get; set; }
    public int? SemesterId { get; set; }
    public string DateRange { get; set; }
    public List<PlanSchoolDto> Schools { get; set; }
}

public class PlanSchoolDto
{
    public int SchoolId { get; set; }
    public string VisitDate { get; set; }
    public int VisitTypeId { get; set; }
}

public class PlanDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PlanTypeId { get; set; }
    public string PlanTypeName { get; set; }
    public int? SemesterId { get; set; }
    public string SemesterName { get; set; }
    public string DateRange { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool ShowSemester { get; set; }
    public List<SchoolDetailsDto> Schools { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}

public class SchoolDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }
    public string Rating { get; set; }
    public string VisitDate { get; set; }
    public string LastEvaluationDate { get; set; }
    public int VisitTypeId { get; set; }
    public string VisitType { get; set; }
    public string AcademicYear { get; set; }
}
