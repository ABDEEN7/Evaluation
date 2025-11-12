using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionPartyTypeDTO : EntityBaseDTO
    {
        public Guid ServiceActionId { get; set; }
        public Guid PartyTypeId { get; set; }
    }
}
