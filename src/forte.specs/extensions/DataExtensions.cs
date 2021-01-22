using System;
using System.Linq;

namespace forte.extensions
{
    public static class DataExtensions
    {
        public static bool SameAs(this byte[] arr1, byte[] arr2)
        {
            // If both arrays are null, treat them as same
            if (arr1 == null && arr2 == null)
            {
                return true;
            }

            // if only one is null, not the other, not the same
            if (arr1 == null || arr2 == null)
            {
                return false;
            }

            // if their size is different, no same
            if (arr1.Length != arr2.Length)
            {
                return false;
            }

            return !arr1.Where((t, index) => t != arr2[index]).Any();
        }

        /// <summary>
        ///     Returns the integer parsed value of the string, or default value if parsing failed
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToIntParsed(this string input, int defaultValue = 0)
        {
            int parsed;
            return int.TryParse(input, out parsed) ? parsed : defaultValue;
        }

        /// <summary>
        ///     Returns the nullable integer parsed value of the string, or default value if parsing failed
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? ToNullableIntParsed(this string input, int? defaultValue = null)
        {
            int parsed;
            return int.TryParse(input, out parsed) ? parsed : defaultValue;
        }
    }
}
