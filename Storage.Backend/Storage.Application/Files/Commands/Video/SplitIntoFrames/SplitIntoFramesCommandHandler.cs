using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.Video.SplitIntoFrames
{
    /// <summary>
    /// Command to split video into frames
    /// </summary>
    public class SplitIntoFramesCommandHandler
        : IRequestHandler<SplitIntoFramesCommand, string>
    {
        /// <summary>
        /// Video files service
        /// </summary>
        private readonly IVideoFilesService _videoFilesService;

        /// <summary>
        /// Initializes class instance of <see cref="SplitIntoFramesCommandHandler"/>
        /// </summary>
        /// <param name="videoFilesService">Video files service</param>
        public SplitIntoFramesCommandHandler(IVideoFilesService videoFilesService)
        {
            _videoFilesService = videoFilesService;
        }

        public async Task<string> Handle(SplitIntoFramesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var framesArchivePath = await _videoFilesService.SplitIntoFramesAsync(request.VideoFileId, request.FramesStep, cancellationToken);

                return framesArchivePath.ConvertPathToUrl();
            }
            catch (ArgumentNullException ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (FileHandlerServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_VIDEO_SPLITTING);
            }
        }
    }
}
