using System.Xml.Serialization;

namespace forte.devices.models.presets
{

    [XmlRoot(ElementName = "ProductionClockSettings")]
    public class VmixProductionClockSetting
    {
        [XmlElement(ElementName = "ProductionClockSettings")]
        public ProductionClockSettings ProductionClockSettings { get; set; }
       
    }
}
