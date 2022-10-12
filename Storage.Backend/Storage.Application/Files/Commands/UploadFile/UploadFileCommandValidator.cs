using FluentValidation;

namespace Storage.Application.Files.Commands.UploadFile
{
    /// <summary>
    /// Upload file command validator
    /// </summary>
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            //RuleFor(uploadImageCommand =>
            //    uploadImageCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(uploadImageCommand =>
                uploadImageCommand.File).NotNull().NotEmpty();
            RuleFor(uploadImageCommand =>
                uploadImageCommand.File.FileName).NotNull();
            RuleFor(uploadImageCommand =>
                uploadImageCommand.File.Length).GreaterThan(0);
        }
    }
}
