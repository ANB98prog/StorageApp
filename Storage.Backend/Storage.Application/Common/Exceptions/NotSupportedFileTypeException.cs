using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Not supported file type
    /// </summary>
    public class NotSupportedFileTypeException : Exception
    {
        /// <summary>
        /// File type
        /// </summary>
        public string FileType { get; private set; }

        /// <summary>
        /// Initiates class instance of <see cref="NotSupportedFileTypeException"/>
        /// </summary>
        /// <param name="fileType">File type</param>
        /// <param name="message">Error message</param>
        public NotSupportedFileTypeException(string fileType, string message)
            :base(message)
        {
            FileType = fileType;
        }
    }
}
