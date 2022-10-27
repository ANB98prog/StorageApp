using Newtonsoft.Json;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Delete error model
    /// </summary>
    public class DeleteErrorModel
    {
        /// <summary>
        /// File id
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FileId { get; set; }

        /// <summary>
        /// File path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
