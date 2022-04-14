using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "OutputFormat")]
    public class VmixPresetOutputFormat
    {
        [XmlElement(ElementName = "OutputsExternal")]
        public VmixPresetOutputsExternal OutputsExternal { get; set; }
        [XmlElement(ElementName = "OutputsExternal2")]
        public VmixPresetOutputsExternal OutputsExternal2 { get; set; }
        [XmlElement(ElementName = "OutputsExternal3")]
        public VmixPresetOutputsExternal OutputsExternal3 { get; set; }
        [XmlElement(ElementName = "OutputsExternal4")]
        public VmixPresetOutputsExternal OutputsExternal4 { get; set; }
        [XmlElement(ElementName = "OutputsFullscreen")]
        public VmixPresetOutputsFullscreen OutputsFullscreen { get; set; }
        [XmlElement(ElementName = "OutputsFullscreen2")]
        public VmixPresetOutputsFullscreen OutputsFullscreen2 { get; set; }
        [XmlAttribute(AttributeName = "OutputSize")]
        public string OutputSize { get; set; }
        [XmlAttribute(AttributeName = "OutputFrameRate")]
        public string OutputFrameRate { get; set; }
        [XmlAttribute(AttributeName = "OutputFrameRateInterlaced")]
        public string OutputFrameRateInterlaced { get; set; }
        [XmlAttribute(AttributeName = "FullscreenEnabled")]
        public string FullscreenEnabled { get; set; }
    }
}