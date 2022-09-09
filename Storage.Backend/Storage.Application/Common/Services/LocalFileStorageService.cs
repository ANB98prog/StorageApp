using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Storage.Application.Common.Services
{
    /// <summary>
    /// Local file storage service
    /// </summary>
    public class LocalFileStorageService : IFileService
    {
        /// <summary>
        /// Local storage directory path
        /// </summary>
        private readonly string _localStorageDir;

        public LocalFileStorageService(string localStorageDir)
        {
            _localStorageDir = localStorageDir;
        }

        /// <summary>
        /// Downloads file from local storage
        /// </summary>
        /// <param name="filePath">FIle to download</param>
        /// <returns>File stream result</returns>
        /// <exception cref="FileUploadingException"></exception>
        public async Task<FileStream> DownloadFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath));
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Couldn't found file!", filePath);
                }

                FileStream fileStream = null;

                using (var file = File.OpenRead(filePath))
                {
                    await file.CopyToAsync(fileStream);
                }

                return fileStream;
            }
            catch (ArgumentNullException ex)
            {
                throw new FileUploadingException(ex.Message, ex.InnerException);
            }
            catch(FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileUploadingException("Unexpected error occured while file downloading.", ex.InnerException);
            }
        }

        /// <summary>
        /// Upload file to local storage
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns>Uploaded file path</returns>
        /// <exception cref="FileUploadingException"></exception>
        public async Task<string> UploadFileAsync(FileModel file)
        {
            try
            {
                ValidateFile(file);

                var path = Path.Combine(_localStorageDir, string.Join(Path.DirectorySeparatorChar, file.Attributes), file.FileName);

                using (var target = File.OpenWrite(path))
                {
                    await file.FileStream.CopyToAsync(target);
                }

                return path;
            }
            catch (ArgumentNullException ex)
            {
                throw new FileUploadingException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new FileUploadingException("Unexpected error occured while file uploading.", ex.InnerException);
            }
        }

        /// <summary>
        /// Validates file argument
        /// </summary>
        /// <param name="file">Argument</param>
        /// <exception cref="ArgumentNullException"></exception>
        private void ValidateFile(FileModel file)
        {
            if (file == null
                    || file.FileStream?.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
        }
    }
}
