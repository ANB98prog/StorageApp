using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Common;
using Storage.Domain;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Queries.Models
{
    /// <summary>
    /// File view model
    /// </summary>
    public class FileVm : IMapWith<BaseFile>
    {
        /// <summary>
        /// File id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// File owner id
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// File department owner id
        /// </summary>
        public Guid DepartmentOwnerId { get; set; }

        /// <summary>
        /// File url
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// File attributes
        /// </summary>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// File mime type
        /// </summary>
        public string MimeType { 
            get
            {
                if (!string.IsNullOrWhiteSpace(OriginalFileName)
                    && MimeTypes.TryGetMimeType(OriginalFileName, out var mimeType))
                {
                    return mimeType;
                }

                return Constants.DEFAULT_MIME_TYPE;
            }
        }

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

        /// <summary>
        /// File annotation
        /// </summary>
        [JsonProperty("annotation")]
        public AnnotationMetadata Annotation { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BaseFile, FileVm>()
                 .ForMember(model => model.Id,
                     opt => opt.MapFrom(upload => upload.Id))
                 .ForMember(model => model.OriginalFileName,
                     opt => opt.MapFrom(upload => upload.OriginalName))
                 .ForMember(model => model.Attributes,
                     opt => opt.MapFrom(upload => upload.Attributes))
                 .ForMember(model => model.CreatedAt,
                     opt => opt.MapFrom(upload => upload.CreatedAt))
                 .ForMember(model => model.EditedAt,
                     opt => opt.MapFrom(upload => upload.EditedAt))
                 .ForMember(model => model.DepartmentOwnerId,
                     opt => opt.MapFrom(upload => upload.DepartmentOwnerId))
                 .ForMember(model => model.OwnerId,
                     opt => opt.MapFrom(upload => upload.OwnerId))
                 .ForMember(model => model.FileUrl,
                     opt => opt.MapFrom(upload => upload.FileUrl))
                 .ForMember(model => model.IsAnnotated,
                     opt => opt.MapFrom(upload => upload.IsAnnotated))
                 .ForMember(model => model.Annotation,
                     opt => opt.MapFrom(upload => upload.Annotation));
        }
    }
}
