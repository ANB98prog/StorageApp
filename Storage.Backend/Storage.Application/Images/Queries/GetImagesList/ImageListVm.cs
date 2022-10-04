using AutoMapper;
using Elasticsearch.Models;
using Mapper;
using Storage.Application.Images.Queries.Models;
using Storage.Domain;
using System.Collections.Generic;

namespace Storage.Application.Images.Queries.GetImagesList
{
    public class ImageListVm : IMapWith<SearchResponse<BaseFile>>
    {
        /// <summary>
        /// Images list
        /// </summary>
        public IList<ImageVm> Images { get; set; }

        /// <summary>
        /// Images count
        /// </summary>
        public int Count 
        { 
            get
            {
                return Images?.Count ?? 0;
            }
        }

        const int maxPageSize = 100;

        /// <summary>
        /// Page number
        /// </summary>
        public int? PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SearchResponse<BaseFile>, ImageListVm>()
                .ForMember(model => model.Images,
                opt => opt.MapFrom(p => p.Documents));

            profile.CreateMap<HitModel<BaseFile>, ImageVm>()
                .ForMember(model => model.Id,
                opt => opt.MapFrom(p => p.Document.Id))
                .ForMember(model => model.OwnerId,
                opt => opt.MapFrom(p => p.Document.OwnerId))
                .ForMember(model => model.DepartmentOwnerId,
                opt => opt.MapFrom(p => p.Document.DepartmentOwnerId))
                .ForMember(model => model.OriginalFileName,
                opt => opt.MapFrom(p => p.Document.OriginalName))
                .ForMember(model => model.FileExtension,
                opt => opt.MapFrom(p => p.Document.FileExtension))
                .ForMember(model => model.Attributes,
                opt => opt.MapFrom(p => p.Document.Attributes))
                .ForMember(model => model.EditedAt,
                opt => opt.MapFrom(p => p.Document.EditedAt))
                .ForMember(model => model.CreatedAt,
                opt => opt.MapFrom(p => p.Document.CreatedAt))
                .ForMember(model => model.ImageUrl,
                opt => opt.MapFrom(p => p.Document.FileUrl))
                .ForMember(model => model.IsAnnotated,
                opt => opt.MapFrom(p => p.Document.IsAnnotated));
        }
    }
}
