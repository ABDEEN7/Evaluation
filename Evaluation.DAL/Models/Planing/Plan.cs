using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.DAL.Models.Planing;

public class Plan : EntityBase, IAuditLogEntity
{
    public string PlanName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
  
    public Guid? AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public Guid? PlanStatusId { get; set; }
    public PlanStatus? PlanStatus { get; set; }
    public Guid? PlanTypeDepId { get; set; }
    public PlanTypeDep? PlanTypeDep { get; set; }
    public Guid? SemesterId { get; set; }
    public Semester? Semester { get; set; }
    public string? PlanJsonValue { get; set; }
    public Guid? PlanAttachmentId { get; set; }
    public EvalAttachment? PlanAttachment { get; set; }

    public virtual ICollection<EvaluationRequest>? EvaluationRequests { get; set; }

    

}
