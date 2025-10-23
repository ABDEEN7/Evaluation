using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Exceptions
{
    public class EmailServicesException : Exception
    {

        public EmailServicesException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
