using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class AssignUserDTO:EntityBaseDTO
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Email { get; set; }
        public bool IsSelected { get; set; }
        public bool IsLeader { get; set; }
        public bool ShowIsDefaultAssigner { get; set; }
        public int? CurrentWorkLoad { get; set; }
        public Guid? PartyTypeId { get; set; }
        public string? PartyTypeTitle { get; set; }
        public string? PartyTypeTitleAr { get; set; }
        public string? PartyTypeTitleEn { get; set; }
    }
}
