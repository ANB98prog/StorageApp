using AutoMapper;
using Mapper;
using Storage.Domain;
using System;
using System.Collections.Generic;

namespace Storage.Application.Images.Queries.GetImage
{
    /// <summary>
    /// Image view model
    /// </summary>
    public class ImageVm : IMapWith<BaseFile>
    {
        /// <summary>
        /// File id
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
        /// Image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Image attributes
        /// </summary>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        public string FileExtension { get; set; }

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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BaseFile, ImageVm>()
                 .ForMember(model => model.Id,
                     opt => opt.MapFrom(upload => upload.Id))
                 .ForMember(model => model.OriginalFileName,
                     opt => opt.MapFrom(upload => upload.OriginalName))
                 .ForMember(model => model.FileExtension,
                     opt => opt.MapFrom(upload => upload.FileExtension))
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
                 .ForMember(model => model.ImageUrl,
                     opt => opt.MapFrom(upload => upload.FileUrl))
                 .ForMember(model => model.IsAnnotated,
                     opt => opt.MapFrom(upload => upload.IsAnnotated));
        }
    }
}
