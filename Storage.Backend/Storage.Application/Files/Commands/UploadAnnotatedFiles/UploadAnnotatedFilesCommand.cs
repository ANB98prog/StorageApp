using Storage.Application.Files.Commands.UploadManyFiles;
using Storage.Domain;

namespace Storage.Application.Files.Commands.UploadAnnotatedFiles
{
    /// <summary>
    /// Command for uploading annotated files
    /// </summary>
    public class UploadAnnotatedFilesCommand : UploadManyFilesCommand
    {
        /// <summary>
        /// Annotation format
        /// </summary>
        public AnnotationFormats AnnotationFormat { get; set; }
    }
}
