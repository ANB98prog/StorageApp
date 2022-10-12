using Microsoft.AspNetCore.Mvc;
using Storage.Application.Images.Commands.UploadImage;
using Storage.Application.Images.Commands.UploadManyImages;
using Storage.Application.Images.Commands.UploadManyImagesArchive;
using Storage.WebApi.Common.Exceptions;
using Storage.WebApi.Models;

namespace Storage.WebApi.Controllers.Files
{
    /// <summary>
    /// Endpoint to upload images to storage
    /// </summary>
    [Produces("application/json")]
    [Route("api/images/upload")]
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
            var command = Mapper.Map<UploadFileRequestModel, UploadImageCommand>(request);

            command.UserId = UserId;

            var imageId = await Mediator.Send(command);

            return Created("", imageId);
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
            var command = Mapper.Map<UploadFileRequestModel, UploadManyImagesArchiveCommand>(request);

            command.UserId = UserId;

            var imagesIds = await Mediator.Send(command);

            return Created("", imagesIds);
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
            var command = Mapper.Map<UploadManyFilesRequestModel, UploadManyImagesCommand>(request);

            command.UserId = UserId;

            var imagesIds = await Mediator.Send(command);

            return Created("", imagesIds);
        }
    }
}
