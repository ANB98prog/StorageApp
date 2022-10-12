using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.UploadManyFilesArchive
{
    /// <summary>
    /// Upload many file in zip archive
    /// </summary>
    public class UploadManyFilesArchiveCommand : BaseUploadCommand<List<Guid>>
    {
    }
}
