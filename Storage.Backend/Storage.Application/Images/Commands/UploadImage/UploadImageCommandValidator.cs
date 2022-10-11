﻿using FluentValidation;
using Storage.Application.Common.Helpers;

namespace Storage.Application.Images.Commands.UploadImage
{
    /// <summary>
    /// Upload image command validator
    /// </summary>
    public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
    {
        public UploadImageCommandValidator()
        {
            //RuleFor(uploadImageCommand =>
            //    uploadImageCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(uploadImageCommand =>
                uploadImageCommand.ImageFile).NotNull().NotEmpty();
            RuleFor(uploadImageCommand =>
                uploadImageCommand.ImageFile.FileName).NotNull();
            RuleFor(uploadImageCommand =>
                uploadImageCommand.ImageFile.Length).GreaterThan(0);
            RuleFor(uploadImageCommand =>
                uploadImageCommand.FileType).Equal(Domain.FileType.Image.ToString()).WithMessage("Тип загружаемого файла должен быть 'Картинка'");
        }
    }
}
