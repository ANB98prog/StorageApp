using Newtonsoft.Json;
using Storage.Domain;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Annotation file info
    /// </summary>
    public class AnnotationFileInfo : FileInfoModel
    {
        /// <summary>
        /// File annotation
        /// </summary>
        [JsonProperty("annotation")]
        public AnnotationMetadata Annotation { get; set; }
    }
}
