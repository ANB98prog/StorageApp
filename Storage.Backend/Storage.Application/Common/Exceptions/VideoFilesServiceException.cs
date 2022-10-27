using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Video files service exception class
    /// </summary>
    public class VideoFilesServiceException : BaseServiceException
    {
        public VideoFilesServiceException(string message) : base(message)
        {
        }

        public VideoFilesServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public VideoFilesServiceException(string message, string userFriendlyMessage) : base(message, userFriendlyMessage)
        {
        }

        public VideoFilesServiceException(string message, string userFriendlyMessage, Exception innerException) : base(message, userFriendlyMessage, innerException)
        {
        }
    }
}
