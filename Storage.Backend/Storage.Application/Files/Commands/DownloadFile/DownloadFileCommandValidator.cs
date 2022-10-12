using FluentValidation;
using System;

namespace Storage.Application.Files.Commands.DownloadFile
{
    public class DownloadFileCommandValidator 
        : AbstractValidator<DownloadFileCommand>
    {
        public DownloadFileCommandValidator()
        {
            RuleFor(download =>
                download.FileId).NotNull().NotEqual(Guid.Empty);
        }
    }
}
