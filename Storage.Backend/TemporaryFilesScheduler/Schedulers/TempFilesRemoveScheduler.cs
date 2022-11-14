using Serilog;
using TemporaryFilesScheduler.Scheduling;

namespace TemporaryFilesScheduler.Schedulers
{
    /// <summary>
    /// Temporary files remover scheduler
    /// </summary>
    public class TempFilesRemoveScheduler : IScheduledTask
    {        
        public TimeSpan TimeStep { get; }

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Maximum age of a file after which it will be deleted
        /// </summary>
        private readonly TimeSpan _fileMaxAge;

        /// <summary>
        /// Temporary files path
        /// </summary>
        private readonly string _tempFilesPath;

        /// <summary>
        /// Initializes class instance of <see cref="TempFilesRemoveScheduler"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="timeStep">Launch frequency</param>
        /// <param name="fileMaxAge">Maximum age of a file</param>
        /// <param name="tempFilesPath">Temporary files path</param>
        public TempFilesRemoveScheduler(ILogger logger, TimeSpan timeStep, TimeSpan fileMaxAge, string tempFilesPath)
        {
            TimeStep = timeStep;
            _logger = logger;
            _fileMaxAge = fileMaxAge;
            _tempFilesPath = tempFilesPath;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (Directory.Exists(_tempFilesPath))
                {
                    _logger.Information($"Try to remove temp files in {_tempFilesPath}");

                    RemoveTemporaryFiles();
                    RemoveEmptyDirectories(); 
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Service} Unexpected error occured while remove temporary files!", "TempFilesRemoveScheduler");
            }
        }

        /// <summary>
        /// Removes temporary files
        /// </summary>
        private void RemoveTemporaryFiles()
        {
            var files = GetTempFiles();

            _logger.Information($"Temporary files count: {files.Count()}");

            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    var fileAge = DateTime.UtcNow - fileInfo.CreationTimeUtc;

                    _logger.Information($"File age: `{fileAge}`");

                    if (fileAge >= _fileMaxAge)
                    {
                        fileInfo.Delete();
                    }
                }
                catch (Exception)
                {
                    /*Ничего не делаем, т.к. файл может использоваться*/
                }
            }
        }

        /// <summary>
        /// Gets temporary files
        /// </summary>
        /// <returns>List of temp files paths</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string[] GetTempFiles()
        {
            if (string.IsNullOrWhiteSpace(_tempFilesPath))
            {
                throw new ArgumentNullException("Temp files path");
            }
            
            return Directory.GetFiles(_tempFilesPath, "*.*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Removes empty directories
        /// </summary>
        private void RemoveEmptyDirectories()
        {
            var emptyDirectories = GetEmptyDirectories();

            foreach (var dir in emptyDirectories)
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch (Exception)
                {
                    /*Ничего не делаем, т.к. директория может использоваться*/
                }
            }
        }

        /// <summary>
        /// Gets empty directories
        /// </summary>
        /// <returns>List of empty directories paths</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string[] GetEmptyDirectories()
        {
            if (string.IsNullOrWhiteSpace(_tempFilesPath))
            {
                throw new ArgumentNullException("Temp files path");
            }

            var directories = Directory.GetDirectories(_tempFilesPath);

            return directories.Where(d => 
                        !Directory.GetFiles(d).Any())
                            .ToArray();
        }
    }
}
