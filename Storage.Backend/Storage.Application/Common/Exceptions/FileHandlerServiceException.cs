using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Exception describes error in FileHandlerService file
    /// </summary>
    public class FileHandlerServiceException : Exception
    {
        /// <summary>
        /// User friendly error message
        /// </summary>
        public string UserFriendlyMessage { get; set; }

        public FileHandlerServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = message;
        }

        public FileHandlerServiceException(string message, string userFriendlyMessage)
            : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }

        public FileHandlerServiceException(string message, string userFriendlyMessage, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }
    }
}
