using AutoMapper;
using Mapper;
using Storage.Domain;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Uploaded file info model
    /// </summary>
    public class UploadedFileModel : IMapWith<FilePath>
    {
        /// <summary>
        /// Fill (absolute) file path
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Relative file path
        /// </summary>
        public string RelativePath { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadedFileModel, FilePath>()
                .ForMember(model => model.FullPath,
                    opt => opt.MapFrom(upload => upload.FullPath))
                .ForMember(model => model.RelativePath,
                    opt => opt.MapFrom(upload => upload.RelativePath));
        }
    }
}
