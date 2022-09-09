using System.IO;

namespace Storage.Application.Common.Models
{
    public class UploadFileModel
    {
        public string FileName { get; set; }

        public string[] Attributes { get; set; }

        public Stream FileStream { get; set; }
    }
}
