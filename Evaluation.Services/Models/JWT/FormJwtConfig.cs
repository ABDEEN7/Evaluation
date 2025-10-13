using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Models.JWT
{
    public class FormJwtConfig
    {
        public string Key { get; set; } = null!;
        public int ExpirationTime { get; set; }//minutes
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;


    }
}
