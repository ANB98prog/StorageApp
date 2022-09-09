using System;

namespace Storage.Application.Common.Exceptions
{
    public class DirectoryCreationException : Exception
    {
        public DirectoryCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
