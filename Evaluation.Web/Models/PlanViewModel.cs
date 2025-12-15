namespace Evaluation.Web.Models;

public class PlanViewModel
{
    public string RenderType { get; set; }  // "action", "preview", "comparison"
    public string ActionType { get; set; }  // "CREATE", "EDIT", "APPROVE", etc.
    public Guid? PlanId { get; set; }
    public Guid? OldPlanId { get; set; }
    public PlanDetailsDto Plan { get; set; }
    public PlanDetailsDto OldPlan { get; set; }
    public bool ShowApprovalButtons { get; set; } = false;
}
public class PlanCreateDto
{
    public string Title { get; set; }
    public Guid PlanTypeId { get; set; }
    public Guid? SemesterId { get; set; }
    public string DateRange { get; set; }
    public List<PlanSchoolDto> Schools { get; set; }
}

public class PlanUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Guid PlanTypeId { get; set; }
    public Guid? SemesterId { get; set; }
    public string DateRange { get; set; }
    public List<PlanSchoolDto> Schools { get; set; }
}

public class PlanSchoolDto
{
    public Guid SchoolId { get; set; }
    public string VisitDate { get; set; }
    public Guid VisitTypeId { get; set; }
}

public class PlanDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid PlanTypeId { get; set; }
    public string PlanTypeName { get; set; }
    public Guid? SemesterId { get; set; }
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
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }
    public string Rating { get; set; }
    public string VisitDate { get; set; }
    public string LastEvaluationDate { get; set; }
    public Guid VisitTypeId { get; set; }
    public string VisitType { get; set; }
    public string AcademicYear { get; set; }
}
