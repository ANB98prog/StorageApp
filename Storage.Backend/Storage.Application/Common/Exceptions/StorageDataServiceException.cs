using System;

namespace Storage.Application.Common.Exceptions
{
    public class StorageDataServiceException : BaseServiceException
    {
        public StorageDataServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public StorageDataServiceException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public StorageDataServiceException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
