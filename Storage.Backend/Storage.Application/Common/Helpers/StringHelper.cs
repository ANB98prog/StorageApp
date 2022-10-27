using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Storage.Application.Common.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Gets text in under_score format
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Formated string</returns>
        public static string ToUnderScore(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var capitalLetters = Regex.Matches(text, @"[A-Z]");

            foreach (var letter in capitalLetters)
            {
                var l = letter.ToString();
                text = Regex.Replace(text, l, $"_{l.ToLower()}");
            }

            text = text.TrimStart('_');

            return text;
        }

        /// <summary>
        /// Gets text in under_score format
        /// </summary>
        /// <param name="text">String to format</param>
        /// <returns>Formated string</returns>
        public static string ToUnderCameCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var letters = text.ToCharArray();

            return $"{letters[0].ToString().ToLowerInvariant()}{string.Join("",letters.Skip(1))}";
        }

        /// <summary>
        /// Converts directory path to url format
        /// </summary>
        /// <param name="path">Path to convert</param>
        /// <returns>Url</returns>
        public static string ConvertPathToUrl(this string path)
        {
            return string.Join("/", (path.Split(Path.DirectorySeparatorChar)));
        }
    }
}
