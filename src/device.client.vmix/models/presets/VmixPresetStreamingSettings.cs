using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "StreamingSettings")]
    public class VmixPresetStreamingSettings
    {
        [XmlElement(ElementName = "StreamingSetting")]
        public VmixPresetStreamingSetting StreamingSetting { get; set; }
        [XmlAttribute(AttributeName = "SelectedIndex")]
        public string SelectedIndex { get; set; }
    }
}