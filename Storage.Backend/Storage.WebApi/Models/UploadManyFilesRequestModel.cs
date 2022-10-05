using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Images.Commands.UploadImage;
using Storage.Application.Images.Commands.UploadManyImages;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Upload many files request
    /// </summary>
    public class UploadManyFilesRequestModel : IMapWith<UploadImageCommand>
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
        /// Files
        /// </summary>
        [JsonProperty("file")]
        public IList<IFormFile> Files { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadManyFilesRequestModel, UploadManyImagesCommand>()
                .ForMember(model => model.ImagesFiles,
                    opt => opt.MapFrom(upload => upload.Files))
                .ForMember(model => model.IsAnnotated,
                    opt => opt.MapFrom(upload => upload.isAnnotated))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));
        }
    }
}
