using AutoMapper;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Files.Commands.UploadAnnotatedFiles;
using Storage.Domain;

namespace Storage.WebApi.Models
{
    /// <summary>
    /// Upload annotated data request
    /// </summary>
    public class UploadAnnotatedDataRequestModel : UploadManyFilesRequestModel, IMapWith<UploadAnnotatedFilesCommand>
    {
        /// <summary>
        /// Annotation format
        /// </summary>
        [JsonProperty("annotationFormat")]
        public AnnotationFormats AnnotationFormat { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadAnnotatedDataRequestModel, UploadAnnotatedFilesCommand>()
                .ForMember(model => model.Files,
                    opt => opt.MapFrom(upload => upload.Files))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => upload.Attributes))
                .ForMember(model => model.AnnotationFormat,
                    opt => opt.MapFrom(upload => upload.AnnotationFormat));
        }
    }
}
