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
    public class ActionShowLogPartyType : EntityBase, IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid PartytypeId { get; set; }
        public PartyType? Partytype { get; set; }
    }
}
