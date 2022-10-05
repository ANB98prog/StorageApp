using FluentValidation;

namespace Storage.Application.Images.Commands.UploadManyImagesArchive
{
    public class UploadManyImagesArchiveCommandValidator : AbstractValidator<UploadManyImagesArchiveCommand>
    {
        public UploadManyImagesArchiveCommandValidator()
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
