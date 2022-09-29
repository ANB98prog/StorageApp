using Microsoft.AspNetCore.Mvc;
using Storage.Application.Images.Commands.UploadImage;
using Storage.WebApi.Models;

namespace Storage.WebApi.Controllers.Images
{
    [Route("api/images/upload")]
    public class ImageUploadController : BaseController
    {
        [HttpPost]        
        public async Task<IActionResult> UploadImageAsync([FromForm] UploadFileRequestModel request)
        {
            var command = Mapper.Map<UploadFileRequestModel, UploadImageCommand>(request);

            command.UserId = UserId;

            var imageId = await Mediator.Send(command);

            return Ok(imageId);
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
