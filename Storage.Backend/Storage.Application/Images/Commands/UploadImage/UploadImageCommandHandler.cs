using MediatR;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Services;
using Storage.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Images.Commands.UploadImage
{
    public class UploadImageCommandHandler
        : IRequestHandler<UploadImageCommand, Guid>
    {
        public async Task<Guid> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var image = new Image
            {
                Id = Guid.NewGuid(),
                OwnerId = request.UserId,
                Attributes = request.Attributes,
                FileExtension = Path.GetExtension(request.ImageFile.FileName),
                OriginalName = request.ImageFile.FileName,
                ContentType = request.ImageFile.ContentType,
                //DepartmentOwnerId
                
            };

            // Добавить связь с организацией

            // Загрузить файл в хранилище
            var filePath = Path.Combine("E:\\Programs\\nginx\\html\\images", $"{image.Id.ToString().Substring(0, 11)}{image.FileExtension}");

            await FileHelper.SaveFileAsync(request.ImageFile.OpenReadStream(), filePath, cancellationToken);

            return image.Id;
        }
    }
}
