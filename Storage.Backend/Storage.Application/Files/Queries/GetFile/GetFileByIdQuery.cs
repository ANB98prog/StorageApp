using MediatR;
using Storage.Application.Files.Queries.Models;
using System;

namespace Storage.Application.Files.Queries.GetFile
{
    public class GetFileByIdQuery : IRequest<FileVm>
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
    }
}
