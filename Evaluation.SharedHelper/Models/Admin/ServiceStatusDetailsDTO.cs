namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceStatusDetailsDTO
    {
        public ServiceStatusDTO ServiceStatus { get; set; } = null!;

        public List<Guid> ServiceStatusPreventPartyTypesList { get; set; } = new();
        public List<DropdownItem> PartyTypesList { get; set; } = new();



    }
}
