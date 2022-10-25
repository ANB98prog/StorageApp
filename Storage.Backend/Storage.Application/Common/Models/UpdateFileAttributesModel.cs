using System;
using System.Collections.Generic;

namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Update file attributes model
    /// </summary>
    public class UpdateFileAttributesModel
    {
        /// <summary>
        /// File id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Files attributes
        /// </summary>
        public List<string> Attributes { get; set; }
    }
}
