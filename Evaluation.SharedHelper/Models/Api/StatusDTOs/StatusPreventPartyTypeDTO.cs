using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.StatusDTOs
{
    public class StatusPreventPartyTypeDTO : EntityBaseDTO
    {
        public Guid StatusId { get; set; }
        public Guid PartyTypeId { get; set; }
        public bool IsPrevent { get; set; }
    }
}
