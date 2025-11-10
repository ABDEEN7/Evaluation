using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.PartyTypeDTOs
{
    public class UserProfileCustomDTO
    {
        public string Name { get; set; } = null!;
        public string QID { get; set; } = null!;
        public string QIDExpiry { get; set; } = null!;
        public string DOB { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string SecondMobile { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string profession { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsMinistryApplicant { get; set; }
    }
}
