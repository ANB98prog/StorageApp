using MediatR;
using System;

namespace Storage.Application.Files.Commands.Video.SplitIntoFrames
{
    /// <summary>
    /// Command to split video on frames
    /// </summary>
    public class SplitIntoFramesCommand
        : IRequest<string>
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Video file id
        /// </summary>
        public Guid VideoFileId { get; set; }

        /// <summary>
        /// Frames step
        /// </summary>
        public int FramesStep { get; set; } = 0;
    }
}
