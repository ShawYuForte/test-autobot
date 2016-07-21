using System.Xml.Serialization;

namespace forte.devices.models
{
    [XmlRoot(ElementName = "AudioMaster")]
    public class VmixPresetAudioMaster
    {
        [XmlAttribute(AttributeName = "AudioDelay")]
        public string AudioDelay { get; set; }
        [XmlAttribute(AttributeName = "AudioChannel")]
        public string AudioChannel { get; set; }
        [XmlAttribute(AttributeName = "AudioGain")]
        public string AudioGain { get; set; }
        [XmlAttribute(AttributeName = "AudioCompressorEnabled")]
        public string AudioCompressorEnabled { get; set; }
        [XmlAttribute(AttributeName = "AudioCompressorRatio")]
        public string AudioCompressorRatio { get; set; }
        [XmlAttribute(AttributeName = "AudioCompressorThreshold")]
        public string AudioCompressorThreshold { get; set; }
        [XmlAttribute(AttributeName = "AudioNoiseGateEnabled")]
        public string AudioNoiseGateEnabled { get; set; }
        [XmlAttribute(AttributeName = "AudioNoiseGateThreshold")]
        public string AudioNoiseGateThreshold { get; set; }
        [XmlAttribute(AttributeName = "AudioEQEnabled")]
        public string AudioEQEnabled { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB0")]
        public string AudioEQGainDB0 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB1")]
        public string AudioEQGainDB1 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB2")]
        public string AudioEQGainDB2 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB3")]
        public string AudioEQGainDB3 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB4")]
        public string AudioEQGainDB4 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB5")]
        public string AudioEQGainDB5 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB6")]
        public string AudioEQGainDB6 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB7")]
        public string AudioEQGainDB7 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB8")]
        public string AudioEQGainDB8 { get; set; }
        [XmlAttribute(AttributeName = "AudioEQGainDB9")]
        public string AudioEQGainDB9 { get; set; }
        [XmlAttribute(AttributeName = "AudioAGCEnabled")]
        public string AudioAGCEnabled { get; set; }
        [XmlAttribute(AttributeName = "Volume")]
        public string Volume { get; set; }
        [XmlAttribute(AttributeName = "HeadphonesVolume")]
        public string HeadphonesVolume { get; set; }
        [XmlAttribute(AttributeName = "Muted")]
        public string Muted { get; set; }
    }
}