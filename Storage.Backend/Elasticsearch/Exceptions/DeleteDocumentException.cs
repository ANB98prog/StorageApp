using System;

namespace Elasticsearch.Exceptions
{
    public class DeleteDocumentException : UnexpectedElasticException
    {
        public DeleteDocumentException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
