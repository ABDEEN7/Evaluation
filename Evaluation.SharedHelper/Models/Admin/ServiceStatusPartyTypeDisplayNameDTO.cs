namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceStatusPartyTypeDisplayNameDTO
    {
        public Guid Id { get; set; }
        public Guid PartyTypeId { get; set; }
        public Guid StatusId { get; set; }
        public string PartyTypeNameEn { get; set; } = null!;
        public string PartyTypeNameAr { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public bool? IsActive { get; set; }

    }
}
