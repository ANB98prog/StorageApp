namespace Storage.WebApi.Common
{
    /// <summary>
    /// Web api constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Describes one gigabyte in bytes
        /// </summary>
        public static readonly int ONE_GIGABYTE_IN_BYTES = 1073741824;

        /// <summary>
        /// Describes two gigabyte in bytes
        /// </summary>
        public static readonly long TWO_GIGABYTE_IN_BYTES = 2147483648;

        /// <summary>
        /// Describes ten gigabytes in bytes
        /// </summary>
        public static readonly long TEN_GIGABYTE_IN_BYTES = 10000000000;

        /// <summary>
        /// Describes default remove files time
        /// </summary>
        /// <remarks>Default: `1 hour`</remarks>
        public static readonly TimeSpan REMOVE_DEFAULT_TIME = new TimeSpan(1, 0, 0);

        /// <summary>
        /// Describes default max file age
        /// </summary>
        /// <remarks>Default: `24 hours`</remarks>
        public static readonly TimeSpan REMOVE_DEFAULT_MAX_FILE_AGE = new TimeSpan(23, 59, 0);
    }
}
