using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Storage.Application.Common.Models
{
    public class BaseUploadCommand<TResponse> 
        : BaseCommand<TResponse> where TResponse : class
    {
        /// <summary>
        /// File attributes
        /// </summary>
        public List<string> Attributes { get; set; } = new List<string>();

        /// <summary>
        /// Is data annotated
        /// </summary>
        public bool IsAnnotated { get; set; } = false;

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
