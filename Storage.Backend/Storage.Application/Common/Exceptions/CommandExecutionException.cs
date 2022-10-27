using System;
namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Exception when command execution
    /// </summary>
    public class CommandExecutionException : BaseServiceException
    {
        public CommandExecutionException(string message) : base(message)
        {
        }

        public CommandExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommandExecutionException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public CommandExecutionException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
