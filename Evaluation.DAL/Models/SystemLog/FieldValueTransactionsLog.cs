using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.ServiceRequestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.SystemLog
{
    public class FieldValueTransactionsLog : EntityBase
    {
        public Guid RefId { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Guid? TransactionsTypeId { get; set; }
        public TransactionType? TransactionsType { get; set; }
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
		public Guid ServiceRequestFieldsValueId { get; set; }
		public ServiceRequestFieldsValue? ServiceRequestFieldsValue { get; set; }

	}
}
