using Elasticsearch.Models;
using Nest;
using System.Collections.Generic;
using System.Linq;

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
        public static Models.SearchResponse<T> MapHits<T>(this ISearchResponse<T> response, List<string> aggregations) where T : class
        {
            var mapped = new Models.SearchResponse<T>()
            {
                Total = response.Total,
                Count = response.Hits.Count
            };

            var documents = new List<HitModel<T>>();

            foreach (var hit in response.Hits)
            {
                var highlight = new List<string>();

                if (hit.Highlight.Any())
                {
                    foreach (var h in hit.Highlight.Values)
                    {
                        highlight.AddRange(h);
                    }
                }

                documents.Add(new HitModel<T>
                {
                    Score = hit.Score ?? 1,
                    Document = hit.Source,
                    Highlight = highlight
                });
            }

            if(aggregations.Any()
                    && response.Aggregations != null
                        && response.Aggregations.Any())
            {
                foreach(var agg_key in aggregations)
                {
                    var compositeAggs = response.Aggregations.Composite(agg_key);

                    if(compositeAggs != null)
                    {
                        var buckets = compositeAggs.Buckets.Select(x => x.Key).ToList();

                        foreach(var bucket in buckets)
                        {
                            var attrs = bucket.Values.Select(o => o.ToString()).ToList();

                            if (mapped.Aggregations.ContainsKey(agg_key))
                            {
                                mapped.Aggregations[agg_key].AddRange(attrs);
                            }
                            else
                            {
                                mapped.Aggregations.Add(agg_key, new List<string>(attrs));
                            }                            
                        }
                    }
                }
            }

            mapped.Documents = documents;

            return mapped;

        }

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
                Total = response.Total,
                Count = response.Hits.Count
            };

            var documents = new List<HitModel<T>>();

            foreach (var hit in response.Hits)
            {
                var highlight = new List<string>();

                if (hit.Highlight.Any())
                {
                    foreach (var h in hit.Highlight.Values)
                    {
                        highlight.AddRange(h);
                    }
                }

                documents.Add(new HitModel<T>
                {
                    Score = hit.Score ?? 1,
                    Document = hit.Source,
                    Highlight = highlight
                });
            }

            mapped.Documents = documents;

            return mapped;

        }
    }
}
