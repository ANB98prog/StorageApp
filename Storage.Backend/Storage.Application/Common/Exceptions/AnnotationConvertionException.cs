using System;

namespace Storage.Application.Common.Exceptions
{
    /// <summary>
    /// Annotation convertion exception
    /// </summary>
    public class AnnotationConvertionException : BaseServiceException
    {
        public AnnotationConvertionException(string message) : base(message)
        {
        }

        public AnnotationConvertionException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
