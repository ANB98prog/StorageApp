using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Exception describes error in FileHandlerService file
    /// </summary>
    public class FileHandlerServiceException : BaseServiceException
    {
        public FileHandlerServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FileHandlerServiceException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public FileHandlerServiceException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
