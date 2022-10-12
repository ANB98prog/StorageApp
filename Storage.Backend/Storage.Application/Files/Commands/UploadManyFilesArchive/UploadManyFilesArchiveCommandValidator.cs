using FluentValidation;
using System.IO;
using System.Linq;

namespace Storage.Application.Files.Commands.UploadManyFilesArchive
{
    public class UploadManyFilesArchiveCommandValidator : AbstractValidator<UploadManyFilesArchiveCommand>
    {
        public UploadManyFilesArchiveCommandValidator()
        {
            RuleFor(uploadManyCommand =>
            uploadManyCommand.File).NotNull().NotEmpty();
            RuleFor(uploadManyCommand =>
            uploadManyCommand.File.FileName).NotNull().NotEmpty();
            RuleFor(uploadManyCommand =>
            uploadManyCommand.File.Length).GreaterThan(0);
            RuleFor(uploadManyCommand =>
            uploadManyCommand.File.FileName).Must(file =>
            {
                return new string[] { ".zip", ".rar" }.Contains(Path.GetExtension(file));

            }).WithMessage("Тип загружаемого файла должен быть 'Архив (rar, zip)'");
        }
    }
}
