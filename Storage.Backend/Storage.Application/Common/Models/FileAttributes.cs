using System.ComponentModel;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// File attributes
    /// </summary>
    public enum FileAttributes
    {
        /// <summary>
        /// Annotated files
        /// </summary>
        [Description("annotated")]
        Annotated,
        /// <summary>
        /// Not annotated files
        /// </summary>
        [Description("notAnnotated")]
        NotAnnotated
    }
}
