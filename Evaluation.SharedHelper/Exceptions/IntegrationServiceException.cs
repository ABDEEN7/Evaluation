using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Exceptions
{
    public class IntegrationServiceException : Exception
    {
        private readonly string friendlyErrorMessage;
        private readonly string actualErrorMessage;
        private readonly Exception exception;
        private readonly string additionalErrorMessage;

        public IntegrationServiceException(string friendlyErrorMessage, string actualErrorMessage,
            Exception exception, string additionalErrorMessage = null)
        {
            this.friendlyErrorMessage = friendlyErrorMessage;
            this.actualErrorMessage = actualErrorMessage;
            this.exception = exception;
            this.additionalErrorMessage = additionalErrorMessage;
        }

        public string FriendlyErrorMessage { get { return friendlyErrorMessage; } }
        public string ActualErrorMessage { get { return actualErrorMessage; } }

        public Exception Exception { get { return exception; } }
        public string AdditionalErrorMessage { get { return additionalErrorMessage; } }


    }
}
