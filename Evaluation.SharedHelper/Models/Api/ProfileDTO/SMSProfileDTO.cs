using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.ProfileDTO
{
    public class SMSProfileDTO
    {

        public Guid Id { get; set; }
        public string BackendName { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsDefault { get; set; }


    }
}
