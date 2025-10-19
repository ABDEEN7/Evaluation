using Evaluation.DAL.Entities.ActionEntities;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Logs
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

    }
}
