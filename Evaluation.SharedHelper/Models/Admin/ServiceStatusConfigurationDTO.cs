


namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceStatusConfigurationDTO : EntityBaseDTO
    {
        public Guid ServiceId { get; set; }
        public string Service { get; set; } = null!;
        public Guid CurrentStatusId { get; set; }
        public string CurrentStatus { get; set; } = null!;
        public Guid NextStatusId { get; set; }
        public string NextStatus { get; set; } = null!;
    }
}
