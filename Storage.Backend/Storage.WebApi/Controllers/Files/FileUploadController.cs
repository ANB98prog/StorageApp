using Microsoft.AspNetCore.Mvc;
using Storage.Application.Files.Commands.UploadFile;
using Storage.Application.Files.Commands.UploadManyFiles;
using Storage.Application.Files.Commands.UploadManyFilesArchive;
using Storage.WebApi.Common.Exceptions;
using Storage.WebApi.Models;

namespace Storage.WebApi.Controllers.Files
{
    /// <summary>
    /// Endpoint to upload files to storage
    /// </summary>
    [Produces("application/json")]
    [Route("api/files/upload")]
    public class FileUploadController : BaseController
    {
        /// <summary>
        /// Uploads file to storage
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>Uploaded image id </returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFileAsync([FromForm] UploadFileRequestModel request)
        {
            var command = Mapper.Map<UploadFileRequestModel, UploadFileCommand>(request);

            command.UserId = UserId;

            var fileId = await Mediator.Send(command);

            return Created("", fileId);
        }

        /// <summary>
        /// Uploads files to storage in zip file
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>Uploaded files ids </returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        [HttpPost("many/zip")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFilesAsync([FromForm] UploadFileRequestModel request)
        {
            var command = Mapper.Map<UploadFileRequestModel, UploadManyFilesArchiveCommand>(request);

            command.UserId = UserId;

            var filesIds = await Mediator.Send(command);

            return Created("", filesIds);
        }

        /// <summary>
        /// Uploads files to storage in separate files
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>Uploaded files ids </returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        [HttpPost("many")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFilesAsync([FromForm] UploadManyFilesRequestModel request)
        {
            var command = Mapper.Map<UploadManyFilesRequestModel, UploadManyFilesCommand>(request);

            command.UserId = UserId;

            var filesIds = await Mediator.Send(command);

            return Created("", filesIds);
        }
    }
}
