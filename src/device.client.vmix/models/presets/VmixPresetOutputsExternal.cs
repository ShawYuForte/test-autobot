using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "OutputsExternal")]
    public class VmixPresetOutputsExternal
    {
        [XmlElement(ElementName = "VirtualFrameRate")]
        public string VirtualFrameRate { get; set; }
        [XmlElement(ElementName = "VirtualFrameRateInterlaced")]
        public string VirtualFrameRateInterlaced { get; set; }
        [XmlElement(ElementName = "ExternalFrameRate")]
        public string ExternalFrameRate { get; set; }
        [XmlElement(ElementName = "ExternalFrameRateInterlaced")]
        public string ExternalFrameRateInterlaced { get; set; }
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
        [XmlElement(ElementName = "VirtualSize")]
        public string VirtualSize { get; set; }
        [XmlElement(ElementName = "ExternalSize")]
        public string ExternalSize { get; set; }
        [XmlElement(ElementName = "ExternalDevice")]
        public string ExternalDevice { get; set; }
        [XmlElement(ElementName = "ExternalAudioDevice")]
        public string ExternalAudioDevice { get; set; }
        [XmlElement(ElementName = "ExternalAudioDelay")]
        public string ExternalAudioDelay { get; set; }
        [XmlElement(ElementName = "Virtual")]
        public string Virtual { get; set; }
        [XmlElement(ElementName = "External")]
        public string External { get; set; }
        [XmlElement(ElementName = "ExternalPort")]
        public string ExternalPort { get; set; }
        [XmlElement(ElementName = "ExternalAudioChannel")]
        public string ExternalAudioChannel { get; set; }
        [XmlElement(ElementName = "ExternalAlphaChannel")]
        public string ExternalAlphaChannel { get; set; }
        [XmlElement(ElementName = "VirtualUseStreaming")]
        public string VirtualUseStreaming { get; set; }
        [XmlElement(ElementName = "ExternalUseDisplay")]
        public string ExternalUseDisplay { get; set; }
    }
}