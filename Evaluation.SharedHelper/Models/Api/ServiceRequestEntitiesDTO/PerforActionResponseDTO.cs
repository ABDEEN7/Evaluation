using Evaluation.DAL.Models.ActionEntities;
using Evaluation.DAL.Models.ServiceRequestEntities;

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
