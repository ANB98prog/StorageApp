using MediatR;
using Microsoft.AspNetCore.Http;
using Storage.Application.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Storage.Application.Images.Commands.UploadImage
{
    public class UploadImageCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public List<string> Attributes { get; set; } = new List<string>();

        public bool IsAnnotated { get; set; } = false;

        public IFormFile ImageFile { get; set; }

        public string FileType { 
            get
            {
                if(ImageFile != null
                    && !string.IsNullOrEmpty(ImageFile.FileName))
                {
                    return FileHelper.GetFileType(ImageFile.FileName).ToString();
                }

                return "unknown";
            }
        }
    }
}
