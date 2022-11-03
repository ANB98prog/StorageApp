namespace TemporaryFilesScheduler
{
    public static class Constants
    {
        /// <summary>
        /// Default temporary files remover scheduler time step (30 min)
        /// </summary>
        public static TimeSpan DEFAULT_TEMP_FILES_REMOVE_SCHEDULER_STEP => new TimeSpan(0, 0, 30, 0);
    }
}
