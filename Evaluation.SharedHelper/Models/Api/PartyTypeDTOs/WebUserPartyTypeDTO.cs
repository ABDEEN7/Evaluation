


using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.PartyTypeDTOs;

namespace Evaluation.DAL.Models.Base
{
    public class WebUserPartyTypeDTO : EntityBaseDTO
	{
        public Guid UserId { get; set; }
        public string User { get; set; } = null!;
        public Guid PartyTypeId { get; set; }
        public PartyTypeDTO PartyType { get; set; } = new();
        public bool? ShowInquires { get; set; }
        public bool? ShowAllPartyType { get; set; }
        public bool CanViewAllRequests { get; set; }

        public string? SignaturePlaceHolder { get; set; }
        public Guid? SystemModuleId { get; set; }

    }
}
