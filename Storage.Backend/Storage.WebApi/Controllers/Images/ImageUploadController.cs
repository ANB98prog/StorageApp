using Microsoft.AspNetCore.Mvc;
using Storage.Application.Images.Commands.UploadImage;
using Storage.WebApi.Models;

namespace Storage.WebApi.Controllers.Images
{
    /// <summary>
    /// Endpoint to upload images to storage
    /// </summary>
    [Produces("application/json")]
    [Route("api/images/upload")]
    public class ImageUploadController : BaseController
    {
        /// <summary>
        /// Uploads image to storage
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>Uploaded image id </returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImageAsync([FromForm] UploadFileRequestModel request)
        {
            var command = Mapper.Map<UploadFileRequestModel, UploadImageCommand>(request);

            command.UserId = UserId;

            var imageId = await Mediator.Send(command);

            return Created("", imageId);
        }

        /// <summary>
        /// Тут должен быть zip файл
        /// </summary>
        /// <returns></returns>
        [HttpPost("many")]
        public async Task<IActionResult> UploadImagesAsync()
        {
            return Ok("Many images uploaded");
        }

        [HttpPost("annotated")]
        public async Task<IActionResult> UploadAnnotatedImagesAsync()
        {
            var bytes = System.IO.File.ReadAllBytes(Path.Combine("wwwroot", "some.txt"));

            return new FileContentResult(bytes, "plain/text");
        }
    }
}
