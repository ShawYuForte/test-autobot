using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "Activators")]
    public class VmixPresetActivators
    {
        [XmlElement(ElementName = "activators")]
        public string TheActivators { get; set; }
    }
}