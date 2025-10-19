using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ActionEntities
{
	public class ActionCondition:EntityBase, IAuditLogEntity
	{
		public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public string Type { get; set; } = null!;
        public Guid? RefID { get; set; }
		public string operators { get; set; } = null!;
		public string FieldValue { get; set; } = null!;
	}
}
