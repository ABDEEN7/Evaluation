using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.IntegrationEntity
{
	public class IntegrationPoint : EntityBase, IAuditLogEntity
	{
		public string NameAr { get; set; } = null!;
		public string NameEn { get; set; } = null!;
		public string BackendName { get; set; } = null!;
		public string EndPoint { get; set; } = null!;
		public string? URLParameter { get; set; } = null!;
		public string ResponseSchema { get; set; } = null!;
		public bool IsInternal { get; set; }

	}
}
