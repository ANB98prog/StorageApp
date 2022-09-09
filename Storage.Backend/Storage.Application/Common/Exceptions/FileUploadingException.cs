using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Exception describes error when uploading file
    /// </summary>
    public class FileUploadingException : Exception
    {
        public FileUploadingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
