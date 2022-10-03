using MediatR;
using Storage.Application.Images.Queries.Models;
using System;

namespace Storage.Application.Images.Queries.GetImage
{
    public class GetImageByIdQuery : IRequest<ImageVm>
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
    }
}
