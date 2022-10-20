using MediatR;
using Storage.Application.Common;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Queries.GetFilesList
{
    public class GetFilesListQuery : IRequest<FilesListVm>
    {
        /// <summary>
        /// File owner id
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// File department owner id
        /// </summary>
        public Guid? DepartmentOwnerId { get; set; }

        /// <summary>
        /// List of attributes
        /// </summary>
        public IList<string>? Attributes { get; set; } = new List<string>();

        /// <summary>
        /// Created form date
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Created To date
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Is data annotated
        /// </summary>
        public bool? IsAnnotated { get; set; }

        const int maxPageSize = Constants.MAX_PAGE_SIZE;

        private int _pageNumber = Constants.DEFAULT_PAGE_NUMBER;

        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber 
        {
            get
            {
                return _pageNumber; 
            }
            set
            {
                _pageNumber = value;

                if(_pageNumber < 0)
                {
                    _pageNumber = 0;
                }
            }
        }

        private int _pageSize = Constants.DEFAULT_PAGE_SIZE;

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
    }
}
