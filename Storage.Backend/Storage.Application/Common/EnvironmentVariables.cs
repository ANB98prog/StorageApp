namespace Storage.Application.Common
{
    /// <summary>
    /// Application's environment variables
    /// </summary>
    public static class EnvironmentVariables
    {
        /// <summary>
        /// Environment variable name that stores environment name
        /// </summary>
        public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// Environment variable name that stores application name
        /// </summary>
        public const string APPLICATION_NAME = "APPLICATION_NAME";

        /// <summary>
        /// Environment variable name that stores local storage directory path
        /// </summary>
        public const string LOCAL_STORAGE_DIR = "LOCAL_STORAGE_DIR";

        /// <summary>
        /// Environment variable name that stores temporary files directory path
        /// </summary>
        public const string TEMPORARY_FILES_DIR = "TEMPORARY_FILES_DIR";

        /// <summary>
        /// Environment variable name that stores elastic url
        /// </summary>
        public const string ELASTIC_URL = "ELASTIC_URL";

        /// <summary>
        /// Environment variable name that stores elastic user name
        /// </summary>
        public const string ELASTIC_USER = "ELASTIC_USER";

        /// <summary>
        /// Environment variable name that stores elastic password
        /// </summary>
        public const string ELASTIC_PASSWORD = "ELASTIC_PASSWORD";

        /// <summary>
        /// Environment variable name that stores path to ffmpeg executable
        /// </summary>
        public const string FFMPEG_EXECUTABLE_PATH = "FFMPEG_EXECUTABLE_PATH";
    }
}
