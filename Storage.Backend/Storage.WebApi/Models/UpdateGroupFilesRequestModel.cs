using Newtonsoft.Json;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Update group of files request model
    /// </summary>
    public class UpdateGroupFilesRequestModel
    {
        /// <summary>
        /// List of files ids to update
        /// </summary>
        [JsonProperty("filesIds")]
        public List<Guid> FilesIds { get; set; }

        /// <summary>
        /// List of attributes
        /// </summary>
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; }
    }
}
