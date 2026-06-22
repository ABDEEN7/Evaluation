using Evaluation.DAL.Models.Attachments;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.Template;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluation.DAL.Models.Planing.EvaluationRequestEntity;

public class EvaluationRequest : EntityBase, IAuditLogEntity
{
	public string? Name { get; set; } = null!;
	public Guid PlanId { get; set; }
    public Plan? Plan { get; set; }
    public Guid OrgTreeId { get; set; }
    public OrgTree? OrgTree { get; set; }
    //public Guid AcademicYearId { get; set; }
    //public AcademicYear? AcademicYear { get; set; }
	public Guid DepEvaluationTypeId { get; set; }
    public DepEvaluationType? DepEvaluationType { get; set; }
	public DateTime FromDate { get; set; }
	public DateTime ToDate { get; set; }
	public Guid ServiceId { get; set; }
	public Service? Service { get; set; }
	public Guid ServiceStatusId { get; set; }
	public ServiceStatus? ServiceStatus { get; set; }
    public string RequestNumber { get; set; } = null!;
    public long Sequence { get; set; }
    public Guid? FormEvalMatrixValueId { get; set; }
    public FormEvalMatrixValue? FormEvalMatrixValue { get; set; }

    public int? EvalDays { get; set; }  // Copy
    public DateOnly? EvaluationDate { get; set; }
    public DateOnly? NextEvaluationDate { get; set; }
    public decimal? FinalEvalValue { get; set; }
    public Guid? FinalReportId { get; set; }

    public virtual ICollection<ServiceRequestFieldsValue>? ServiceRequestFieldsValues { get; set; }
    public virtual ICollection<EvaluationRequestAssignment>? EvaluationRequestAssignments { get; set; }
    public virtual ICollection<ServiceRequest>? ServiceRequest { get; set; }

   




}