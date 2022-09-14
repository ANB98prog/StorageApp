using AutoMapper;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileAttributes = Storage.Application.Common.Models.FileAttributes;

namespace Storage.Application.Common.Services
{
    public class ImagesFileHandlerService : IFileHandlerService
    {
        /// <summary>
        /// Contract mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Files service
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// Directory for temporary files
        /// </summary>
        private readonly string TEMP_DIR = Path.Combine(Environment.CurrentDirectory, "temp");

        /// <summary>
        /// Initializes class instance of <see cref="ImagesFileHandlerService"/>
        /// </summary>
        /// <param name="mapper">Contract mapper</param>
        /// <param name="fileService">Files service</param>
        public ImagesFileHandlerService(IMapper mapper, IFileService fileService)
        {
            _mapper = mapper;
            _fileService = fileService;

            if(!Directory.Exists(TEMP_DIR))
                Directory.CreateDirectory(TEMP_DIR);
        }

        public async Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            return await _fileService.DownloadFileAsync(filePath, cancellationToken);
        }

        public async Task<FileStream> DownloadManyFilesAsync(List<string> filesPath, CancellationToken cancellationToken)
        {
            if (filesPath != null 
                && filesPath.Count == 0)
            {
                throw new ArgumentNullException(nameof(filesPath));
            }
                        
            return await _fileService.DownloadManyFilesAsync(filesPath, cancellationToken);
        }

        public async Task<List<Guid>> UploadArchiveFileAsync(UploadFileRequestModel file, CancellationToken cancellationToken)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var files = await LoadArchiveFilesAsync(file, cancellationToken);

            /*Save in storage*/
            var savedFilesPath = await _fileService.UploadManyFilesAsync(_mapper.Map<List<UploadFileRequestModel>, List<FileModel>>(files), cancellationToken);

            /*Need to append saved files path*/
            if (savedFilesPath != null)
            {
                foreach (var savedFile in savedFilesPath)
                {
                    var match = files.FirstOrDefault(f => f.SystemName.Equals(Path.GetFileName(savedFile)));

                    if(match != null)
                    {
                        match.OriginalFilePath = savedFile;
                    }
                }
            }

            /*Indexing*/
            /*
             TO DO
             */

            return files.Select(s => s.Id).ToList();

        }

        /// <summary>
        /// Loads archive files
        /// </summary>
        /// <param name="archive">Archive to load</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Loaded files</returns>
        private async Task<List<UploadFileRequestModel>> LoadArchiveFilesAsync(UploadFileRequestModel archive, CancellationToken cancellationToken)
        {
            var filesData = new List<UploadFileRequestModel>();

            var tempPath = Path.Combine(TEMP_DIR, archive.SystemName);
            
            /*Save archive in local storage*/
            await FileHelper.SaveFileAsync(archive.Stream, tempPath, cancellationToken);

            /*Unzip*/
            var unzipPath = FileHelper.UnzipFolder(tempPath);

            var files = Directory.GetFiles(unzipPath);

            /*Need to add attributes to each file*/
            if (files.Any())
            {
                foreach (var file in files)
                {
                    var stream =  await FileHelper.LoadFileAsync(file);

                    var fileId = Guid.NewGuid();
                    var systemName = $"{fileId.Trunc()}{Path.GetExtension(file)}";
                    filesData.Add(new UploadFileRequestModel
                    {
                        Id = fileId,
                        SystemName = systemName,
                        OriginalName = Path.GetFileName(file),
                        Attributes = archive.Attributes,
                        FileExtension = Path.GetExtension(file),
                        ContentType = archive.ContentType,
                        IsAnnotated = archive.IsAnnotated,
                        DepartmentOwnerId = archive.DepartmentOwnerId,
                        OwnerId = archive.OwnerId,
                        Stream = stream
                    });
                }
            }

            /*Need to remove archive*/
            FileHelper.RemoveFile(tempPath);

            return filesData;
        }

        /// <summary>
        /// Uploads file to server
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded file id</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Guid> UploadFileAsync(UploadFileRequestModel file, CancellationToken cancellationToken)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var result = await UploadFileToStorageAsync(file, cancellationToken);

            return result;
        }

        public async Task<List<Guid>> UploadManyFileAsync(List<UploadFileRequestModel> files, CancellationToken cancellationToken)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }

            var ids = new List<Guid>();
            foreach (var file in files)
            {
                ids.Add(await UploadFileToStorageAsync(file, cancellationToken));
            }

            return ids;
        }

        private async Task<Guid> UploadFileToStorageAsync(UploadFileRequestModel upload, CancellationToken cancellationToken)
        {
            var file = new FileModel
            {
                FileName = upload.SystemName,
                Attributes = upload.IsAnnotated ?
                    new string[] { FileAttributes.Annotated.ToString() }
                        : new string[] { FileAttributes.NotAnnotated.ToString() },
                FileStream = upload.Stream
            };

            // Save file to storage
            var savedFilePath = await _fileService.UploadFileAsync(file, cancellationToken);

            upload.OriginalFilePath = savedFilePath;

            // Index file in db

            return upload.Id;
            
        }
    }
}
