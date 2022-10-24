using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Files.Commands.PrepareAnnotatedFiles;
using Storage.Domain;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Prepare annotated files request model
    /// </summary>
    public class PrepareAnnotatedDataRequestModel : IMapWith<PrepareAnnotatedFilesCommand>
    {
        /// <summary>
        /// Annotated files ids
        /// </summary>
        [JsonProperty("filesIds")]
        public List<Guid> FilesIds { get; set; }

        /// <summary>
        /// Annotation format
        /// </summary>
        [JsonProperty("annotationFormat")]
        public AnnotationFormats AnnotationFormat { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrepareAnnotatedDataRequestModel, PrepareAnnotatedFilesCommand>()
                .ForMember(model => model.AnnotatedFilesIds,
                    opt => opt.MapFrom(upload => upload.FilesIds))
                .ForMember(model => model.AnnotationFormat,
                    opt => opt.MapFrom(upload => upload.AnnotationFormat));
        }
    }
}
