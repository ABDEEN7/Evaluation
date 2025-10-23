using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models
{
    public class ResponseInfo
    {
        public string Lang { get; set; } = null!;
        public PathString Path { get; set; }
        public string Method { get; set; } = null!;
        public DateTime ResponseTime { get; set; }
    }
}
