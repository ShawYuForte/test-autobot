using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "Shortcuts")]
    public class VmixPresetShortcuts
    {
        [XmlElement(ElementName = "ArrayOfShortcut")]
        public VmixPresetArrayOfShortcut ArrayOfShortcut { get; set; }
    }
}