using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormsModules;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.Planing;
using Evaluation.DAL.Entities.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.StatusEntities;
using Evaluation.DAL.Entities.SystemModulesEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ServiceRequestEntities
{
	public class ServiceRequest : EntityBase, IAuditLogEntity
	{
		public string RequestNumber { get; set; } = null!;
		public Guid StatusId { get; set; }
		public ServiceStatus? Status { get; set; }
		public Guid ServiceId { get; set; }
		public Service? Service { get; set; }
		public Guid OrgTreeId { get; set; }
		public OrgTree? OrgTree { get; set; }
		public int Sequence { get; set; }
		public Guid? PlanId { get; set; }
		public Plan? Plan { get; set; }
		public Guid? EvaluationRequestId { get; set; }
		public EvaluationRequest? EvaluationRequest { get; set; }
		public Guid? EvaluationPartyId { get; set; }
		public EvaluationParty? EvaluationParty { get; set; }
		public Guid? EvaluationRequestHistoryId { get; set; }
		public EvaluationRequestHistory? EvaluationRequestHistory { get; set; }
		public Guid? PlanHistoryId { get; set; }
		public PlanHistory? PlanHistory { get; set; }

		public virtual ICollection<EvaluationRequestFieldsValue>? ServiceRequestFieldsValues { get; set; }
		public virtual ICollection<RequestAssignment>? Assignments { get; set; }
	}
}
