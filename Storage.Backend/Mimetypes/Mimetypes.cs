using System.Collections.Generic;
using System.Text;

namespace Mimetypes
{
    public class Mimetypes
    {
        private readonly Dictionary<string, List<string>> _mimetypes = new Dictionary<string, List<string>>
        {
            {"text/csv", new List<string>{ ".csv" }},
            {"text/javascript", new List<string>{ ".js" }},
            {"text/plain", new List<string>{ ".txt" }},
            {"text/html", new List<string>{ ".htm", ".html" }},
            
            {"image/bmp", new List<string>{ ".bmp" }},
            {"image/gif", new List<string>{ ".gif" }},
            {"image/jpeg", new List<string>{ ".jpeg", ".jpg" }},
            {"image/png", new List<string>{ ".png" }},
            {"image/svg+xml", new List<string>{ ".svg" }},
            {"image/tiff", new List<string>{ ".tif", ".tiff" }},

            {"application/msword", new List<string>{ ".doc" }},
            {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", new List<string>{ ".docx" }},
            {"application/json", new List<string>{ ".json" }},
            {"application/vnd.oasis.opendocument.spreadsheet", new List<string>{ ".ods" }},
            {"application/vnd.oasis.opendocument.text", new List<string>{ ".odt" }},
            {"application/pdf", new List<string>{ ".pdf" }},
            {"application/vnd.rar", new List<string>{ ".rar" }},
            {"application/x-tar", new List<string>{ ".tar" }},
            {"application/zip", new List<string>{ ".zip" }},
            {"application/x-7z-compressed", new List<string>{ ".7z" }},
            {"application/vnd.ms-excel", new List<string>{ ".xls" }},
            {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new List<string>{ ".xlsx" }},
            {"application/xml", new List<string>{ ".xml" }},

            {"video/mp4", new List<string>{ ".mp4" }},
            {"video/mpeg", new List<string>{ ".mpeg" }},
            
            {"audio/wav", new List<string>{ ".wav" }},
            {"audio/mpeg", new List<string>{ ".mp3" }},
        };

        /// <summary>
        /// Gets mime type extensions
        /// </summary>
        /// <param name="mimetype">Mimetype</param>
        /// <returns>Available extension for passed mimetype</returns>
        public List<string> GetMimeTypeExtensions(string mimetype)
        {
            var extensions = new List<string>();

            if(string.IsNullOrWhiteSpace(mimetype))
                return extensions;

            if(mimetype.Contains("*"))
            {
                
            }
            else
            {
                if(_mimetypes.ContainsKey(mimetype))
                {
                    extensions = _mimetypes.GetValueOrDefault(mimetype);
                }
            }

            return extensions;
        }

        private List<string> GetExtensionsByAsteriskPattern(string mimetype)
        {
            return new List<string>();
        }

        /// <summary>
        /// Try to get mime type by file extension
        /// </summary>
        /// <param name="extension">File extension</param>
        /// <returns>Parsing result</returns>
        public bool TryGetMimeType(string extension, out string? mimetype)
        {
            return true;
        }
    }
}