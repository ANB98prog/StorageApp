using System;

namespace Storage.Application.Common.Helpers
{
    public static class GuidHelper
    {
        /// <summary>
        /// Truncates guid by "-" separator and returns last part
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>Last part of guid</returns>
        public static string Trunc(this Guid guid)
        {
            var partition = guid.ToString().Split("-");
            return partition[partition.Length - 1];
        }
    }
}
