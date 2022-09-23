namespace Elasticsearch.Models
{
    /// <summary>
    /// Elastic search response
    /// </summary>
    /// <typeparam name="T">Documents type</typeparam>
    public class SearchResponse<T> where T : class
    {
        /// <summary>
        /// Total count
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Found documents count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Documents
        /// </summary>
        public IEnumerable<HitModel<T>> Documents { get; set; }
    }
}
