using System.Collections.Generic;
using System.Xml.Serialization;

namespace forte.devices.models.presets.playlists
{
    [XmlRoot(ElementName = "PlayLists")]
    public class PlayLists
    {
        [XmlElement(ElementName = "PlayList")]
        public List<PlayList> PlayList { get; set; }
    }
}