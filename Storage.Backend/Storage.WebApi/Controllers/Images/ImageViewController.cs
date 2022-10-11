using Microsoft.AspNetCore.Mvc;
using Storage.Application.Images.Queries.GetImage;
using Storage.Application.Images.Queries.GetImagesList;
using Storage.Application.Images.Queries.Models;
using Storage.WebApi.Common.Exceptions;

namespace Storage.WebApi.Controllers.Images
{
    [Produces("application/json")]
    [Route("api/images")]
    public class ImageViewController : BaseController
    {
        /// <summary>
        /// Gets image by id
        /// </summary>
        /// <param name="id">Image id</param>
        /// <returns>Image details</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Gets images by query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>Images details</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<ImageListVm>> SearchAsync([FromQuery] GetImagesListQuery query)
        {
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }
    }
}
