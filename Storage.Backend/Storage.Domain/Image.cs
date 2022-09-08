namespace Storage.Domain
{
    /// <summary>
    /// Image domain model
    /// </summary>
    public class Image : BaseFile
    {       
        /// <summary>
        /// Compressed file path
        /// </summary>
        public string CompressedFilePath { get; set; }
    }
}
