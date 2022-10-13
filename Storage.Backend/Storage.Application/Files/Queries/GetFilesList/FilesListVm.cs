using AutoMapper;
using Elasticsearch.Models;
using Mapper;
using Newtonsoft.Json;
using Storage.Application.Files.Queries.Models;
using Storage.Domain;
using System.Collections.Generic;

namespace Storage.Application.Files.Queries.GetFilesList
{
    public class FilesListVm : IMapWith<SearchResponse<BaseFile>>
    {
        /// <summary>
        /// Files list
        /// </summary>
        public IList<FileVm> Files { get; set; }

        /// <summary>
        /// Files count
        /// </summary>
        public int Count 
        {
            get
            {
                return Files?.Count ?? 0;
            }
        }

        /// <summary>
        /// Items total count
        /// </summary>
        private long _totalCount;

        /// <summary>
        /// Files total count
        /// </summary>
        [JsonProperty("total_count")]
        public long TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
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
            profile.CreateMap<SearchResponse<BaseFile>, FilesListVm>()
                .ForMember(model => model.Files,
                    opt => opt.MapFrom(p => p.Documents))
                .ForMember(model => model.Count,
                    opt => opt.MapFrom(p => p.Count))
                .ForMember(model => model.TotalCount,
                    opt => opt.MapFrom(p => p.Total));

            profile.CreateMap<HitModel<BaseFile>, FileVm>()
                .ForMember(model => model.Id,
                opt => opt.MapFrom(p => p.Document.Id))
                .ForMember(model => model.OwnerId,
                opt => opt.MapFrom(p => p.Document.OwnerId))
                .ForMember(model => model.DepartmentOwnerId,
                opt => opt.MapFrom(p => p.Document.DepartmentOwnerId))
                .ForMember(model => model.OriginalFileName,
                opt => opt.MapFrom(p => p.Document.OriginalName))
                .ForMember(model => model.Attributes,
                opt => opt.MapFrom(p => p.Document.Attributes))
                .ForMember(model => model.EditedAt,
                opt => opt.MapFrom(p => p.Document.EditedAt))
                .ForMember(model => model.CreatedAt,
                opt => opt.MapFrom(p => p.Document.CreatedAt))
                .ForMember(model => model.FileUrl,
                opt => opt.MapFrom(p => p.Document.FileUrl))
                .ForMember(model => model.IsAnnotated,
                opt => opt.MapFrom(p => p.Document.IsAnnotated));
        }
    }
}
