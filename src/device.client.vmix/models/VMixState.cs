using System.Collections.Generic;
using System.Xml.Serialization;

namespace forte.devices.models
{
    public class VmixState
    {
        public string Version { get; set; }
        public List<VmixInput> Inputs { get; set; }
        [XmlAttribute("Preview")]
        public string PreviewNumber { get; set; }
        [XmlIgnore]
        public VmixInput Preview { get; set; }
        [XmlIgnore]
        public VmixInput Active { get; set; }
        [XmlAttribute("Active")]
        public string ActiveNumber { get; set; }
        public bool Recording { get; set; }
        public bool Streaming { get; set; }
        public bool Playlist { get; set; }
        public List<VmixAudio> Audio { get; set; }
    }
}