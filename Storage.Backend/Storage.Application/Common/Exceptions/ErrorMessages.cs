using System;

namespace Storage.Application.Common.Exceptions
{
    public static class ErrorMessages
    {
        /// <summary>
        /// Error message if some required param are empty
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns>Error message</returns>
        public static string ArgumentNullExeptionMessage(string paramName) => $"Parameter '{paramName}' is not set!";
        
        /// <summary>
        /// Unexpected error message while file uploading
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE = "Unexpected error occured while file uploading.";

        /// <summary>
        /// Unexpected error message while files uploading
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE = "Unexpected error occured while files uploading.";

        /// <summary>
        /// Unexpected error message while archive file uploading
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE = "Unexpected error occured while archive file uploading.";

        /// <summary>
        /// Unexpected error message while download files
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILES_MESSAGE = "Unexpected error occured while download files.";

        /// <summary>
        /// Unexpected error message while download file
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILE_MESSAGE = "Unexpected error occured while download file.";

        /// <summary>
        /// Unexpected error message while remove file
        /// </summary>
        public static string UNEXPECTED_ERROR_WHILE_FILE_REMOVE_MESSAGE => "Unexpected error occured while remove file.";

        /// <summary>
        /// Unexpected error message while remove files
        /// </summary>
        public static string UNEXPECTED_ERROR_WHILE_FILES_REMOVE_MESSAGE => "Unexpected error occured while remove files.";

        /// <summary>
        /// Error message if uploaded not supported file type
        /// </summary>
        /// <param name="fileExt">File extension</param>
        /// <returns>Error message</returns>
        public static string NotSupportedFileExtension(string fileExt) => $"Not supported file extension {fileExt}";

        /// <summary>
        /// Error message while download file if it not found
        /// </summary>
        public static string FileNotFoundErrorMessage(string fileName) => $"Could not find file '{fileName}'";

        /// <summary>
        /// File not found exception
        /// </summary>
        public static string FILE_NOT_FOUND_ERROR_MESSAGE => "File not found!";

        /// <summary>
        /// Error message if required parameter are empty
        /// </summary>
        public static string EmptyRequiredParameterErrorMessage(string paramName) => $"Required paramenter cannot be empty. Parameter name: '{paramName}'";

        /// <summary>
        /// Error message if invalid required parameter
        /// </summary>
        public static string InvalidRequiredParameterErrorMessage(string validationMessage) => $"Invalid required paramenter. Validation message: '{validationMessage}'";

        /// <summary>
        /// Unexpected error message while add item to elastic storage
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_ADD_ITEM_TO_STORAGE_MESSAGE = "Unexpected error occured while add item to elastic storage.";

        /// <summary>
        /// Unexpected error message while get item info from elastic storage
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE = "Unexpected error occured while get item info from elastic storage.";

        /// <summary>
        /// Unexpected error message while get items info from elastic storage
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_GET_ITEMS_INFO_FROM_STORAGE_MESSAGE = "Unexpected error occured while get items info from elastic storage.";

        /// <summary>
        /// Item not found error message
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Error message</returns>
        public static string ItemNotFoundErrorMessage(string id) => $"Item with '{id}' id not found!";

        /// <summary>
        /// Unexpected eeor while get image by id
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_GET_IMAGE_BY_ID_MESSAGE = "Unexpected error occured while get image by id.";

        /// <summary>
        /// Unexpected error while get images list
        /// </summary>
        public const string UNEXPECTED_ERROR_WHILE_SEARCH_IMAGES_MESSAGE = "Unexpected error occured while get list of images.";

        /// <summary>
        /// Error message if passed unsupported archive type
        /// </summary>
        public static string NotSupportedArchiveTypeErrorMessage(string type) => $"Not supported archive type: '{type}'";

        /// <summary>
        /// Remove item error message
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Error message</returns>
        public static string RemovingItemFromStorageErrorMessage(string id) => $"Cannot remove item with id '{id}'";

        /// <summary>
        /// Wrong parameter format error message
        /// </summary>
        /// <param name="parameter">Parameter name</param>
        /// <param name="format">Correct format</param>
        /// <returns><Error message/returns>
        public static string WrongParameterFormatErrorMessage(string parameter, string format) => $"Parameter '{parameter}' has wrong format. Correct format is '{format}'";
    }
}
