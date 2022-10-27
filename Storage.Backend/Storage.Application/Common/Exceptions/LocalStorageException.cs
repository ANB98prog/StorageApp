using System;

namespace Storage.Application.Common.Exceptions
{
    public class LocalStorageException : BaseServiceException
    {
        public LocalStorageException(string message) : base(message)
        {
        }

        public LocalStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LocalStorageException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public LocalStorageException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
