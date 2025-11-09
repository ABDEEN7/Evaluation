using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Masters;
using Evaluation.DAL.Entities.Org;
using Evaluation.DAL.Entities.ServicesEntities;
using Evaluation.DAL.Entities.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Planing.EvaluationRequestEntity
{
	
	public class EvaluationRequestHistory : EntityBase
	{
		public Guid EvaluationRequestId { get; set; }
		public EvaluationRequest EvaluationRequest { get; set; }
		public Guid PlanId { get; set; }
		public Plan Plan { get; set; }
		public Guid OrgTreeId { get; set; }
		public OrgTree? OrgTree { get; set; }
		public Guid EvaluationTypeId { get; set; }
		public EvaluationType? EvaluationType { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public Guid ServiceId { get; set; }
		public Service? Service { get; set; }
		public Guid StatusServiceId { get; set; }
		public StatusService? StatusService { get; set; }
	}
}
