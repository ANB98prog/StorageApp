using System;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    public interface IVideoFilesService
    {
        /// <summary>
        /// Splits video into frames
        /// </summary>
        /// <param name="videoFileId">Video file id</param>
        /// <param name="step">Frames step</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Path to splited frames</returns>
        public Task<string> SplitIntoFramesAsync(Guid videoFileId, int step = 0, CancellationToken cancellationToken = default);
    }
}
