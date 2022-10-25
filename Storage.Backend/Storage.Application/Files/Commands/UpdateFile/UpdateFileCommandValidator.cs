using FluentValidation;
using System;

namespace Storage.Application.Files.Commands.UpdateFile
{
    /// <summary>
    /// Update file command validator
    /// </summary>
    public class UpdateFileCommandValidator
        : AbstractValidator<UpdateFileCommand>
    {
        public UpdateFileCommandValidator()
        {
            //RuleFor(update =>
            //    update.UserId).NotEqual(Guid.Empty);
            RuleFor(update =>
                update.FileId).NotEqual(Guid.Empty);
            RuleFor(update =>
                update.Attributes).NotNull().NotEmpty();
        }
    }
}
