using Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.Audit
{
    public class AuditLog : EntityBase
    {
        public string TableName { get; set; } = null!;
        public string RefId { get; set; } = null!;
        public string ColumnName { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

    }
}
