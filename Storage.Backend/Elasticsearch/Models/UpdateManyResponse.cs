using System.Collections.Generic;

namespace Elasticsearch.Models
{
    /// <summary>
    /// Updated many documents response
    /// </summary>
    public class UpdateManyResponse
    {
        /// <summary>
        /// Acknowledged action
        /// </summary>
        public bool Acknowledged { get; set; }

        /// <summary>
        /// Documents with errors
        /// </summary>
        public IEnumerable<AddDocumentError> ItemsWithErrors { get; set; }

        /// <summary>
        /// Updated count
        /// </summary>
        public int Count { get; set; }
    }
}
