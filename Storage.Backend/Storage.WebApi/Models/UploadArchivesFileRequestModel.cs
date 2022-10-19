using AutoMapper;
using Mapper;
using Storage.Application.Files.Commands.UploadManyFilesArchive;
using Storage.WebApi.Models;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Upload archives files request
    /// </summary>
    public class UploadArchivesFileRequestModel : UploadManyFilesRequestModel, IMapWith<UploadManyFilesArchiveCommand>
    {
        /// <summary>
        /// Mime type
        /// </summary>
        public List<string> MimeTypes { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadArchivesFileRequestModel, UploadManyFilesArchiveCommand>()
                .ForMember(model => model.Files,
                    opt => opt.MapFrom(upload => upload.Files))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));
        }
    }
}
