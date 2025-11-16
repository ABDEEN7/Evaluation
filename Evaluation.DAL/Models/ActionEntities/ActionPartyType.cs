using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.UserEntiy;

namespace Evaluation.DAL.Models.ActionEntities
{
    public class ActionPartyType : EntityBase, IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid PartyTypeId { get; set; }
        public PartyType? PartyType { get; set; }
        public Guid Priority { get; set; }
        public string? ActionPartyTypeSettings { get; set; }
    }
}
