using FluentValidation;

namespace Storage.Application.Images.Commands.UploadManyImages
{
    public class UploadManyImagesCommandValidation
        : AbstractValidator<UploadManyImagesCommand>
    {
        public UploadManyImagesCommandValidation()
        {
            RuleFor(uploadManyCommand =>
            uploadManyCommand.ImagesFiles).NotNull().NotEmpty();
        }
    }
}
