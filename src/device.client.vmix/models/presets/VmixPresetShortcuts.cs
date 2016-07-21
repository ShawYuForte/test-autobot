using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "Shortcuts")]
    public class VmixPresetShortcuts
    {
        [XmlElement(ElementName = "ArrayOfShortcut")]
        public VmixPresetShortcut ArrayOfShortcut { get; set; }
    }
}