using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Images.Commands.UploadImage
{
    public class UploadImageCommandHandler
        : IRequestHandler<UploadImageCommand, Guid>
    {
        public Task<Guid> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
