using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Storage.WebApi.Common.Validators;

namespace Storage.WebApi.Controllers.Images
{
    [Route("api/images/upload")]
    public class ImageUploadController : BaseController
    {
        [HttpPost]
        
        public async Task<IActionResult> UploadImageAsync(IFormFile file)
        {
            if(!file.IsValid())
            {

            }

            return Ok("Single image uploaded");
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
