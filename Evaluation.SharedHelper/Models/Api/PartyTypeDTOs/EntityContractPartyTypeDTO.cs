


using Evaluation.SharedHelper.Models;

namespace Evaluation.SharedHelper.Models.Api.PartyTypeDTOs
{
	public class EntityContractPartyTypeDTO : EntityBaseDTO
	{
        public Guid PartyTypeId { get; set; }
		public Guid EntityContractId { get; set; }
		public Guid SectorId { get; set; }
	}
}
