namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Preparing files response model
    /// </summary>
    public class PrepareFilesResponseModel
    {
        /// <summary>
        /// Preparing task
        /// </summary>
        public Storage.Domain.Task Task { get; set; }

        /// <summary>
        /// File to resulting file
        /// </summary>
        public string FilePath { get; set; }
    }
}
