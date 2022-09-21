using System.Runtime.Serialization;

namespace Elasticsearch.Exceptions
{
    /// <summary>
    /// Describes unexpected elastic client exception
    /// </summary>
    public class UnexpectedElasticException : Exception
    {
        public UnexpectedElasticException()
        {
        }

        public UnexpectedElasticException(string? message) : base(message)
        {
        }

        public UnexpectedElasticException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnexpectedElasticException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
