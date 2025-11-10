using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FieldErrorDTO
    {
        public Guid? fieldId { get; set; }
        public string error { get; set; }
    }
}
