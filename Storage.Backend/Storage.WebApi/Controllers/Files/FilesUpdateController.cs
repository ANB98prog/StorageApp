using Microsoft.AspNetCore.Mvc;
using Storage.Application.Common.Models;
using Storage.Application.Files.Commands.DeleteFile;
using Storage.Application.Files.Commands.UpdateFile;
using Storage.Application.Files.Commands.UpdateGroupFiles;
using Storage.Application.Files.Commands.UpdateManyFiles;
using Storage.WebApi.Common.Exceptions;
using Storage.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.WebApi.Controllers.Files
{
    /// <summary>
    /// Endpoint to work with files
    /// </summary>
    [Produces("application/json")]
    [Route("api/file/update")]
    public class FilesUpdateController : BaseController
    {
        /// <summary>
        /// Updates file attributes
        /// </summary>
        /// <param name="fileId">File id to update</param>
        /// <param name="request">File attributes</param>
        /// <returns>Update acknowledgment</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">BadRequest</response>
        [HttpPatch("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<UpdatedVm> UpdateFileByIdAsync([FromRoute] Guid fileId, [FromBody] UpdateFileRequestModel request)
        {
            var command = new UpdateFileCommand()
            {
                UserId = UserId,
                FileId = fileId,
                Attributes = request.Attributes
            };

            return await Mediator.Send(command);
        }

        /// <summary>
        /// Updates file attributes
        /// </summary>
        /// <param name="request">Files update request</param>
        /// <returns>Update acknowledgment</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">BadRequest</response>
        [HttpPatch("many")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<UpdatedManyVm> UpdateManyFilesAsync([FromBody] List<UpdateManyFilesRequestModel> request)
        {
            var command = new UpdateManyFilesCommand()
            {
                UserId = UserId,
                Updates = Mapper.Map<List<UpdateManyFilesRequestModel>, List<FileUpdateData>>(request)
            };

            return await Mediator.Send(command);
        }

        /// <summary>
        /// Updates groups of files' attributes
        /// </summary>
        /// <param name="request">Files update request</param>
        /// <returns>Update acknowledgment</returns>
        /// <response code="200">Successfully</response>
        /// <response code="400">BadRequest</response>
        [HttpPatch("group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserfriendlyException), StatusCodes.Status400BadRequest)]
        public async Task<UpdatedManyVm> UpdateGroupFilesAsync([FromBody] UpdateGroupFilesRequestModel request)
        {
            var command = new UpdateGroupFilesCommand()
            {
                UserId = UserId,
                Attributes = request.Attributes,
                FilesIds = request.FilesIds
            };

            return await Mediator.Send(command);
        }
    }
}
