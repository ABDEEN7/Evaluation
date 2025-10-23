using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Exceptions
{
    public class HttpResponseException : Exception
    {

        public HttpResponseException(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        public HttpResponseMessage HttpResponseMessage { get; }
    }
}
