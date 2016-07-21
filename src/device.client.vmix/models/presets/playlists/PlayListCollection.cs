using System.Xml.Serialization;

namespace forte.devices.models.presets.playlists
{
    [XmlRoot(ElementName = "PlayListCollection")]
    public class PlayListCollection
    {
        [XmlElement(ElementName = "PlayLists")]
        public PlayLists PlayLists { get; set; }
        [XmlElement(ElementName = "CurrentPlayList")]
        public PlayList CurrentPlayList { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }
}