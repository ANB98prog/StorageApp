using System.IO;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Annotated file data
    /// </summary>
    public class AnnotatedFileData : AnnotationFileInfo
    {
        /// <summary>
        /// File data stream
        /// </summary>
        public FileStream File { get; set; }
    }
}
