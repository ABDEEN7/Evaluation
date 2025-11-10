


namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceDTO : EntityBaseDTO
    {
        public Guid SystemModuleId { get; set; }
        public string SystemModule { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string BackendName { get; set; } = null!;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        
        public string PrefixCode { get; set; } = null!;
        public string ReqNumberDef { get; set; } = null!;
        public bool? IsAutoAssignEnabled { get; set; }
        public string? ServiceSettings { get; set; }
        public bool Initialservice { get; set; }
        public bool IsFreez { get; set; }
        public DateTime? FreezDate { get; set; }
        public string? Icon { get; set; }
        public int OrderNo { get; set; }
        public string? Routing { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<ActionDTO>? Actions { get; set; }
        public string? Description { get; set; }
        public Guid[]? ServiceInitiatorPartyType { get; set; }
        public Guid[]? ServiceRequestShowPartyType { get; set; }

    }
}
