using System.Text.RegularExpressions;

namespace forte.devices.extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        /// Remove a certain attribute from input xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string RemoveXmlAttribute(this string xml, string attribute)
        {
            string tagRegEx = $@"<\/?{attribute}[\sa-zA-Z:=\-""\/\.0-9]*>";

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