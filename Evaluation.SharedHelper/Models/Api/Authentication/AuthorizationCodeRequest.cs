using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.Authentication
{
    public class AuthorizationCodeRequest
    {
        public string code { get; set; } = null!;
        public string? json { get; set; }
    }
}
