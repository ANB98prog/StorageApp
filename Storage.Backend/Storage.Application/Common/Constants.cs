using System.Collections.Generic;

namespace Storage.Application.Common
{
    public static class Constants
    {
        /// <summary>
        /// Default page size
        /// </summary>
        public const int DEFAULT_PAGE_SIZE = 10;

        /// <summary>
        /// Max page size
        /// </summary>
        public const int MAX_PAGE_SIZE = 100;

        /// <summary>
        /// Default page number
        /// </summary>
        public const int DEFAULT_PAGE_NUMBER = 0;

        /// <summary>
        /// Default mime type
        /// </summary>
        public const string DEFAULT_MIME_TYPE = "plain/text";

        /// <summary>
        /// Supported annotations files mimetypes
        /// </summary>
        public static List<string> ANNOTATION_MIMETYPES = new List<string>()
        {
            "image/gif",
            "image/png",
            "text/plain",
            "image/tiff",
            "image/jpeg",
            "application/xml",
            "application/json",
            "image/bmp"
        };

        /// <summary>
        /// Supported images mimetypes
        /// </summary>
        public static List<string> IMAGES_MIMETYPES = new List<string>()
        {
            "image/gif",
            "image/png",
            "image/tiff",
            "image/jpeg",
            "image/bmp",
            "image/svg+xml",
            "image/webp"
        };
    }
}
