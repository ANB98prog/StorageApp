using System;

namespace Storage.Application.Common.Exceptions
{
    public class BaseServiceException : Exception
    {
        /// <summary>
        /// User friendly error message
        /// </summary>
        public string UserFriendlyMessage { get; set; }

        public BaseServiceException(string message)
            : base(message)
        {
            UserFriendlyMessage = message;
        }

        public BaseServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = message;
        }

        public BaseServiceException(string message, string userFriendlyMessage)
            : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }

        public BaseServiceException(string message, string userFriendlyMessage, Exception innerException)
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }
    }
}
