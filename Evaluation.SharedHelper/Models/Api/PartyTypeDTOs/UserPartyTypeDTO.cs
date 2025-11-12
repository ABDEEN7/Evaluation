
namespace Evaluation.SharedHelper.Models.Api.PartyTypeDTOs
{
    public class UserPartyTypeDTO : EntityBaseDTO
    {
        public Guid UserId { get; set; }
        public string User { get; set; } = null!;
        public Guid PartyTypeId { get; set; }
        public string PartyType { get; set; } = null!;
        public bool? ShowInquires { get; set; }
        public bool? ShowAllPartyType { get; set; }
        public string? SignaturePlaceHolder { get; set; }
        public bool CanViewAllRequests { get; set; }
        public bool CanViewAllEvaluations { get; set; }
        public Guid? SystemModuleId { get; set; }
        

    }
}
