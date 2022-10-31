using Newtonsoft.Json;
using System.Collections.Generic;

namespace Storage.Application.Files.Queries.Attributes
{
    /// <summary>
    /// Attributes list view model
    /// </summary>
    public class AttributesListVm
    {
        /// <summary>
        /// Attributes list
        /// </summary>
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; } = new List<string>();

        /// <summary>
        /// Attributes count
        /// </summary>
        [JsonProperty("count")]
        public int Count
        {
            get
            {
                return Attributes?.Count ?? 0;
            }
        }

        /// <summary>
        /// Attributes total count
        /// </summary>
        private long _totalCount;

        /// <summary>
        /// Attributes total count
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
                _pageSize = value;
            }
        }
    }
}
