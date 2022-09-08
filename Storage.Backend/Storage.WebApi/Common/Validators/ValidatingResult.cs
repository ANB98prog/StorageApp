namespace Storage.WebApi.Common.Validators
{
    /// <summary>
    /// Validating result model
    /// </summary>
    public class ValidatingResult
    {
        /// <summary>
        /// Is property valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Exception message if property is not valid
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}
