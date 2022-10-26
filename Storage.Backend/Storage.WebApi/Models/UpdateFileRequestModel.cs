using Newtonsoft.Json;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Update file request
    /// </summary>
    public class UpdateFileRequestModel
    {
        /// <summary>
        /// File's attributes
        /// </summary>
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; }
    }
}
