using AutoMapper;
using Mapper;
using MediatR;
using Storage.Application.Common.Models;
using Storage.Application.Files.Commands.UpdateFile;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.UpdateManyFiles
{
    /// <summary>
    /// Update list of files command
    /// </summary>
    public class UpdateManyFilesCommand
        : IRequest<UpdatedManyVm>
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Updates
        /// </summary>
        public List<FileUpdateData> Updates { get; set; }
    }
    public class FileUpdateData : IMapWith<UpdateFileAttributesModel>
    {
        /// <summary>
        /// File id
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// File's attributes
        /// </summary>
        public List<string> Attributes { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FileUpdateData, UpdateFileAttributesModel>()
                .ForMember(m => m.Id,
                    opt => opt.MapFrom(f => f.FileId))
                .ForMember(m => m.Attributes,
                    opt => opt.MapFrom(f => f.Attributes));
        }

    }
}
