using AutoMapper;
using Mapper;
using Storage.Domain;
using System.IO;

namespace Storage.Application.Common.Models
{
    public class UploadFileRequestModel : BaseFile, IMapWith<FileModel>
    {
        /// <summary>
        /// File stream
        /// </summary>
        public Stream Stream { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadFileRequestModel, FileModel>()
                .ForMember(model => model.FileName,
                    opt => opt.MapFrom(upload => upload.SystemName))
                .ForMember(model => model.FileStream,
                    opt => opt.MapFrom(upload => upload.Stream))
                .ForMember(model => model.Attributes,
                    opt => opt.MapFrom(upload => (upload.IsAnnotated) ?
                        new string[] {FileAttributes.Annotated.ToString()} 
                            : new string[] { FileAttributes.NotAnnotated.ToString() }));
        }
    }
}
