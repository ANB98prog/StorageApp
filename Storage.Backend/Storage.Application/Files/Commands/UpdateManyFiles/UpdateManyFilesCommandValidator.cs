using FluentValidation;
using System;

namespace Storage.Application.Files.Commands.UpdateManyFiles
{
    /// <summary>
    /// Update many files command validator
    /// </summary>
    public class UpdateManyFilesCommandValidator
        : AbstractValidator<UpdateManyFilesCommand>
    {
        public UpdateManyFilesCommandValidator()
        {
            //RuleFor(many => many.UserId)
            //    .NotEqual(Guid.Empty);
            RuleFor(many => many.Updates)
                            .NotNull()
                                .NotEmpty();
            RuleForEach<FileUpdateData>(many => many.Updates)
                .SetValidator(new FileUpdateDataValidator());
        }
    }

    public class FileUpdateDataValidator
        : AbstractValidator<FileUpdateData>
    {
        public FileUpdateDataValidator()
        {
            RuleFor(update => update.FileId)
                .NotEqual(Guid.Empty);
            RuleFor(update => update.Attributes)
                .NotNull()
                    .NotEmpty();
        }
    }
}
