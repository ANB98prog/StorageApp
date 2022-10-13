using Elasticsearch;
using System.Text.RegularExpressions;

namespace Storage.Application.Common.Helpers
{
    public static class ElasticHelper
    {
        /// <summary>
        /// Gets index name from class name
        /// </summary>
        /// <param name="className">Class name</param>
        /// <returns>Formated index name</returns>
        public static string GetFormattedIndexName(string className)
        {
            if(string.IsNullOrWhiteSpace(className))
                return string.Empty;

            if (!className.Equals("Model"))
            {
                className = Regex.Replace(className, "Model", string.Empty);
            }

            return className.ToUnderScore();
        }

        /// <summary>
        /// Gets formatted property name
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Formated property name</returns>
        public static string GetFormattedPropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return string.Empty;

            return propertyName.ToUnderCameCase();
        }

        /// <summary>
        /// Returns keyword property
        /// </summary>
        /// <param name="propertyName">PropertyName</param>
        /// <returns></returns>
        public static string Keyword(this string propertyName)
        {
            return $"{propertyName}.{ElasticConstants.KEYWORD_PROPERTY}";
        }
    }
}
