using FluentValidation;
using System;

namespace Storage.Application.Files.Commands.DeleteFile
{
    public class DeleteCommandValidator
        : AbstractValidator<DeleteFileCommand>
    {
        public DeleteCommandValidator()
        {
            RuleFor(delete =>
                delete.FileId).NotEqual(Guid.Empty);
        }
    }
}
