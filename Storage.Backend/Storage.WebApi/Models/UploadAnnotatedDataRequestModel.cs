using Storage.Domain;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Upload annotated data request
    /// </summary>
    public class UploadAnnotatedDataRequestModel : UploadFileRequestModel
    {
        /// <summary>
        /// Annotation format
        /// </summary>
        public AnnotationFormats AnnotationFormat { get; set; }
    }
}
