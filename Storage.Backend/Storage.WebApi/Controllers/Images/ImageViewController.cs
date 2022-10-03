using Microsoft.AspNetCore.Mvc;
using Storage.Application.Images.Queries.GetImage;
using Storage.Application.Images.Queries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.WebApi.Controllers.Images
{
    [Route("api/images")]
    public class ImageViewController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageVm>> Get(Guid id)
        {
            var query = new GetImageByIdQuery()
            {
                Id = id
            };

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }
    }
}
