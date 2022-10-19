using FluentValidation;
using System;
using System.IO;
using System.Linq;

namespace Storage.Application.Files.Commands.UploadAnnotatedFiles
{
    public class UploadAnnotatedFilesCommandValidator : AbstractValidator<UploadAnnotatedFilesCommand>
    {
        public UploadAnnotatedFilesCommandValidator()
        {
            RuleFor(uploadManyCommand =>
            uploadManyCommand.Files).NotNull().NotEmpty();
            RuleForEach(uploadManyCommand =>
            uploadManyCommand.Files).NotNull().NotEmpty();
            RuleForEach(uploadManyCommand =>
            uploadManyCommand.Files).Must(file =>
            {
                return new string[] { ".zip", ".rar" }.Contains(Path.GetExtension(file.FileName));

            }).WithMessage("Тип загружаемого файла должен быть 'Архив (rar, zip)'");
        }
    }
}
