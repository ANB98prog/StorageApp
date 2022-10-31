using Microsoft.AspNetCore.Mvc;
using Storage.Application.Files.Commands.Video.SplitIntoFrames;
using Storage.Application.Files.Queries.Attributes;
using Storage.WebApi.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.WebApi.Controllers.Attributes
{
    /// <summary>
    /// Endpoint to work with attributes
    /// </summary>
    [Produces("application/json")]
    [Route("api/attributes")]
    public class AttributesController : BaseController
    {
        /// <summary>
        /// Gets attributes list
        /// </summary>
        /// <param name="query">Attributes query</param>
        /// <returns>Attributes lists</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<AttributesListVm> GetAttributesListAsync([FromQuery] string? attr, [FromQuery] int? pageSize, [FromQuery] int? pageNumber)
        {
            var query = new GetAttributesListQuery
            {
                Query = attr
            };

            if (pageSize != null)
                query.PageSize = pageSize.Value;
            if (pageNumber != null)
                query.PageNumber = pageNumber.Value;

            return await Mediator.Send(query);
        }
    }
}
