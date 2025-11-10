namespace Evaluation.SharedHelper.Models.Admin
{
    public class ManageServiceStatusDTO
    {
        public ServiceStatusDTO status { get; set; } = null!;
        public string json { get; set; } = null!;

        public List<Guid> ServiceStatusPreventPartyTypesList { get; set; } = new();
    }
}
