using Microsoft.AspNetCore.Mvc;
using Storage.Application.Images.Queries.GetImage;
using Storage.Application.Images.Queries.GetImagesList;
using Storage.Application.Images.Queries.Models;

namespace Storage.WebApi.Controllers.Images
{
    [Route("api/images")]
    public class ImageViewController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageVm>> GetAsync(Guid id)
        {
            var query = new GetImageByIdQuery()
            {
                Id = id
            };

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<ImageListVm>> SearchAsync([FromQuery] GetImagesListQuery query)
        {
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

    }
}
