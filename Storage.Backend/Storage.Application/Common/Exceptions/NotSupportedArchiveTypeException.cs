using System;

namespace Storage.Application.Common.Exceptions
{
    public class NotSupportedArchiveTypeException : BaseServiceException
    {
        public NotSupportedArchiveTypeException(string archiveType) : base(ErrorMessages.NotSupportedArchiveTypeErrorMessage(archiveType))
        {
        }

        public NotSupportedArchiveTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotSupportedArchiveTypeException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public NotSupportedArchiveTypeException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
