namespace Elasticsearch.Models
{
    /// <summary>
    /// Document error
    /// </summary>
    public class AddDocumentError
    {
        /// <summary>
        /// Document id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; set; }
    }
}
