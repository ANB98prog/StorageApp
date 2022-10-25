using Newtonsoft.Json;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Delete file model
    /// </summary>
    public class DeleteFileResponseModel : BaseDeleteFileModel
    {
        /// <summary>
        /// Error
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeleteErrorModel Error { get; set; }
    }
}
