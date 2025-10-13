using Evaluation.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.DAL.Models.Exceptions
{
    public class ExceptionLog : EntityBase
    {
        public string ExceptionType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string StackTrace { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string? JsonParameter { get; set; }
        public string? Context { get; set; }
    }
}
