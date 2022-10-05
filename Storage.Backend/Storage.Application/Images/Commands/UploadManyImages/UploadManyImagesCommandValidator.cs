using FluentValidation;

namespace Storage.Application.Images.Commands.UploadManyImages
{
    public class UploadManyImagesCommandValidator : AbstractValidator<UploadManyImagesCommand>
    {
        public UploadManyImagesCommandValidator()
        {
            RuleFor(uploadManyCommand =>
            uploadManyCommand.ImagesZipFile).NotNull().NotEmpty();
            RuleFor(uploadManyCommand =>
            uploadManyCommand.ImagesZipFile.FileName).NotNull().NotEmpty();
            RuleFor(uploadManyCommand =>
            uploadManyCommand.ImagesZipFile.Length).GreaterThan(0);
            RuleFor(uploadManyCommand =>
            uploadManyCommand.FileType).Equal(Domain.FileType.Zip.ToString());
        }
    }
}
