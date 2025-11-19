
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.ActionEntities
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
