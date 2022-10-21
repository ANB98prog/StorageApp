using Newtonsoft.Json;
using System;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// File info model
    /// </summary>
    public class FileInfoModel
    {
        /// <summary>
        /// File id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// System file name
        /// </summary>
        [JsonProperty("SystemName")]
        public string Name { get; set; }

        /// <summary>
        /// File path
        /// </summary>
        public string FilePath { get; set; }
    }
}
