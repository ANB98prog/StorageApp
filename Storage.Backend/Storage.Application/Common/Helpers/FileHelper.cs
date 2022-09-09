using Storage.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
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
            if (stream == null
                || stream.Length == 0)
            {
                throw new ArgumentNullException(nameof(stream), "File stream cannot be empty!");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            using (var file = File.OpenWrite(filePath))
            {
                await stream.CopyToAsync(file, cancellationToken);
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

            if (string.IsNullOrEmpty(sourceFile))
                throw new ArgumentNullException(nameof(sourceFile));
            if (string.IsNullOrEmpty(destinationPath))
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
        }

        /// <summary>
        /// Archives files
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <param name="archivePath">Archive file path</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ArchiveFolder(string sourcePath, string archivePath)
        {
            try
            {
                var archiveName = Path.GetFileNameWithoutExtension(sourcePath);
                archivePath = Path.Combine(Path.GetFullPath(archivePath), $"{archiveName}.zip");
                ZipFile.CreateFromDirectory(sourcePath, archivePath);

                return archivePath;
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
        /// <exception cref="Exception"></exception>
        public static void UnzipFolder(string archivePath, string destinationPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(archivePath, destinationPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured while unzip file", ex);
            }
        }
    }
}
