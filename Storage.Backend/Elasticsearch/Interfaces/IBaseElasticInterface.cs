using Newtonsoft.Json;
using System;

namespace Elasticsearch.Interfaces
{
    public interface IBaseElasticInterface
    {
        /// <summary>
        /// File id
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
