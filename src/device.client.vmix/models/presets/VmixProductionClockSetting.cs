using System.Xml.Serialization;

namespace forte.devices.models.presets
{

    [XmlRoot(ElementName = "ProductionClockSettings")]
    public class ProductionClockSettings
    {
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "AMPM")]
        public string AMPM { get; set; }
        [XmlElement(ElementName = "Format")]
        public string Format { get; set; }
        [XmlElement(ElementName = "StartTime")]
        public string StartTime { get; set; }
        [XmlElement(ElementName = "EndTime")]
        public string EndTime { get; set; }
        [XmlElement(ElementName = "TimeZoneOffset")]
        public string TimeZoneOffset { get; set; }
    }

    [XmlRoot(ElementName = "ProductionClockSettings")]
    public class VmixProductionClockSetting
    {
        [XmlElement(ElementName = "ProductionClockSettings")]
        public ProductionClockSettings ProductionClockSettings { get; set; }
    }
}
