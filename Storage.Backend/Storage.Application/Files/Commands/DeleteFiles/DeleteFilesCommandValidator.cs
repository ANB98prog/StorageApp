using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.DeleteFiles
{
    public class DeleteFilesCommandValidator : AbstractValidator<DeleteFilesCommand>
    {
        public DeleteFilesCommandValidator()
        {
            RuleFor(delete => delete.FilesIds)
                .NotNull().NotEmpty();

        }
    }
}
