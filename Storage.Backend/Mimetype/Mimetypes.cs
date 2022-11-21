using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mimetype
{
    /// <summary>
    /// Class for working with mimeTypes
    /// </summary>
    public static class Mimetype
    {
        /// <summary>
        /// Available mimeTypes
        /// </summary>
        private static Dictionary<string, List<string>> _mimetypes = new Dictionary<string, List<string>>
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
            {"video/x-msvideo", new List<string>{ ".avi" }},
            {"video/x-ms-wmv", new List<string>{ ".wmv" }},
            {"video/webm", new List<string>{ ".webm" }},

            {"audio/wav", new List<string>{ ".wav" }},
            {"audio/mpeg", new List<string>{ ".mp3" }},
        };

        /// <summary>
        /// Gets mime type extensions
        /// </summary>
        /// <param name="mimetype">Mimetype</param>
        /// <returns>Available extension for passed mimetype</returns>
        public static List<string> GetMimeTypeExtensions(string mimetype)
        {
            try
            {
                var extensions = new List<string>();

                if (string.IsNullOrWhiteSpace(mimetype))
                    return extensions;

                if (mimetype.Contains("*"))
                {
                    extensions = GetExtensionsByAsteriskPattern(mimetype);
                }
                else
                {
                    if (_mimetypes.ContainsKey(mimetype))
                    {
                        extensions = _mimetypes.GetValueOrDefault(mimetype);
                    }
                }

                return extensions;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while get mimeType extensions", ex);
            }
        }

        /// <summary>
        /// Gets entensions by mimetype mask
        /// for example `image/*`
        /// </summary>
        /// <param name="mimetype">MimeType mask</param>
        /// <returns>Found files extensions</returns>
        private static List<string> GetExtensionsByAsteriskPattern(string mimetype)
        {
            var mimeTypes = new List<string>();

            var mimetypePattern = mimetype
                                        .Replace("/", @"\/")
                                            .Replace("*", @"[-.a-z1-9]*");

            var foundTypes = _mimetypes.Where(m => Regex.IsMatch(m.Key, mimetypePattern));

            foreach(var type in foundTypes)
            {
                mimeTypes.AddRange(type.Value);
            }

            return mimeTypes;
        }

        /// <summary>
        /// Try to get mime type by file extension
        /// </summary>
        /// <param name="fileName">File extension</param>
        /// <returns>Parsing result</returns>
        public static bool TryGetMimeType(string fileName, out string? mimetype)
        {
            try
            {
                mimetype = null;

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return false;
                }

                var extension = Path.GetExtension(fileName);

                mimetype = _mimetypes.FirstOrDefault(e => e.Value.Contains(extension)).Key ?? null;

                return (mimetype == null) 
                            ? false : true;  
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while get mimeType by file extension", ex);
            }
        }
    }
}
