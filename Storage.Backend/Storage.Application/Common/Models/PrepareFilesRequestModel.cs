using System.Collections.Generic;
using System.ComponentModel;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Prepare files request model
    /// </summary>
    public class PrepareFilesRequestModel
    {
        /// <summary>
        /// Files path
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// Archive type
        /// </summary>
        public ArchiveType ArchiveType { get; set; }
    }

    /// <summary>
    /// Archive type
    /// </summary>
    public enum ArchiveType 
    {
        [Description("zip")]
        Zip,

        [Description("rar")]
        Rar
    }
}
