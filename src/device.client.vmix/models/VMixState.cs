using System.Collections.Generic;
using System.Xml.Serialization;

namespace forte.devices.models
{
    public class VMixState
    {
        public string Version { get; set; }
        public List<VMixInput> Inputs { get; set; }
        [XmlAttribute("Preview")]
        public string PreviewNumber { get; set; }
        [XmlIgnore]
        public VMixInput Preview { get; set; }
        [XmlIgnore]
        public VMixInput Active { get; set; }
        [XmlAttribute("Active")]
        public string ActiveNumber { get; set; }
        public bool Recording { get; set; }
        public bool Streaming { get; set; }
        public bool Playlist { get; set; }
        public List<VMixAudio> Audio { get; set; }
    }
}