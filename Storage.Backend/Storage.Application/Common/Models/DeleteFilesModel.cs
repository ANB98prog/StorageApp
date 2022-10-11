using Newtonsoft.Json;
using System.Collections.Generic;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Delete files model
    /// </summary>
    public class DeleteFilesModel : BaseDeleteFileModel
    {
        /// <summary>
        /// Deleting errors
        /// </summary>
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<DeleteErrorModel> Errors { get; set; }
    }
}
