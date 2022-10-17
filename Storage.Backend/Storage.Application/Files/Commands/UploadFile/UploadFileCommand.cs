using Microsoft.AspNetCore.Http;
using Storage.Application.Common;
using Storage.Application.Common.Models;

namespace Storage.Application.Files.Commands.UploadFile
{
    /// <summary>
    /// Upload file command
    /// </summary>
    public class UploadFileCommand : BaseUploadCommand<string>
    {
        /// <summary>
        /// File data
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// File data mime type
        /// </summary>
        public string MimeType
        {
            get
            {
                if (File != null
                    && !string.IsNullOrEmpty(File.FileName)
                        && MimeTypes.TryGetMimeType(File.FileName, out var mimeType))
                {
                    return mimeType;
                }

                return Constants.DEFAULT_MIME_TYPE;
            }
        }
    }
}
