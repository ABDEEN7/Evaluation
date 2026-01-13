using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities
{
	public class EvaluationPartyServiceDTO
	{
		public Guid Id { get; set; }
		public string NameAr { get; set; } = null!;
		public string NameEn { get; set; } = null!;
		public string BackendName { get; set; } = null!;
		public int OrderNo { get; set; }
		public bool IsFreez { get; set; }
		public bool ShowInWebSite { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public List<ServiceRequestDTO> Requests { get; set; } = new();
	}
}
