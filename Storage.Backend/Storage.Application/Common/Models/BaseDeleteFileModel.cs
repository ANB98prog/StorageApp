namespace Storage.Application.Common.Models
{
    /// <summary>
    /// Delete file model
    /// </summary>
    public class BaseDeleteFileModel
    {
        /// <summary>
        /// Action acknowledge
        /// </summary>
        public bool Acknowledged { get; set; } = true;
    }
}
