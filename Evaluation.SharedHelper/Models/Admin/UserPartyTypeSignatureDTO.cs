

namespace Evaluation.SharedHelper.Models.Admin
{
    public class UserPartyTypeSignatureDTO : EntityBaseDTO
    {
        public Guid UserPartyTypeId { get; set; }
        public UserPartyTypeDTO? UserPartyType { get; set; }
        public byte[] Signature { get; set; } = null!;

    }
}
