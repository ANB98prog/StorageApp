using Microsoft.AspNetCore.Mvc;
using Storage.Application.Common.Models;
using Storage.Application.Files.Commands.DeleteFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.WebApi.Controllers.Files
{
    /// <summary>
    /// Endpoint to work with files
    /// </summary>
    [Produces("application/json")]
    [Route("api/files")]
    public class FilesController : BaseController
    {
        /// <summary>
        /// Uploads image to storage
        /// </summary>
        /// <param name="id">File id to remove</param>
        /// <returns>Remove acknowledgment</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">BadRequest</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeleteFileModel>> RemoveFileByIdAsync([FromRoute] Guid id)
        {
            var command = new DeleteFileCommand()
            {
                FileId = id,
                UserId = UserId
            };

            return await Mediator.Send(command);
        }
    }
}
