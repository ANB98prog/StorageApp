using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.WebApi.Models;
using Storage.Domain;
using Storage.Application.Files.Commands.UploadAnnotatedFiles;
using Storage.Application.Files.Commands.PrepareAnnotatedFiles;

namespace Storage.WebApi.Controllers.AnnotatedFiles
{
    [Produces("application/json")]
    [Route("api/files/annotated")]
    public class AnnotatedFilesController : BaseController
    {
        /// <summary>
        /// Uploads annotated files
        /// </summary>
        /// <param name="request">Upload request</param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ActionResult> UploadAnnotatedDataAsync([FromForm] UploadAnnotatedDataRequestModel request)
        {
            var command = Mapper.Map<UploadAnnotatedDataRequestModel, UploadAnnotatedFilesCommand>(request);

            var annotatedFilesIds = await Mediator.Send(command);

            return Created("", annotatedFilesIds);
        }

        /// <summary>
        /// Prepares annotated data
        /// </summary>
        /// <param name="ids">Annotated files ids</param>
        /// <returns>Path to zip archive to download</returns>
        [HttpPost("prepare")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> PrepareAnnotatedDataAsync([FromBody] PrepareAnnotatedDataRequestModel request)
        {
            var command = Mapper.Map<PrepareAnnotatedDataRequestModel, PrepareAnnotatedFilesCommand>(request);
            command.UserId = UserId;

            var preparedFilePath = await Mediator.Send(command);

            return Created("", preparedFilePath);
        }

        /// <summary>
        /// Gets annotation formats
        /// </summary>
        /// <returns>List of formats</returns>
        [HttpGet("formats")]
        public async Task<ActionResult<List<string>>> GetAnnotationsFormatsAsync()
        {
            return Ok(Enum.GetNames(typeof(AnnotationFormats)));
        }
    }
}
