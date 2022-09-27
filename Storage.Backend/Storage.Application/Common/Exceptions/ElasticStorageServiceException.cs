using System;

namespace Storage.Application.Common.Exceptions
{
    public class ElasticStorageServiceException : BaseServiceException
    {
        public ElasticStorageServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ElasticStorageServiceException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public ElasticStorageServiceException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
