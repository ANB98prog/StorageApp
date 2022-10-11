using Microsoft.AspNetCore.Mvc;
using Storage.Application.Common.Models;
using Storage.Application.Files.Commands.DeleteFile;
using Storage.Application.Files.Commands.DeleteFiles;

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
        /// Removes file from storage
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

        /// <summary>
        /// Removes files from storage
        /// </summary>
        /// <param name="ids">Files ids to remove</param>
        /// <returns>Remove acknowledgment</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">BadRequest</response>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeleteFilesModel>> RemoveFilesAsync([FromBody] List<string> ids)
        {
            var command = new DeleteFilesCommand()
            {
                FilesIds = ids,
                UserId = UserId
            };

            return await Mediator.Send(command);
        }
    }
}
