using Evaluation.SharedHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Exceptions
{
    public class BusinessException : Exception
    {
        public List<FieldErrorDTO> FieldErrors { get; }
        public BusinessException(string message) : base(message)
        {

        }
        public BusinessException(List<FieldErrorDTO> errors)
        : base("Validation failed")
        {
            FieldErrors = errors;
        }
    }
}
