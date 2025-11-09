using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.Planing.EvaluationRequestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ServiceRequestEntities
{
	public class EvaluationRequestHistoryFieldsValue : EntityBase, IAuditLogEntity
	{
		public Guid EvaluationRequesttId { get; set; }
		public EvaluationRequest? EvaluationRequest { get; set; }

		public Guid EvaluationRequestHistoryId { get; set; }
		public EvaluationRequestHistory? EvaluationRequestHistory { get; set; }
		public Guid FieldId { get; set; }
		public Field? Field { get; set; }
		public string? Value { get; set; }
		public bool? IsMissing { get; set; }
		public bool IsApproved { get; set; }
	}

}
