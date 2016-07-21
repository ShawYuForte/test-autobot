using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "OutputsFullscreen")]
    public class VmixPresetOutputsFullscreen
    {
        [XmlElement(ElementName = "Overlay0")]
        public string Overlay0 { get; set; }
        [XmlElement(ElementName = "Overlay1")]
        public string Overlay1 { get; set; }
        [XmlElement(ElementName = "Overlay2")]
        public string Overlay2 { get; set; }
        [XmlElement(ElementName = "Overlay3")]
        public string Overlay3 { get; set; }
        [XmlElement(ElementName = "Overlay4")]
        public string Overlay4 { get; set; }
        [XmlElement(ElementName = "Overlay5")]
        public string Overlay5 { get; set; }
        [XmlElement(ElementName = "Input")]
        public string Input { get; set; }
        [XmlElement(ElementName = "Display")]
        public string Display { get; set; }
    }
}