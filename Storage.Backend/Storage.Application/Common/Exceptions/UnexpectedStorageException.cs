using System;

namespace Storage.Application.Common.Exceptions
{
    public class UnexpectedStorageException : BaseServiceException
    {
        public UnexpectedStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnexpectedStorageException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public UnexpectedStorageException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
