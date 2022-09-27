using System;

namespace Storage.Application.Common.Exceptions
{
    public class LocalStorageException : Exception
    {
        /// <summary>
        /// User friendly error message
        /// </summary>
        public string UserFriendlyMessage { get; set; }

        public LocalStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = message;
        }

        public LocalStorageException(string message, string userFriendlyMessage)
            : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }

        public LocalStorageException(string message, string userFriendlyMessage, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }
    }
}
