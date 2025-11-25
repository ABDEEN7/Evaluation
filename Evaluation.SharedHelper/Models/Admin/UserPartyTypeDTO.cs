


namespace Evaluation.SharedHelper.Models.Admin
{
    public class UserPartyTypeDTO : EntityBaseDTO
    {
        public Guid UserId { get; set; }
        public Guid? SystemModuleId { get; set; }
        public string? User { get; set; }
        public Guid PartyTypeId { get; set; }
        public string? PartyType { get; set; }
        public string? SignaturePlaceHolder { get; set; }


    }
}
