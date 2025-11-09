using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ServiceRequestEntities
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
		public bool IsNDA{ get; set; }
		public bool NDAStatusId{ get; set; }
		public bool NDAApproveDate{ get; set; }
	}
}
