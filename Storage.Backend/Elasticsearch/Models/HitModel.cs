using System.Collections.Generic;

namespace Elasticsearch.Models
{
    /// <summary>
    /// Elastic hit model
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class HitModel<T>
        where T : class
    {
        /// <summary>
        /// Hit score
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Document data
        /// </summary>
        public T Document { get; set; }

        /// <summary>
        /// Search highlights
        /// </summary>
        public List<string> Highlight { get; set; }
    }
}
