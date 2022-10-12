using FluentValidation;

namespace Storage.Application.Files.Commands.UploadManyFiles
{
    public class UploadManyFilesCommandValidation
        : AbstractValidator<UploadManyFilesCommand>
    {
        public UploadManyFilesCommandValidation()
        {
            RuleFor(uploadManyCommand =>
            uploadManyCommand.Files).NotNull().NotEmpty();
        }
    }
}
