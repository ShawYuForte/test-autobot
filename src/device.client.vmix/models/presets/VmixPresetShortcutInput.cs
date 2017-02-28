using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "Input")]
    public class VmixPresetShortcutInput
    {
        [XmlElement(ElementName = "Key")]
        public string Key { get; set; }
        [XmlElement(ElementName = "Number")]
        public string Number { get; set; }
    }
}