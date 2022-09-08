namespace Storage.WebApi.Common.Validators
{
    /// <summary>
    /// IFormFile validator extension
    /// </summary>
    public static class FormFileValidator
    {
        /// <summary>
        /// Is model valid
        /// </summary>
        /// <param name="file">Form file</param>
        /// <returns>
        /// true if valid
        /// else false
        /// </returns>
        public static bool IsValid(this IFormFile file)
        {
            return (file == null || file.Length == 0)
                ? false : true;
        }
    }
}
