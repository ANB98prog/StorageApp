using Storage.Application.Common.Services;
using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class Factory
    {
        public static string StorageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp");

        public static IFileService CreateLocalFileStorageService()
        {
            if (!Directory.Exists(StorageDirectory))
            {
                Directory.CreateDirectory(StorageDirectory);
            }

            return new LocalFileStorageService(StorageDirectory);
        }
    }
}
