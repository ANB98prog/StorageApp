namespace Elasticsearch.Models
{
    /// <summary>
    /// Index many documents response model
    /// </summary>
    public class AddManyResponseModel
    {
        /// <summary>
        /// Indexed documents ids
        /// </summary>
        public IEnumerable<string> Ids { get; set; }

        /// <summary>
        /// Documents with errors
        /// </summary>
        public IEnumerable<AddDocumentError> ItemsWithErrors { get; set; }
    }
}
