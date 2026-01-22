using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.Template;

namespace Evaluation.DAL.Models.Planing.EvaluationRequestEntity;

public class EvaluationRequest : EntityBase
{
    public Guid PlanId { get; set; }
    public Plan? Plan { get; set; }
    public Guid OrgTreeId { get; set; }
    public OrgTree? OrgTree { get; set; }
    public Guid DepEvaluationTypeId { get; set; }
    public DepEvaluationType? DepEvaluationType { get; set; }
	public DateTime FromDate { get; set; }
	public DateTime ToDate { get; set; }
	public Guid ServiceId { get; set; }
	public Service? Service { get; set; }
	public Guid ServiceStatusId { get; set; }
	public ServiceStatus? ServiceStatus { get; set; }
    public string? RequestNumber  { get; set; }
    public long Sequence { get; set; }
	public virtual ICollection<ServiceRequestFieldsValue>? ServiceRequestFieldsValues { get; set; }



}