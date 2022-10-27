using Newtonsoft.Json;
using System.Collections.Generic;

namespace Storage.Application.Common.Models
{
    public class ManyFilesActionResponse<T> where T : class
    {
        /// <summary>
        /// Response data
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// Deleting errors
        /// </summary>
        [JsonProperty("errors")]
        public List<DeleteErrorModel> Errors { get; set; }
    }
}
