using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Storage.Domain
{
    public class BaseFile
    {
        /// <summary>
        /// Image id
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Image owner id
        /// </summary>
        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Image department owner id
        /// </summary>
        [JsonProperty("departmentOwnerId")]
        public Guid DepartmentOwnerId { get; set; }

        /// <summary>
        /// Original image name
        /// </summary>
        [JsonProperty("originalName")]
        public string OriginalName { get; set; }

        /// <summary>
        /// System image name
        /// </summary>
        [JsonProperty("systemName")]
        public string SystemName { get; set; }

        /// <summary>
        /// File Mime type
        /// </summary>
        [JsonProperty("mimeType")]
        public string MimeType { get; set;}

        /// <summary>
        /// File path
        /// </summary>
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        /// <summary>
        /// File url
        /// </summary>
        [JsonProperty("fileUrl")]
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
        [JsonProperty("attributes")]
        public IEnumerable<string> Attributes { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Edited at
        /// </summary>
        [JsonProperty("editedAt")]
        public DateTime EditedAt { get; set; }

        /// <summary>
        /// Describes is file annotated
        /// </summary>
        [JsonProperty("isAnnotated")]
        public bool IsAnnotated { get; set; }

        /// <summary>
        /// File annotation
        /// </summary>
        [JsonProperty("Annotation", NullValueHandling = NullValueHandling.Ignore)]
        public AnnotationMetadata Annotation { get; set; }
    }

    /// <summary>
    /// Uploaded file path
    /// </summary>
    public class FilePath
    {
        /// <summary>
        /// Fill (absolute) file path
        /// </summary>
        [JsonProperty("fullPath")]
        public string FullPath { get; set; }

        /// <summary>
        /// Relative file path
        /// </summary>
        [JsonProperty("relativePath")]
        public string RelativePath { get; set; }
    }

    /// <summary>
    /// Annotation metadata
    /// </summary>
    public class AnnotationMetadata
    {
        /// <summary>
        /// Annotation classes
        /// </summary>
        [JsonProperty("classes")]
        public List<AnnotatedClass> Classes { get; set; }

        /// <summary>
        /// Annotation
        /// </summary>
        [JsonProperty("annotations")]
        public List<Annotation> Annotations { get; set; }
    }

    /// <summary>
    /// Annotation class info
    /// </summary>
    public class AnnotatedClass
    {
        /// <summary>
        /// Class index
        /// </summary>
        [JsonProperty("classIndex")]
        public int ClassIndex { get; set; }

        /// <summary>
        /// Class name
        /// </summary>
        [JsonProperty("className")]
        public string ClassName { get; set; }
    }

    public class Annotation
    {
        /// <summary>
        /// Class index
        /// </summary>
        [JsonProperty("classIndex")]
        public int ClassIndex { get; set; }

        /// <summary>
        /// Bounding boxes
        /// </summary>
        [JsonProperty("bboxes")]
        public float[] Bboxes { get; set; }
    }
}
