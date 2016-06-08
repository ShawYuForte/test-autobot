using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forte.device.extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Creates 'a-lower-cased-slug' string for the specified phrase.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="useDashes">Whether to use dashes between the words or not. Uses by default.</param>
        /// <returns>The slug string.</returns>
        public static string Slugify(this string phrase, bool useDashes = true)
        {
            if (string.IsNullOrWhiteSpace(phrase)) throw new ArgumentException($"Cannot slugify empty {nameof(phrase)}");

            // Remove accent (replace symbols like "éåäöíØ" with standard ones "eaaoiO")
            var str = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(phrase));

            // Remove non alpha-numeric symbols
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "", RegexOptions.IgnoreCase).Trim().ToLower();

            // Replace multiple spaces into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // Replace spaces with hyphens
            str = Regex.Replace(str, @"\s", "-");

            if (!useDashes)
            {
                str = new string(str.ToCharArray()
                    .Where(c => c != '-')
                    .ToArray());
            }

            // Return lower cased slug
            return str.ToLower();
        }
    }
}
