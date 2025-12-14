using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ServiceRequestEntities
{
	public class RequestAssignment : EntityBase, IAuditLogEntity
	{
		public Guid MinistryUserId { get; set; }
		public MinistryUser? MinistryUser { get; set; } 
		public Guid ServiceRequestId { get; set; }
		public ServiceRequest? ServiceRequest { get; set; } 
		public Guid PartyTypeId { get; set; }
		public PartyType? PartyType { get; set; } 
		public bool IsLeader{ get; set; }
		public string? Note { get; set; }

	}
}
