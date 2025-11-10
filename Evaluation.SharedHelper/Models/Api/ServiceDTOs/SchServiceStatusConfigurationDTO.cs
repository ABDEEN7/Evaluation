


namespace Evaluation.SharedHelper.Models.Api.ServiceDTOs
{
    public class SchServiceStatusConfigurationDTO : EntityBaseDTO
    {
        public Guid ServiceId { get; set; }
        public Guid CurrentStatusId { get; set; }
        public string CurrentStatus { get; set; } = null!;
        public Guid NextStatusId { get; set; }
        public string NextStatus { get; set; } = null!;
    }
}
