using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "StreamingSetting")]
    public class VmixPresetStreamingSetting
    {
        [XmlElement(ElementName = "StreamingType")]
        public string StreamingType { get; set; }
        [XmlElement(ElementName = "AudioDataRate0")]
        public string AudioDataRate0 { get; set; }
        [XmlElement(ElementName = "AudioDataRate1")]
        public string AudioDataRate1 { get; set; }
        [XmlElement(ElementName = "AudioDataRate2")]
        public string AudioDataRate2 { get; set; }
        [XmlElement(ElementName = "AudioFormat")]
        public string AudioFormat { get; set; }
        [XmlElement(ElementName = "AutoAdjust")]
        public string AutoAdjust { get; set; }
        [XmlElement(ElementName = "EncodeWidth")]
        public string EncodeWidth { get; set; }
        [XmlElement(ElementName = "EncodeHeight")]
        public string EncodeHeight { get; set; }
        [XmlElement(ElementName = "EncodeWidth2")]
        public string EncodeWidth2 { get; set; }
        [XmlElement(ElementName = "EncodeHeight2")]
        public string EncodeHeight2 { get; set; }
        [XmlElement(ElementName = "EncodeWidth3")]
        public string EncodeWidth3 { get; set; }
        [XmlElement(ElementName = "EncodeHeight3")]
        public string EncodeHeight3 { get; set; }
        [XmlElement(ElementName = "VideoEnabled")]
        public string VideoEnabled { get; set; }
        [XmlElement(ElementName = "VideoEnabled2")]
        public string VideoEnabled2 { get; set; }
        [XmlElement(ElementName = "VideoEnabled3")]
        public string VideoEnabled3 { get; set; }
        [XmlElement(ElementName = "VideoDataRate")]
        public string VideoDataRate { get; set; }
        [XmlElement(ElementName = "VideoDataRate2")]
        public string VideoDataRate2 { get; set; }
        [XmlElement(ElementName = "VideoDataRate3")]
        public string VideoDataRate3 { get; set; }
        [XmlElement(ElementName = "KeyframeFrequency")]
        public string KeyframeFrequency { get; set; }
        [XmlElement(ElementName = "Level")]
        public string Level { get; set; }
        [XmlElement(ElementName = "Preset")]
        public string Preset { get; set; }
        [XmlElement(ElementName = "MinimumVideoBitRate")]
        public string MinimumVideoBitRate { get; set; }
        [XmlElement(ElementName = "Profile")]
        public string Profile { get; set; }
        [XmlElement(ElementName = "Threads")]
        public string Threads { get; set; }
        [XmlElement(ElementName = "NetworkBuffer")]
        public string NetworkBuffer { get; set; }
        [XmlElement(ElementName = "StrictCBR")]
        public string StrictCBR { get; set; }
        [XmlElement(ElementName = "Destination0")]
        public string Destination0 { get; set; }
        [XmlElement(ElementName = "Destination1")]
        public string Destination1 { get; set; }
        [XmlElement(ElementName = "Destination2")]
        public string Destination2 { get; set; }
        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }
        [XmlElement(ElementName = "Height")]
        public string Height { get; set; }
        [XmlElement(ElementName = "FrameRate")]
        public string FrameRate { get; set; }
        [XmlElement(ElementName = "HardwareEncoding")]
        public string HardwareEncoding { get; set; }
        [XmlElement(ElementName = "ExternalLink")]
        public string ExternalLink { get; set; }
        [XmlElement(ElementName = "KeyframeAligned")]
        public string KeyframeAligned { get; set; }
    }
}