using Evaluation.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Audit
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
