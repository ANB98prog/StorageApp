using System;
using System.IO;
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
        public static async Task<Stream> LoadFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            Stream stream = null;

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await file.CopyToAsync(stream);
            }

            if (stream == null)
            {
                throw new Exception("Could not get file.");
            }

            return stream;
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
    }
}
