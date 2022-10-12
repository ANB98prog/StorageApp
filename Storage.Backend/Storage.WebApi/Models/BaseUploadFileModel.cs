using Newtonsoft.Json;

namespace Storage.WebApi.Models
{
    public class BaseUploadFileModel
    {
        /// <summary>
        /// File attributes
        /// </summary>
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; }
    }
}
