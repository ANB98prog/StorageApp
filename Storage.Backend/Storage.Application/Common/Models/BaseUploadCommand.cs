using System.Collections.Generic;

namespace Storage.Application.Common.Models
{
    public class BaseUploadCommand<TResponse> 
        : BaseCommand<TResponse> where TResponse : class
    {
        /// <summary>
        /// File attributes
        /// </summary>
        public List<string> Attributes { get; set; } = new List<string>();

        /// <summary>
        /// Is data annotated
        /// </summary>
        public bool IsAnnotated { get; set; } = false;

    }
}
