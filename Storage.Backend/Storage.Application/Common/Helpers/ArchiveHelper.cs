using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using Storage.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Storage.Application.Common.Helpers
{
    public static class ArchiveHelper
    {
        public static string UnzipFile(string archivePath, string destinationPath)
        {
            var archMethods = new Dictionary<string, Func<string, string, string>>();

            archMethods.Add(".rar", (string archPath, string destPath) =>
            {
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);
                                
                using (var archive = RarArchive.Open(archPath))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        entry.WriteToDirectory(destinationPath, new SharpCompress.Common.ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }
                }

                return destPath;
            });
            archMethods.Add(".zip", (string archPath, string destPath) =>
            {
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                using (var archive = ZipArchive.Open(archPath))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        entry.WriteToDirectory(destinationPath, new SharpCompress.Common.ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }
                }

                return destPath;
            });

            if (string.IsNullOrWhiteSpace(archivePath))
                throw new ArgumentNullException(nameof(archivePath));
            if (!File.Exists(archivePath))
                throw new FileNotFoundException("Couldn't found archive file", archivePath);
            if (string.IsNullOrWhiteSpace(destinationPath))
                destinationPath = Path.Combine(Directory.GetParent(archivePath)?.FullName, Path.GetFileNameWithoutExtension(archivePath));

            var archiveType = Path.GetExtension(archivePath);

            if(!archMethods.ContainsKey(archiveType))
                throw new NotSupportedArchiveTypeException(archiveType);

            return archMethods[archiveType](archivePath, destinationPath);
        }
    }
}
