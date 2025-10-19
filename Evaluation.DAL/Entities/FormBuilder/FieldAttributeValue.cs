using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormBuilder
{
    public class FieldAttributeValue : EntityBase, IAuditLogEntity
    {
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
        public string AttributeKey { get; set; } = null!;
        public string? AttributeValue { get; set; }
        public string? MessageAr { get; set; }
        public string? MessageEn { get; set; }
        public string? Description { get; set; }

    }
}
