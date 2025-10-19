using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models
{
    public class FieldErrorDTO
    {
        public Guid? fieldId { get; set; }
        public string error { get; set; }
    }
}
