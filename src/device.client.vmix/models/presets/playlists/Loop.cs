/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Xml.Serialization;

namespace forte.devices.models.presets.playlists
{
    [XmlRoot(ElementName = "Loop")]
    public class Loop
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}