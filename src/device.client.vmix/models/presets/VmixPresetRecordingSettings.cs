using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "RecordingSettings")]
    public class VmixPresetRecordingSettings
    {
        [XmlElement(ElementName = "Size")]
        public string Size { get; set; }
        [XmlElement(ElementName = "FrameRate")]
        public string FrameRate { get; set; }
        [XmlElement(ElementName = "FrameRateInterlaced")]
        public string FrameRateInterlaced { get; set; }
        [XmlElement(ElementName = "CodecDevice")]
        public string CodecDevice { get; set; }
        [XmlElement(ElementName = "HardwareAcceleration")]
        public string HardwareAcceleration { get; set; }
        [XmlElement(ElementName = "Audio")]
        public string Audio { get; set; }
        [XmlElement(ElementName = "AudioDelay")]
        public string AudioDelay { get; set; }
        [XmlElement(ElementName = "Filename")]
        public string Filename { get; set; }
        [XmlElement(ElementName = "Interval")]
        public string Interval { get; set; }
        [XmlElement(ElementName = "MPEGBitRate")]
        public string MPEGBitRate { get; set; }
        [XmlElement(ElementName = "VideoFileFormat")]
        public string VideoFileFormat { get; set; }
        [XmlElement(ElementName = "WMVAudioCodec")]
        public string WMVAudioCodec { get; set; }
        [XmlElement(ElementName = "WMVVideoCodec")]
        public string WMVVideoCodec { get; set; }
        [XmlElement(ElementName = "WMVPort")]
        public string WMVPort { get; set; }
        [XmlElement(ElementName = "WMVBitRate")]
        public string WMVBitRate { get; set; }
        [XmlElement(ElementName = "WMVQuality")]
        public string WMVQuality { get; set; }
        [XmlElement(ElementName = "WMVFPS")]
        public string WMVFPS { get; set; }
        [XmlElement(ElementName = "SelectedTab")]
        public string SelectedTab { get; set; }
        [XmlElement(ElementName = "AudioBitRate")]
        public string AudioBitRate { get; set; }
        [XmlElement(ElementName = "FFMPEGFormatXML")]
        public string FFMPEGFormatXML { get; set; }
        [XmlElement(ElementName = "MP4Format")]
        public string MP4Format { get; set; }
    }
}