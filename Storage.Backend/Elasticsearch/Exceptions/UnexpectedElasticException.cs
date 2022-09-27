using System;
using System.Runtime.Serialization;

namespace Elasticsearch.Exceptions
{
    /// <summary>
    /// Describes unexpected elastic client exception
    /// </summary>
    public class UnexpectedElasticException : Exception
    {
        public string UserfriendlyMessage { get; private set; }

        public UnexpectedElasticException()
        {
        }

        public UnexpectedElasticException(string? message) : base(message)
        {
            UserfriendlyMessage = message;
        }

        public UnexpectedElasticException(string? message, Exception? innerException) : base(message, innerException)
        {
            UserfriendlyMessage = message;
        }

        protected UnexpectedElasticException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
