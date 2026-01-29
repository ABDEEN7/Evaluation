using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.ServiceEnities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ServiceRequestEntities
{
	public class ServiceStatusConfiguration : EntityBase, IAuditLogEntity
	{
		public Guid ServiceId { get; set; }
		public Service? Service { get; set; }
		public Guid CurrentStatusId { get; set; }
		public Guid NextStatusId { get; set; }
	}
}
