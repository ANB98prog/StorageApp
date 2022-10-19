using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Files.Commands.UploadFile;
using Storage.Application.Files.Commands.UploadManyFiles;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Upload many files request
    /// </summary>
    public class UploadManyFilesRequestModel : BaseUploadFileModel, IMapWith<UploadFileCommand>
    {
        /// <summary>
        /// Files
        /// </summary>
        [JsonProperty("file")]
        public IList<IFormFile> Files { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadManyFilesRequestModel, UploadManyFilesCommand>()
                .ForMember(model => model.Files,
                    opt => opt.MapFrom(upload => upload.Files))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes));
        }
    }
}
