using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Images.Commands.UploadImage;
using Storage.Application.Images.Commands.UploadManyImages;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Upload file request
    /// </summary>
    public class UploadFileRequestModel : IMapWith<UploadImageCommand>
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
            profile.CreateMap<UploadFileRequestModel, UploadImageCommand>()
                .ForMember(model => model.ImageFile,
                    opt => opt.MapFrom(upload => upload.File))
                .ForMember(model => model.IsAnnotated,
                    opt => opt.MapFrom(upload => upload.isAnnotated))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));

            profile.CreateMap<UploadFileRequestModel, UploadManyImagesArchiveCommand>()
                .ForMember(model => model.ImagesZipFile,
                    opt => opt.MapFrom(upload => upload.File))
                .ForMember(model => model.IsAnnotated,
                    opt => opt.MapFrom(upload => upload.isAnnotated))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));
        }
    }
}
