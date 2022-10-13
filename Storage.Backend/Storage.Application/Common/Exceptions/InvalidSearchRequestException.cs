using System;

namespace Storage.Application.Common.Exceptions
{
    public class InvalidSearchRequestException : BaseServiceException
    {
        public InvalidSearchRequestException(string message) : base(message)
        {
        }

        public InvalidSearchRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidSearchRequestException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public InvalidSearchRequestException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
