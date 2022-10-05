using MediatR;
using Microsoft.AspNetCore.Http;
using Storage.Application.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Storage.Application.Images.Commands.UploadManyImagesArchive
{
    public class UploadManyImagesArchiveCommand : IRequest<List<Guid>>
    {
        public Guid UserId { get; set; }

        public List<string> Attributes { get; set; } = new List<string>();

        public bool IsAnnotated { get; set; } = false;

        public IFormFile ImagesZipFile { get; set; }

        public string FileType
        {
            get
            {
                if (ImagesZipFile != null
                    && !string.IsNullOrEmpty(ImagesZipFile.FileName))
                {
                    return FileHelper.GetFileType(ImagesZipFile.FileName).ToString();
                }

                return "unknown";
            }
        }
    }
}
