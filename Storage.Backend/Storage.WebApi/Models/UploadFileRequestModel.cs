using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Files.Commands.UploadFile;
using Storage.Application.Files.Commands.UploadManyFilesArchive;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Upload file request
    /// </summary>
    public class UploadFileRequestModel : IMapWith<UploadFileCommand>
    {
        /// <summary>
        /// File attributes
        /// </summary>
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; }

        /// <summary>
        /// Is data annotated
        /// </summary>
        [JsonProperty("is_annotated")]
        public bool isAnnotated { get; set; }

        /// <summary>
        /// File data
        /// </summary>
        [JsonProperty("file")]
        public IFormFile File { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadFileRequestModel, UploadFileCommand>()
                .ForMember(model => model.File,
                    opt => opt.MapFrom(upload => upload.File))
                .ForMember(model => model.IsAnnotated,
                    opt => opt.MapFrom(upload => upload.isAnnotated))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));

            profile.CreateMap<UploadFileRequestModel, UploadManyFilesArchiveCommand>()
                .ForMember(model => model.File,
                    opt => opt.MapFrom(upload => upload.File))
                .ForMember(model => model.IsAnnotated,
                    opt => opt.MapFrom(upload => upload.isAnnotated))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));
        }
    }
}
