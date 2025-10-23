using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Exceptions
{
    public class MissingConfigException : Exception
    {
        public MissingConfigException(string message) : base(message)
        {

        }
    }
}
