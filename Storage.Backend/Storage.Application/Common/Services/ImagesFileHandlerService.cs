using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Storage.Application.Common.Services
{
    public class ImagesFileHandlerService : IFileHandlerService
    {
        private readonly IFileService _fileService;

        public ImagesFileHandlerService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DownloadManyFilesAsync(List<string> filesPath, CancellationToken cancellationToken)
        {
            //Подготавливаем файлы
                // Создаем временную папку
                // Архивируем ее

            var tempDir = Path.GetRandomFileName();

            var zipFile = FileHelper.MoveFilesToAsync(filesPath, tempDir);
        }

        public Task<Guid> UploadFileAsync<T>(T file, CancellationToken cancellationToken) where T : BaseFile
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Task> UploadLargeFileAsync<T>(T file, CancellationToken cancellationToken) where T : BaseFile
        {
            throw new NotImplementedException();
        }
    }
}
