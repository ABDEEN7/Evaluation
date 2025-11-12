using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceDTOs
{
    public class ServiceInitiatorPartyTypeDTO : EntityBaseDTO
    {
        public Guid serviceId { get; set; }
        public Guid PartyTypeId { get; set; }
    }
}
