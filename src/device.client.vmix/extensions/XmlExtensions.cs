#region

using System.Text.RegularExpressions;

#endregion

namespace forte.devices.extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        ///     Remove certain attribute including its content from input xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string RemoveXmlAttribute(this string xml, string attribute)
        {
            string tagRegEx = $@"<{attribute}[\sa-zA-Z:=\-""\/\.0-9]*>[\s\S]*?<\/?{attribute}[\sa-zA-Z:=\-""\/\.0-9]*>";

            var match = Regex.Match(xml, tagRegEx);

            while (match.Success)
            {
                xml = xml.Replace(match.Value, string.Empty);
                match = match.NextMatch();
            }

            return xml.RemoveXmlAttributeTags(attribute);
        }

        /// <summary>
        ///     Remove certain attribute tags from input xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string RemoveXmlAttributeTags(this string xml, string attribute)
        {
            string tagRegEx = $@"<\/?{attribute}[\sa-zA-Z:=\-""\/\.0-9]*\/?>";

            var match = Regex.Match(xml, tagRegEx);

            while (match.Success)
            {
                xml = xml.Replace(match.Value, string.Empty);
                match = match.NextMatch();
            }

            return xml;
        }
    }
}