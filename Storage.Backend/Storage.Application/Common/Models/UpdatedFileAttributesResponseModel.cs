using AutoMapper;
using Elasticsearch.Models;
using Mapper;
using Newtonsoft.Json;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Updated file attributes response model
    /// </summary>
    public class UpdatedFileAttributesResponseModel : BaseFileActionModel
    {
        public UpdatedFileAttributesResponseModel(bool acknowledged)
        {
            Acknowledged = acknowledged;
        }
    }
}
