using System;

namespace Storage.Application.Common.Exceptions
{
    public class ServiceArgumentException : BaseServiceException
    {
        public ServiceArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ServiceArgumentException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public ServiceArgumentException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
