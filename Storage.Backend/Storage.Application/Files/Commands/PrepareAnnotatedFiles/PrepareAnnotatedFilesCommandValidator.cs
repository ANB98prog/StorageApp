using FluentValidation;

namespace Storage.Application.Files.Commands.PrepareAnnotatedFiles
{
    public class PrepareAnnotatedFilesCommandValidator
        : AbstractValidator<PrepareAnnotatedFilesCommand>
    {
        public PrepareAnnotatedFilesCommandValidator()
        {
            //RuleFor(prepare =>
            //    prepare.UserId).NotNull().NotEqual(Guid.Empty);
            RuleFor(prepare =>
                prepare.AnnotatedFilesIds).NotNull();
        }
    }
}
