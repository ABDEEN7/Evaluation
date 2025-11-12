using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.StatusDTOs
{
    public class StatusPartyTypeDisplayNameDTO : EntityBaseDTO
    {
        public string Title { get; set; } = null!;
        public Guid StatusId { get; set; }
        public Guid PartyTypeId { get; set; }
    }
}
