namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceActionDetailsDTO
    {

        public ServiceActionDTO ServiceAction { get; set; } = null!;

        public List<DropdownItem> PartyTypesList { get; set; } = new();
        public List<DropdownItem> TemplateDocsList { get; set; } = new();
    }
}
