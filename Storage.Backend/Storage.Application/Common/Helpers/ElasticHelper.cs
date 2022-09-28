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

            var capitalLetters = Regex.Matches(className, @"[A-Z]");

            foreach(var letter in capitalLetters)
            {
                var l = letter.ToString();
                className = Regex.Replace(className, l, $"_{l.ToLower()}");
            }

            className = className.TrimStart('_');

            return className;
        }
    }
}
