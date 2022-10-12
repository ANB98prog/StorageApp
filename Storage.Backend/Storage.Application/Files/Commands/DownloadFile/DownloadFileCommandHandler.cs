using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.DownloadFile
{
    public class DownloadFileCommandHandler
        : IRequestHandler<DownloadFileCommand, FileStream>
    {
        public Task<FileStream> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
