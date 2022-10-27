using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Exception that occures due user's input
    /// </summary>
    public class UserException : BaseServiceException
    {
        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public UserException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
