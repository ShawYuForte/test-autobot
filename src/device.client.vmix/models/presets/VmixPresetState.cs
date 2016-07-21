using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "State")]
    public class VmixPresetState
    {
        [XmlAttribute(AttributeName = "Main")]
        public string Main { get; set; }
        [XmlAttribute(AttributeName = "Preview")]
        public string Preview { get; set; }
        [XmlAttribute(AttributeName = "PlayLists")]
        public string PlayLists { get; set; }
    }
}