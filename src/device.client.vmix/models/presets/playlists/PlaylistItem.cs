using System.Xml.Serialization;

namespace forte.devices.models.presets.playlists
{
    [XmlRoot(ElementName = "PlaylistItem")]
    public class PlaylistItem
    {
        [XmlElement(ElementName = "DisplayType")]
        public string DisplayType { get; set; }
        [XmlElement(ElementName = "VideoSelectedIndex")]
        public string VideoSelectedIndex { get; set; }
        [XmlElement(ElementName = "Transition")]
        public string Transition { get; set; }
        [XmlElement(ElementName = "TransitionDurationString")]
        public string TransitionDurationString { get; set; }
        [XmlElement(ElementName = "DurationString")]
        public string DurationString { get; set; }
        [XmlElement(ElementName = "PositionString")]
        public string PositionString { get; set; }
        [XmlElement(ElementName = "InputKey")]
        public string InputKey { get; set; }
    }
}