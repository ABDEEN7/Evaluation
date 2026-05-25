using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.Org;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.StatusEntities;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ServiceRequestEntities
{
	public class ServiceRequest : EntityBase, IAuditLogEntity
	{
		public string RequestNumber { get; set; } = null!;
		public Guid StatusId { get; set; }
		public ServiceStatus? Status { get; set; }
		public Guid ServiceId { get; set; }
		public Service? Service { get; set; }
		public Guid? OrgTreeId { get; set; }
		public OrgTree? OrgTree { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public long Sequence { get; set; }
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
        public DateTime? VisitDateFrom { get; set; }
        public DateTime? VisitDateTo { get; set; }
        public string? Name { get; set; }
        public Guid? VisitorUserId { get; set; }
        public MinistryUser? VisitorUser { get; set; }
        public Guid? EducationLevelId { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public Guid? SchoolCourseId { get; set; }
        public SchoolCourse? SchoolCourse { get; set; }
        public Guid? SchoolClassId { get; set; }
        public SchoolClass? SchoolClass { get; set; }


        public virtual ICollection<ServiceRequestFieldsValue>? ServiceRequestFieldsValues { get; set; }
		public virtual ICollection<RequestAssignment>? Assignments { get; set; }
	}
}
