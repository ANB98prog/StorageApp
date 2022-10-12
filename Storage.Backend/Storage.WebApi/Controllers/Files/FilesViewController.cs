using Microsoft.AspNetCore.Mvc;
using Storage.Application.Files.Queries.GetFile;
using Storage.Application.Files.Queries.GetFilesList;
using Storage.Application.Files.Queries.Models;
using Storage.WebApi.Common.Exceptions;

namespace Storage.WebApi.Controllers.Files
{
    [Produces("application/json")]
    [Route("api/files")]
    public class FilesViewController : BaseController
    {
        /// <summary>
        /// Gets file by id
        /// </summary>
        /// <param name="id">File id</param>
        /// <returns>File details</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<FileVm>> GetAsync(Guid id)
        {
            var query = new GetFileByIdQuery()
            {
                Id = id
            };

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Gets files by query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>Files details</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<FilesListVm>> SearchAsync([FromQuery] GetFilesListQuery query)
        {
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }
    }
}
