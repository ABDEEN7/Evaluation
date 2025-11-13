using Evaluation.SharedHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.PartyTypeDTOs
{
    public class PartyTypeDTO : EntityBaseDTO
	{
        public string Name { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? BackendName { get; set; }
        public bool IsEmployeePartyType { get; set; }
        public bool CanViewAllRequests { get; set; }
        public bool CanViewAllScholarships { get; set; }
        public string SystemModulesId { get; set; } = null!;
        public string? PartyTypeParentId { get; set; }
    }
}
