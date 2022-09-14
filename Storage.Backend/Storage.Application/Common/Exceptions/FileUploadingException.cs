using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Exception describes error when uploading file
    /// </summary>
    public class FileUploadingException : Exception
    {
        /// <summary>
        /// User friendly error message
        /// </summary>
        public string UserFriendlyMessage { get; set; }

        public FileUploadingException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = message;
        }

        public FileUploadingException(string message, string userFriendlyMessage)
            : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }

        public FileUploadingException(string message, string userFriendlyMessage, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }
    }
}
