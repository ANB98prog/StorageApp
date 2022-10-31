using MediatR;
using Storage.Application.Common;

namespace Storage.Application.Files.Queries.Attributes
{
    /// <summary>
    /// Get attributes list query
    /// </summary>
    public class GetAttributesListQuery
        : IRequest<AttributesListVm>
    {
        /// <summary>
        /// Attribute search query
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
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

                if (_pageNumber < 0)
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
                _pageSize = value;
            }
        }
    }
}
