using System.IO;

namespace Storage.Application.Common.Models
{
    public class FileModel
    {
        public string FileName { get; set; }

        public string[] Attributes { get; set; }

        public Stream FileStream { get; set; }
    }
}
