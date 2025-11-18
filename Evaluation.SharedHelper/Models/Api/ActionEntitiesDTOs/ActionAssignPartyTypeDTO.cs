using Evaluation.DAL.Models.ActionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionAssignPartyTypeDTO : EntityBaseDTO
    {
        public string ServiceActionId { get; set; } = null!;
        public ServiceAction ServiceAction { get; set; } = null!;
        public string PartyTypeId { get; set; } = null!;
        public int MaximumAssignedUser { get; set; }
    }
}
