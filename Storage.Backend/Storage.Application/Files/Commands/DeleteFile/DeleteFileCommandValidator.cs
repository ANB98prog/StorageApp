using FluentValidation;
using System;

namespace Storage.Application.Files.Commands.DeleteFile
{
    public class DeleteFileCommandValidator
        : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(delete =>
                delete.FileId).NotEqual(Guid.Empty);
        }
    }
}
