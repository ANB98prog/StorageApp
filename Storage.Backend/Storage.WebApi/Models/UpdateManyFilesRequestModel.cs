using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Files.Commands.UpdateManyFiles;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Update many files request
    /// </summary>
    public class UpdateManyFilesRequestModel : IMapWith<FileUpdateData>
    {
        /// <summary>
        /// File id
        /// </summary>
        [JsonProperty("fileId")]
        public Guid FileId { get; set; }

        /// <summary>
        /// Attributes
        /// </summary>
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateManyFilesRequestModel, FileUpdateData>()
                .ForMember(update => update.FileId,
                    opt => opt.MapFrom(f => f.FileId))
                .ForMember(update => update.Attributes,
                    opt => opt.MapFrom(f => f.Attributes));
        }
    }
}
