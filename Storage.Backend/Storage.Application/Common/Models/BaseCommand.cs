using MediatR;
using System;

namespace Storage.Application.Common.Models
{
    public class BaseCommand<TResponse> : IRequest<TResponse> where TResponse : class
    {
        /// <summary>
        /// User id that initialized command execution
        /// </summary>
        public Guid UserId { get; set; }
    }
}
