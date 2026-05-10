using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Dtos.TeamMemberDto
{
	public class ForceAssignmentStatusDto
	{
		public Guid MinistryUserId { get; set; }

		public Guid EvaluationRequestId { get; set; }
	}
}
