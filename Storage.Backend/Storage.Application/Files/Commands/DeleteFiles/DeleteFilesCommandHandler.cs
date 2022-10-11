using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.DeleteFiles
{
    public class DeleteFilesCommandHandler
        : IRequestHandler<DeleteFilesCommand, DeleteFilesModel>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public DeleteFilesCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<DeleteFilesModel> Handle(DeleteFilesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidateParameters(request);

                var guids = request.FilesIds
                                .Select(id => Guid.Parse(id))
                                    .ToList();

                var removeFilesResult = await _fileHandlerService.RemoveFilesAsync(guids);

                return removeFilesResult;
            }
            catch (ArgumentNullException ex)
            {
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (FileHandlerServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_FILES_REMOVE_MESSAGE);
            }
        }

        private void ValidateParameters(DeleteFilesCommand parameter)
        {
            if (parameter == null
                || parameter.FilesIds == null)
            {
                throw new ValidationException(ErrorMessages.InvalidRequiredParameterErrorMessage("Input parameter are empty!"));
            }

            if (parameter.FilesIds.Any())
            {
                var errMessages = new List<string>();

                foreach (var id in parameter.FilesIds)
                {
                    var isParsed = Guid.TryParse(id, out var result);

                    if (!isParsed)
                    {
                        errMessages.Add(ErrorMessages.WrongParameterFormatErrorMessage("id", id, "1065ea8c-a85b-42e3-90f3-c21af5184b08"));
                    }
                }

                if (errMessages.Any())
                {
                    throw new ValidationException(errMessages);
                }
            }

        }
    }
}
