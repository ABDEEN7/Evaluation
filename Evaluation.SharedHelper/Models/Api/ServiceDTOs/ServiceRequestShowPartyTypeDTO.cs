using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceDTOs
{
    public class ServiceRequestShowPartyTypeDTO : EntityBaseDTO
    {
        public Guid ServiceId { get; set; }
        public Guid PartyTypeId { get; set; }
    }
}
