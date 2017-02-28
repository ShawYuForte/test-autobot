using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "DataSources")]
    public class VmixPresetDataSources
    {
        [XmlElement(ElementName = "datasources")]
        public string Datasources { get; set; }
    }
}