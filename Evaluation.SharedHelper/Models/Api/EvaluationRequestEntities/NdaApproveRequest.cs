using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities
{
	public class NdaApproveRequest
	{
		public Guid EvaluationRequestId { get; set; }
		public Guid? PlanId { get; set; }
		public Guid? MinistryUserId { get; set; } 
		public Guid? NDAStatusId { get; set; }
		public string ConflictReason { get; set; } = null!;
	}
	public class NdaApproveResponse
	{
		public bool IsSuccess { get; set; }
		public bool IsNdaApprovalPending { get; set; }
		public string? Message { get; set; }
	}
}
