namespace Elasticsearch.Exceptions
{
    /// <summary>
    /// Error that describes elastic index creation
    /// </summary>
    public class IndexCreationException : Exception
    {
        /// <summary>
        /// Index name
        /// </summary>
        public string IndexName { get; private set; }

        /// <summary>
        /// User friendly message
        /// </summary>
        public string UserfriendlyMessage { get; private set; }

        /// <summary>
        /// Creates exception of <see cref="IndexCreationException"/>
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="innerException">Inner exception</param>
        public IndexCreationException(string indexName, Exception innerException)
            : base(ErrorMessages.ERROR_CREATING_INDEX(indexName), innerException)
        {
            IndexName = indexName;
            UserfriendlyMessage = ErrorMessages.ERROR_CREATING_INDEX(indexName);            
        }

        /// <summary>
        /// Creates exception of <see cref="IndexCreationException"/>
        /// </summary>
        /// <param name="indexName">Index name</param>
        public IndexCreationException(string indexName, string message)
            : base(message)
        {
            IndexName = indexName;
            UserfriendlyMessage = message;
        }

        /// <summary>
        /// Creates exception of <see cref="IndexCreationException"/>
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="innerException">Inner exception</param>
        public IndexCreationException(string indexName, string message, Exception innerException)
            : base(message, innerException)
        {
            IndexName = indexName;
            UserfriendlyMessage = message;
        }
    }
}
