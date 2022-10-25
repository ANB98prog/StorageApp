using AutoMapper;
using Elasticsearch.Models;
using Mapper;
using MediatR;
using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.UpdateFile
{
    /// <summary>
    /// Update file command model
    /// </summary>
    public class UpdateFileCommand
        : IRequest<UpdatedVm>, IMapWith<UpdateFileAttributesModel>
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

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
            profile.CreateMap<UpdateFileCommand, UpdateFileAttributesModel>()
                .ForMember(m => m.Id,
                    opt => opt.MapFrom(f => f.FileId))
                .ForMember(m => m.Attributes,
                    opt => opt.MapFrom(f => f.Attributes));
        }
    }
}
