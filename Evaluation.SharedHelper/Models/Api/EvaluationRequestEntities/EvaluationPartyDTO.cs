using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities
{
	public class EvaluationPartyDTO
	{
		public Guid Id { get; set; }
		public Guid DepartmentId { get; set; }
		public string NameAr { get; set; } = null!;
		public string NameEn { get; set; } = null!;
		public int OrderNo { get; set; }
        public bool IsSupportFiles { get; set; }
        public virtual List<EvaluationPartyServiceDTO> Services { get; set; }  = [];
	}
}
