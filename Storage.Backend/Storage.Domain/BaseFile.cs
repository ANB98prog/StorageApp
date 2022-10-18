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

        /// <summary>
        /// Initializes class instance of <see cref="AnnotatedClass"/>
        /// </summary>
        /// <param name="index">Class index</param>
        /// <param name="name">Class name</param>
        public AnnotatedClass(int index, string name)
        {
            ClassIndex = index;
            ClassName = name;
        }
    }

    public class Annotation
    {
        /// <summary>
        /// Class index
        /// </summary>
        [JsonProperty("classIndex")]
        public int ClassIndex { get; set; }

        /// <summary>
        /// Bounding box
        /// </summary>
        [JsonProperty("bboxes")]
        public BoudingBox Bboxes { get; set; }
    }

    /// <summary>
    /// Annotation bounding box
    /// </summary>
    public class BoudingBox
    {
        /// <summary>
        /// Image info
        /// </summary>
        public AnnotationImageInfo ImageInfo { get; }

        /// <summary>
        /// Annotation with relative coordinates
        /// </summary>
        public RelativeAnnotationBbox RelativeAnnotation { get; }

        /// <summary>
        /// Annotation with pixels coordinates
        /// </summary>
        public PixelsAnnotationBbox PixelsAnnotation { get; }

        /// <summary>
        /// Annotation with polygons
        /// </summary>
        public PolygonAnnotationBbox PolygonAnnotation { get; }

        /// <summary>
        /// Initializes class instance of <see cref="BoudingBox"/>
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <param name="relativeAnnotation">Annotation with relative coordinates</param>
        public BoudingBox(AnnotationImageInfo imageInfo, RelativeAnnotationBbox relativeAnnotation)
        {
            ImageInfo = imageInfo;
            RelativeAnnotation = relativeAnnotation;

            PixelsAnnotation = RelativeAnnotation.ConvertToPixels(imageInfo);
            PolygonAnnotation = PixelsAnnotation.ConvertToPolygons(imageInfo);
        }

        /// <summary>
        /// Initializes class instance of <see cref="BoudingBox"/>
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <param name="pixelsAnnotation">Annotation with pixels coordinates</param>
        public BoudingBox(AnnotationImageInfo imageInfo, PixelsAnnotationBbox pixelsAnnotation)
        {
            ImageInfo = imageInfo;
            PixelsAnnotation = pixelsAnnotation;

            RelativeAnnotation = PixelsAnnotation.ConvertToRelative(imageInfo);
            PolygonAnnotation = PixelsAnnotation.ConvertToPolygons(imageInfo);
        }

        /// <summary>
        /// Initializes class instance of <see cref="BoudingBox"/>
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <param name="polygonAnnotation">Annotation with polygons</param>
        public BoudingBox(AnnotationImageInfo imageInfo, PolygonAnnotationBbox polygonAnnotation)
        {
            ImageInfo = imageInfo;
            PolygonAnnotation = polygonAnnotation;

            PixelsAnnotation = PolygonAnnotation.ConvertToPixels(imageInfo);
            RelativeAnnotation = PixelsAnnotation.ConvertToRelative(imageInfo);
        }
    }

    /// <summary>
    /// Image info
    /// </summary>
    public class AnnotationImageInfo
    {
        public int Height { get; set; }
        public int Width { get; set; }

        /// <summary>
        /// Initializes class instance of <see cref="AnnotationImageInfo"/>
        /// </summary>
        /// <param name="width">Image with</param>
        /// <param name="height">Image height</param>
        public AnnotationImageInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Bounting box with image relative coordianates
    /// </summary>
    public class RelativeAnnotationBbox
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// BBox height
        /// </summary>
        public float H { get; set; }

        /// <summary>
        /// BBox width
        /// </summary>
        public float W { get; set; }

        /// <summary>
        /// Initializes class instance of <see cref="RelativeAnnotationBbox"/>
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="w">BBox height</param>
        /// <param name="h">BBox width</param>
        public RelativeAnnotationBbox(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            H = h;
            W = w;
        }

        /// <summary>
        /// Converts to pixels coordinates
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <returns>Bbox with pixels coordinates</returns>
        public PixelsAnnotationBbox ConvertToPixels(AnnotationImageInfo imageInfo)
        {
            int x1 = (int)Math.Ceiling((X-(W/2))*imageInfo.Width);
            int y1 = (int)Math.Ceiling((Y - (H / 2)) * imageInfo.Height);

            int w = (int)Math.Ceiling(W * imageInfo.Width);
            int h = (int)Math.Ceiling(H * imageInfo.Height);

            int x2 = x1 + w;
            int y2 = y1 + h;

            return new PixelsAnnotationBbox(x1, y1, x2, y2);
        }

        /// <summary>
        /// Converts to polygons
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <returns></returns>
        public PolygonAnnotationBbox ConvertToPolygons(AnnotationImageInfo imageInfo)
        {
            return new PolygonAnnotationBbox( new float[] {0f, 0f, 0f, 0f});
        }
    }

    /// <summary>
    /// Bounting box with image pixel coordianates
    /// </summary>
    public class PixelsAnnotationBbox
    {
        /// <summary>
        /// Upper left 'x' coord
        /// </summary>
        public int X1 { get; set; }

        /// <summary>
        /// Upper left 'y' coord
        /// </summary>
        public int Y1 { get; set; }

        /// <summary>
        /// Lower right 'x' coord
        /// </summary>
        public int X2 { get; set; }

        /// <summary>
        /// Lower right 'y' coord  
        /// </summary>
        public int Y2 { get; set; }

        /// <summary>
        /// Initializes class instance of <see cref="PixelsAnnotationBbox"/>
        /// </summary>
        /// <param name="x1">Upper left 'x' coord</param>
        /// <param name="y1">Upper left 'y' coord</param>
        /// <param name="x2">Lower right 'x' coord </param>
        /// <param name="y2">Lower right 'y' coord </param>
        public PixelsAnnotationBbox(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// Converts to relative coordinates
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <returns></returns>
        public RelativeAnnotationBbox ConvertToRelative(AnnotationImageInfo imageInfo)
        {
            float w_rel = Math.Abs(X1 - X2) / (float)imageInfo.Width;
            float h_rel = Math.Abs(Y1 - Y2) / (float)imageInfo.Height;

            float x_rel = (X1 / (float)imageInfo.Width) + (float)w_rel / 2;
            float y_rel = (Y1 / (float)imageInfo.Height) + (float)h_rel / 2;

            return new RelativeAnnotationBbox(x_rel, y_rel, w_rel, h_rel);
        }

        /// <summary>
        /// Converts to polygons
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <returns></returns>
        public PolygonAnnotationBbox ConvertToPolygons(AnnotationImageInfo imageInfo)
        {
            return new PolygonAnnotationBbox(new float[] { 0f, 0f, 0f, 0f });
        }
    }

    /// <summary>
    /// Bounding box with polygons
    /// </summary>
    public class PolygonAnnotationBbox
    {
        /// <summary>
        /// Polygon points
        /// </summary>
        public float[] Points { get; set; }

        /// <summary>
        /// Initializes class instance of <see cref="PolygonAnnotationBbox"/>
        /// </summary>
        /// <param name="points">Polygon points</param>
        public PolygonAnnotationBbox(float[] points)
        {
           Points = points;
        }

        /// <summary>
        /// Converts to relative coordinates
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <returns></returns>
        public RelativeAnnotationBbox ConvertToRelative(AnnotationImageInfo imageInfo)
        {
            return new RelativeAnnotationBbox(0f, 0f, 0f, 0f);
        }

        /// <summary>
        /// Converts to pixels coordinates
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <returns></returns>
        public PixelsAnnotationBbox ConvertToPixels(AnnotationImageInfo imageInfo)
        {
            return new PixelsAnnotationBbox(0, 0, 0, 0);
        }
    }
}
