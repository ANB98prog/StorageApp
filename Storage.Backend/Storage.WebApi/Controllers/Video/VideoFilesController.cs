using Microsoft.AspNetCore.Mvc;
using Storage.Application.Files.Commands.Video.SplitIntoFrames;
using Storage.WebApi.Common.Exceptions;

namespace Storage.WebApi.Controllers.Video
{
    /// <summary>
    /// Endpoint to work with video files
    /// </summary>
    [Produces("application/json")]
    [Route("api/video")]
    public class VideoFilesController : BaseController
    {
        /// <summary>
        /// Splits video file into frames
        /// </summary>
        /// <param name="fileId">Video file id</param>
        /// <param name="step">Frames step</param>
        /// <returns>Path to archive with frames</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest</response>
        [HttpPost("split/{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<string> SplitIntoFramesAsync([FromRoute] Guid fileId, [FromQuery] int step)
        {
            var command = new SplitIntoFramesCommand
            {
                VideoFileId = fileId,
                FramesStep = step,
                UserId = UserId
            };

            return await Mediator.Send(command);
        }
    }
}
