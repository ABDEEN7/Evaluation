using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ServiceRequestEntities
{
	public class ServiceRequestFieldsValue : EntityBase, IAuditLogEntity
	{
		public Guid ServiceRequestId { get; set; }
		public ServiceRequest? ServiceRequest { get; set; }
		public Guid FieldId { get; set; }
		public Field? Field { get; set; }
		public string? Value { get; set; }
		public bool? IsMissing { get; set; }
		public bool IsApproved { get; set; }
	}

}
