
using Evaluation.DAL.Entities.ActionEntities;
using Evaluation.DAL.Entities.ServiceRequestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO
{
    public class PerforActionResponseDTO
    {
        public IList<ActionStatusConfigNotification> notifications { get; set; }
        public ServiceRequest request { get; set; }
        public ServiceAction actiondb { get; set; }
        public Guid actionlog { get; set; }
    }
}
