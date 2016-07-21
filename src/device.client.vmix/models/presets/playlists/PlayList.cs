using System.Collections.Generic;
using System.Xml.Serialization;

namespace forte.devices.models.presets.playlists
{
    [XmlRoot(ElementName = "PlayList")]
    public class PlayList
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Loop")]
        public Loop Loop { get; set; }
        [XmlElement(ElementName = "ClearOverlays")]
        public string ClearOverlays { get; set; }
        [XmlElement(ElementName = "Manual")]
        public string Manual { get; set; }
        [XmlElement(ElementName = "PlayList")]
        public PlayList PlayListItems { get; set; }
        [XmlElement(ElementName = "PlaylistItem")]
        public List<PlaylistItem> PlaylistItem { get; set; }
    }
}