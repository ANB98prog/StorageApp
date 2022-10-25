using Newtonsoft.Json;

namespace Storage.Application.Common.Models
{
    public class BaseFileActionModel
    {
        /// <summary>
        /// Action acknowledge
        /// </summary>
        [JsonProperty("acknowledged")]
        public bool Acknowledged { get; set; } = true;
    }
}
