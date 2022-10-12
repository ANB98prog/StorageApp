using Microsoft.AspNetCore.Http;
using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.UploadManyFiles
{
    /// <summary>
    /// Upload many files command
    /// </summary>
    public class UploadManyFilesCommand : BaseCommand<List<Guid>>
    {
        /// <summary>
        /// Files attributes
        /// </summary>
        public List<string> Attributes { get; set; } = new List<string>();

        /// <summary>
        /// Is files annotated
        /// </summary>
        public bool IsAnnotated { get; set; } = false;

        /// <summary>
        /// Files data
        /// </summary>
        public IList<IFormFile> Files { get; set; }
    }
}
