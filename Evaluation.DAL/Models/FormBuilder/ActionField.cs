using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Evaluation.DAL.Models.FormBuilder
{
  
    public class ActionField : EntityBase, IAuditLogEntity
    {
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public bool IsEditable { get; set; }
        public int? OrderNo { get; set; }
        public ICollection<ActionFieldAttribute>? ActionFieldAttribute { get; } = new List<ActionFieldAttribute>();

    }
}
