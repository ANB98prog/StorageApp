namespace TemporaryFilesScheduler.Scheduling
{
    /// <summary>
    /// Scheduled task interface
    /// </summary>
    public interface IScheduledTask
    {
        /// <summary>
        /// Launch frequency
        /// </summary>
        public TimeSpan TimeStep { get; }

        /// <summary>
        /// Executes scheduled task
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
