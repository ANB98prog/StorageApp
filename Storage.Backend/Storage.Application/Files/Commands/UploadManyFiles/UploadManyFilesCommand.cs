using Microsoft.AspNetCore.Http;
using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.UploadManyFiles
{
    /// <summary>
    /// Upload many files command
    /// </summary>
    public class UploadManyFilesCommand : BaseUploadCommand<ManyFilesActionResponse<List<Guid>>>
    {
        /// <summary>
        /// Files data
        /// </summary>
        public IList<IFormFile> Files { get; set; }

        /// <summary>
        /// Mime type
        /// </summary>
        public List<string> MimeTypes { get; set; } =  new List<string>();
    }
}
