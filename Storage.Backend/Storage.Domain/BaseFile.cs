using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Storage.Domain
{
    public class BaseFile
    {
        /// <summary>
        /// Image id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Image owner id
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Image department owner id
        /// </summary>
        public Guid DepartmentOwnerId { get; set; }

        /// <summary>
        /// Original image name
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// System image name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// File type
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FileType FileType { 
            get
            {
                if (!string.IsNullOrWhiteSpace(OriginalName))
                {
                    return GetFileType(OriginalName);
                }

                return FileType.Unknown;
            } 
        }

        /// <summary>
        /// File path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// File url
        /// </summary>
        public string FileUrl { 
            get
            {
                if (!string.IsNullOrWhiteSpace(FilePath))
                {
                    return string.Join("/", (FilePath.Split(Path.DirectorySeparatorChar)));
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Image attributes
        /// </summary>
        public IEnumerable<string> Attributes { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        /// <summary>
        /// Edited at
        /// </summary>
        public DateTime EditedAt { get; set; }

        /// <summary>
        /// Describes is file annotated
        /// </summary>
        public bool IsAnnotated { get; set; }

        public static FileType GetFileType(string filePath)
        {
            var types = new Dictionary<FileType, List<string>>
            {
                { FileType.Text, new List<string> { ".txt", ".docx"} },
                { FileType.Image, new List<string> { ".jpg", ".png", ".jpeg", ".bmp", ".tif", ".tiff", ".gif" } },
                { FileType.Video, new List<string> { ".mp4", ".avi", ".mpg", ".mpeg", ".wmv" } },
                { FileType.Audio, new List<string> { ".mp3", ".wav", ".wma", ".mid", ".midi", ".aiff", ".au" } },
            };

            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath));
                }

                return types.FirstOrDefault(t =>
                        t.Value.Contains(Path.GetExtension(filePath).ToLowerInvariant()))
                            .Key;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while get file type", ex);
            }
        }
    }

    /// <summary>
    /// Uploaded file path
    /// </summary>
    public class FilePath
    {
        /// <summary>
        /// Fill (absolute) file path
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Relative file path
        /// </summary>
        public string RelativePath { get; set; }
    }

    /// <summary>
    /// File types
    /// </summary>
    public enum FileType
    {
        [Description("unknown")]
        Unknown,
        [Description("text")]
        Text,
        [Description("image")]
        Image,
        [Description("video")]
        Video,
        [Description("audio")]
        Audio,
        [Description("zip")]
        Zip
    }
}
