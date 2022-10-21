using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using Storage.Application.Common.Exceptions;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Storage.Application.Common.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Loads file
        /// </summary>
        /// <param name="filePath">File path to load</param>
        /// <returns>File stream</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<FileStream> LoadFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
            {
                throw new ArgumentException("Path does not contain file name!", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                var fileName = Path.GetFileName(filePath);
                throw new FileNotFoundException($"Could not find file: '{filePath}'", fileName);
            }

            /*
             Если текстовый файл имеет кодировку не utf-8 (например он на русском)
             */
            var textFilesExtensions = new string[] { ".txt", ".json" };
            if (textFilesExtensions.Contains(Path.GetExtension(filePath)))
            {
                try
                {
                    var bytes = File.ReadAllBytes(filePath);
                    bytes = Encoding.Convert(Encoding.GetEncoding(1251), Encoding.UTF8, bytes);

                    await File.WriteAllBytesAsync(filePath, bytes);
                }
                catch (Exception)
                {
                }
            }

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);

        }

        /// <summary>
        /// Saves file
        /// </summary>
        /// <param name="stream">File stream</param>
        /// <param name="filePath">File path to save</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task SaveFileAsync(Stream stream, string filePath, CancellationToken cancellationToken)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "File stream cannot be empty!");
            }

            var saveStream = stream;

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
                throw new ArgumentException("Path does not contains file name!", nameof(filePath));

            var directory = Directory.GetParent(filePath);

            if (directory != null
                && !Directory.Exists(directory.FullName))
                Directory.CreateDirectory(directory.FullName);

            using (var file = File.OpenWrite(filePath))
            {
                await saveStream.CopyToAsync(file, cancellationToken);
            }

            await saveStream.DisposeAsync();
        }

        /// <summary>
        /// Saves file
        /// </summary>
        /// <param name="data">File data</param>
        /// <param name="filePath">File path to save</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task SaveFileAsync(string data, string filePath, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data), "File data cannot be empty!");
            }

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
                throw new ArgumentException("Path does not contains file name!", nameof(filePath));

            var directory = Directory.GetParent(filePath);

            if (directory != null
                && !Directory.Exists(directory.FullName))
                Directory.CreateDirectory(directory.FullName);

            await File.WriteAllTextAsync(filePath, data, cancellationToken);
        }

        /// <summary>
        /// Copies several files to another directory
        /// </summary>
        /// <param name="sourceFiles">Files to move</param>
        /// <param name="destinationPath">Destination path</param>
        public static async Task CopyFilesToAsync(List<string> sourceFiles, string destinationPath)
        {
            foreach (var source in sourceFiles)
            {
                await CopyFileToAsync(source, destinationPath);
            }
        }

        /// <summary>
        /// Copies file to another directory
        /// </summary>
        /// <param name="sourceFile">File to move</param>
        /// <param name="destinationPath">Destination path</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryCreationException"></exception>
        public static async Task CopyFileToAsync(string sourceFile, string destinationPath)
        {

            if (string.IsNullOrWhiteSpace(sourceFile))
                throw new ArgumentNullException(nameof(sourceFile));
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentNullException(nameof(destinationPath));

            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find file.", Path.GetFileName(sourceFile));

            if (!Directory.Exists(destinationPath))
            {
                try
                {
                    Directory.CreateDirectory(destinationPath);
                }
                catch (Exception ex)
                {
                    throw new DirectoryCreationException($"Couldn't create '{destinationPath}' directory!", ex);
                }
            }

            using (var dest = File.OpenWrite(Path.Combine(destinationPath, Path.GetFileName(sourceFile))))
            using (var source = File.OpenRead(sourceFile))
            {
                await source.CopyToAsync(dest);
            }
        }

        /// <summary>
        /// Moves several files to another directory
        /// </summary>
        /// <param name="sourceFiles">Files to move</param>
        /// <param name="destinationPath">Destination path</param>
        public static async Task MoveFilesToAsync(List<string> sourceFiles, string destinationPath)
        {
            foreach (var source in sourceFiles)
            {
                await MoveFileToAsync(source, destinationPath);
            }
        }

        /// <summary>
        /// Moves file to another directory
        /// </summary>
        /// <param name="sourceFile">File to move</param>
        /// <param name="destinationPath">Destination path</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryCreationException"></exception>
        public static async Task MoveFileToAsync(string sourceFile, string destinationPath)
        {

            if (string.IsNullOrWhiteSpace(sourceFile))
                throw new ArgumentNullException(nameof(sourceFile));
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentNullException(nameof(destinationPath));

            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Cannot find file.", sourceFile);

            if (!Directory.Exists(destinationPath))
            {
                try
                {
                    Directory.CreateDirectory(destinationPath);
                }
                catch (Exception ex)
                {
                    throw new DirectoryCreationException($"Couldn't create '{destinationPath}' directory!", ex);
                }
            }

            using (var dest = File.OpenWrite(Path.Combine(destinationPath, Path.GetFileName(sourceFile))))
            using (var source = File.OpenRead(sourceFile))
            {
                await source.CopyToAsync(dest);
            }

            File.Delete(sourceFile);
        }

        /// <summary>
        /// Removes file
        /// </summary>
        /// <param name="filePath">File to remove</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Removes directory
        /// </summary>
        /// <param name="dirPath">Directory to remove</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveDirectory(string dirPath)
        {
            if (string.IsNullOrWhiteSpace(dirPath))
                throw new ArgumentNullException(nameof(dirPath));

            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
        }

        /// <summary>
        /// Archives files
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <param name="archivePath">Archive file path</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ArchiveFolder(string sourcePath, string archivePath = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath))
                    throw new ArgumentNullException(nameof(sourcePath));
                if (string.IsNullOrWhiteSpace(archivePath))
                    archivePath = Directory.GetParent(sourcePath)?.FullName;

                var archiveName = Path.GetFileNameWithoutExtension(sourcePath);
                archivePath = Path.Combine(Path.GetFullPath(archivePath), $"{archiveName}.zip");
                ZipFile.CreateFromDirectory(sourcePath, archivePath);
                
                return archivePath;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while archive files", ex);
            }
        }

        /// <summary>
        /// Archives files
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <param name="archivePath">Archive file path</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static FileStream ArchiveFolderStream(string sourcePath, string archivePath = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath))
                    throw new ArgumentNullException(nameof(sourcePath));
                if (string.IsNullOrWhiteSpace(archivePath))
                    archivePath = Directory.GetParent(sourcePath)?.FullName;

                var archiveName = Path.GetFileNameWithoutExtension(sourcePath);
                archivePath = Path.Combine(Path.GetFullPath(archivePath), $"{archiveName}.zip");
                ZipFile.CreateFromDirectory(sourcePath, archivePath);

                return new FileStream(archivePath, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while archive files", ex);
            }
        }

        /// <summary>
        /// Unzipping file
        /// </summary>
        /// <param name="archivePath">Archive file</param>
        /// <param name="destinationPath">Unzipped files path</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NotSupportedArchiveTypeException"></exception>
        /// <exception cref="Exception"></exception>
        public static string UnzipFolder(string archivePath, string destinationPath = null)
        {
            try
            {
                return ArchiveHelper.UnzipFile(archivePath, destinationPath);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (NotSupportedArchiveTypeException ex)
            {
                throw ex;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while unzip file", ex);
            }
        }

        /// <summary>
        /// Gets mime type by file name
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Mime type</returns>
        public static string GetFileMimeType(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName)
                && MimeTypes.TryGetMimeType(fileName, out var mimeType))
            {
                return mimeType;
            }

            return Constants.DEFAULT_MIME_TYPE;
        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetFileAttributesByMimeData(string mimeType)
        {
            if (!string.IsNullOrWhiteSpace(mimeType))
            {
                return mimeType.Split("/");
            }

            return new string[] {};
        }

        //public static FileType GetFileType(string filePath)
        //{
        //    var types = new Dictionary<FileType, List<string>>
        //    {
        //        { FileType.Text, new List<string> { ".txt", ".docx"} },
        //        { FileType.Image, new List<string> { ".jpg", ".png", ".jpeg", ".bmp", ".tif", ".tiff", ".gif" } },
        //        { FileType.Video, new List<string> { ".mp4", ".avi", ".mpg", ".mpeg", ".wmv" } },
        //        { FileType.Audio, new List<string> { ".mp3", ".wav", ".wma", ".mid", ".midi", ".aiff", ".au" } },
        //        { FileType.Zip, new List<string> { ".zip", ".rar" } },
        //    };

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(filePath))
        //        {
        //            throw new ArgumentNullException(nameof(filePath));
        //        }

        //        return types.FirstOrDefault(t =>
        //                t.Value.Contains(Path.GetExtension(filePath).ToLowerInvariant()))
        //                    .Key;
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Unexpected error occured while get file type", ex);
        //    }
        //}

    }
}
