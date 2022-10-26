using FluentValidation;

namespace Storage.Application.Files.Commands.UpdateGroupFiles
{
    public class UpdateGroupFilesCommandValidator
        : AbstractValidator<UpdateGroupFilesCommand>
    {
        public UpdateGroupFilesCommandValidator()
        {
            //RuleFor(update => update.UserId)
            //    .NotEqual(Guid.Empty);
            RuleFor(update => update.FilesIds)
                    .NotEmpty();
            RuleFor(update => update.Attributes)
                    .NotEmpty();
        }
    }
}
