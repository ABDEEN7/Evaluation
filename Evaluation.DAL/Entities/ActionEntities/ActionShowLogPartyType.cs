using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Entities.ActionEntities
{
    public class ActionShowLogPartyType : BaseEntities, IAuditLogEntity
    {
        public Guid ServiceActionId { get; set; }
        public ServiceAction? ServiceAction { get; set; }
        public Guid PartytypeId { get; set; }
        public PartyType? Partytype { get; set; }
    }
}
