using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.FormBuilder
{
    public class FieldPartyType : EntityBase, IAuditLogEntity
    {
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public Guid FieldId { get; set; }
        public Field? Field { get; set; }
    }
}
