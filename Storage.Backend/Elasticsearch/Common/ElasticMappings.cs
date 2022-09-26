using Elasticsearch.Models;
using Nest;

namespace Elasticsearch.Common
{
    public static class ElasticMappings
    {
        /// <summary>
        /// Maps elastic search hits
        /// </summary>
        /// <typeparam name="T">Type of documents</typeparam>
        /// <param name="response">Search response</param>
        /// <returns>Mapped result</returns>
        public static Models.SearchResponse<T> MapHits<T>(this ISearchResponse<T> response) where T : class
        {
            var mapped = new Models.SearchResponse<T>()
            {
                Count = response.Total
            };

            var documents = new List<HitModel<T>>();

            foreach (var hit in response.Hits)
            {
                documents.Add(new HitModel<T>
                {
                    Score = hit.Score ?? 1,
                    Document = hit.Source
                });
            }

            mapped.Documents = documents;

            return mapped;

        }
    }
}
