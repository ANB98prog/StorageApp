using AutoMapper;
using Elasticsearch.Models;
using Mapper;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Update bulk files attributes model
    /// </summary>
    public class UpdateBulkFilesAttributesModel : BaseFileActionModel, IMapWith<UpdateManyResponse>
    {
        /// <summary>
        /// Updated items count
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Deleting errors
        /// </summary>
        [JsonProperty("errors")]
        public List<DeleteErrorModel> Errors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateManyResponse, UpdateBulkFilesAttributesModel>()
                .ForMember(m => m.Acknowledged,
                    opt => opt.MapFrom(f => f.Acknowledged))
                .ForMember(m => m.Count,
                    opt => opt.MapFrom(f => f.Count))
                .ForMember(m => m.Errors,
                    opt => opt.MapFrom(f => f.ItemsWithErrors));

            profile.CreateMap<AddDocumentError, DeleteErrorModel>()
                .ForMember(m => m.ErrorMessage,
                    opt => opt.MapFrom(f => f.Error))
                .ForMember(m => m.FileId,
                    opt => opt.MapFrom(f => f.Id));

        }
    }
}
