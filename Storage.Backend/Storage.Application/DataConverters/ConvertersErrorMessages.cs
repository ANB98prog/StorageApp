namespace Storage.Application.DataConverters
{
    public static class ConvertersErrorMessages
    {
        /// <summary>
        /// Error message if classes file not found
        /// </summary>
        public const string CLASSES_FILE_NOT_FOUND_ERROR_MESSAGE = "Classes file not found.";

        /// <summary>
        /// Error message if couldn't get classes from file
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_GET_CLASSES = "Could not get classes from file";

        /// <summary>
        /// Error message if couldn't get bounding boxes from file
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_GET_BBOXES = "Could not get bounding boxes from file";

        /// <summary>
        /// Error message if couldn't convert annotated data
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_CONVERT_ANNOTATION_DATA = "Unexpected error occured while convert annotated data.";

        /// <summary>
        /// Error message if annotation coordiantes has bad format
        /// </summary>
        public const string ANNOTATION_COORDINATES_COVERTION_ERROR_MESSAGE = "Annotation coordinates has invalid format.";
    }
}
