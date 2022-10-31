using System.Collections.Generic;

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
        public long Total { get; set; }

        /// <summary>
        /// Found documents count
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Documents
        /// </summary>
        public IEnumerable<HitModel<T>> Documents { get; set; }

        /// <summary>
        /// Aggregations values
        /// </summary>
        public Dictionary<string, List<string>> Aggregations { get; set; } = new Dictionary<string, List<string>>();
    }
}
