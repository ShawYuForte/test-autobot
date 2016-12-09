using System.Xml.Serialization;
// ReSharper disable InconsistentNaming

namespace forte.devices.models.presets
{
    public enum VmixInputTypes
    {
        Video = 0,
        Image = 1,
    }

    /* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
    [XmlRoot(ElementName = "Input")]
    public class VmixPresetInput
    {
        public static VmixPresetInput CreateWithDefaults()
        {
            return new VmixPresetInput
            {
                Positions = VmixPreset.PositionsMagicString,
                Triggers = VmixPreset.TriggersMagicString,
                RangeStart = "0",
                RangeStop = "0",
                ShortcutMappings = "",
                Loop = "False",
                VolumeF = "1",
                VolumeF1 = "1",
                VolumeF2 = "1",
                BalanceF = "0",
                AspectRatio = "100",
                Category = "0",
                MouseClickAction = "0",
                Collapsed = "False",
                Solo = "False",
                HeadphonesVolumeF = "1",
                BusMaster = "True",
                TallyCOMPort = "None",
                TallyNumber = "0",
                AutoPause = "True",
                AutoRestart = "False",
                AutoPlay = "True",
                Mirror = "False",
                SelectedIndex = "0",
                Rate = "1",
                XML = "",
                ShaderSource = "00000000-0000-0000-0000-000000000000",
                PTZProvider = "",
                PTZConnection = "",
                PTZAutoConnect = "False",
                PTZDefaultPanTiltSpeed = "0.5",
                PTZDefaultZoomSpeed = "0.5",
                VideoShader_ColorCorrectionSourceEnabled = "0",
                VideoShader_White = "1",
                VideoShader_Black = "0",
                VideoShader_Red = "0",
                VideoShader_Green = "0",
                VideoShader_Blue = "0",
                VideoShader_Alpha = "1",
                VideoShader_Saturation = "1",
                VideoShader_ColorKey = "0",
                VideoShader_Deinterlace = "False",
                VideoShader_Sharpen = "False",
                VideoShader_ToleranceRed = "0",
                VideoShader_ToleranceGreen = "0",
                VideoShader_ToleranceBlue = "0",
                VideoShader_ToleranceGreenGap = "0",
                VideoShader_GreenFilter = "False",
                VideoShader_GreenFilterTransparencyThreshold = "1",
                VideoShader_LumaKeyThreshold = "0",
                VideoShader_AntiAliasing = "False",
                VideoShader_AntiAliasingFilter = "0",
                VideoShader_ClippingX1 = "0",
                VideoShader_ClippingX2 = "1",
                VideoShader_ClippingY1 = "0",
                VideoShader_ClippingY2 = "1",
                VideoShader_PremultipliedAlpha = "False"
            };
        }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "Position")]
        public string Position { get; set; }
        [XmlAttribute(AttributeName = "RangeStart")]
        public string RangeStart { get; set; }
        [XmlAttribute(AttributeName = "RangeStop")]
        public string RangeStop { get; set; }
        [XmlAttribute(AttributeName = "State")]
        public string State { get; set; }
        [XmlAttribute(AttributeName = "OriginalTitle")]
        public string OriginalTitle { get; set; }
        [XmlAttribute(AttributeName = "ShortcutMappings")]
        public string ShortcutMappings { get; set; }
        [XmlAttribute(AttributeName = "Key")]
        public string Key { get; set; }
        [XmlAttribute(AttributeName = "Loop")]
        public string Loop { get; set; }
        [XmlAttribute(AttributeName = "VolumeF")]
        public string VolumeF { get; set; }
        [XmlAttribute(AttributeName = "VolumeF1")]
        public string VolumeF1 { get; set; }
        [XmlAttribute(AttributeName = "VolumeF2")]
        public string VolumeF2 { get; set; }
        [XmlAttribute(AttributeName = "Muted")]
        public string Muted { get; set; }
        [XmlAttribute(AttributeName = "BalanceF")]
        public string BalanceF { get; set; }
        [XmlAttribute(AttributeName = "AspectRatio")]
        public string AspectRatio { get; set; }
        [XmlAttribute(AttributeName = "Category")]
        public string Category { get; set; }
        [XmlAttribute(AttributeName = "MouseClickAction")]
        public string MouseClickAction { get; set; }
        [XmlAttribute(AttributeName = "Collapsed")]
        public string Collapsed { get; set; }
        [XmlAttribute(AttributeName = "Solo")]
        public string Solo { get; set; }
        [XmlAttribute(AttributeName = "HeadphonesVolumeF")]
        public string HeadphonesVolumeF { get; set; }
        [XmlAttribute(AttributeName = "BusMaster")]
        public string BusMaster { get; set; }
        [XmlAttribute(AttributeName = "BusA")]
        public string BusA { get; set; }
        [XmlAttribute(AttributeName = "BusB")]
        public string BusB { get; set; }
        [XmlAttribute(AttributeName = "Positions")]
        public string Positions { get; set; }
        [XmlAttribute(AttributeName = "Triggers")]
        public string Triggers { get; set; }
        [XmlAttribute(AttributeName = "TallyCOMPort")]
        public string TallyCOMPort { get; set; }
        [XmlAttribute(AttributeName = "TallyNumber")]
        public string TallyNumber { get; set; }
        [XmlAttribute(AttributeName = "AutoAudioMixing")]
        public string AutoAudioMixing { get; set; }
        [XmlAttribute(AttributeName = "AutoPause")]
        public string AutoPause { get; set; }
        [XmlAttribute(AttributeName = "AutoRestart")]
        public string AutoRestart { get; set; }
        [XmlAttribute(AttributeName = "AutoPlay")]
        public string AutoPlay { get; set; }
        [XmlAttribute(AttributeName = "Mirror")]
        public string Mirror { get; set; }
        [XmlAttribute(AttributeName = "SelectedIndex")]
        public string SelectedIndex { get; set; }
        [XmlAttribute(AttributeName = "Rate")]
        public string Rate { get; set; }
        [XmlAttribute(AttributeName = "XML")]
        public string XML { get; set; }
        [XmlAttribute(AttributeName = "ShaderSource")]
        public string ShaderSource { get; set; }
        [XmlAttribute(AttributeName = "PTZProvider")]
        public string PTZProvider { get; set; }
        [XmlAttribute(AttributeName = "PTZConnection")]
        public string PTZConnection { get; set; }
        [XmlAttribute(AttributeName = "PTZAutoConnect")]
        public string PTZAutoConnect { get; set; }
        [XmlAttribute(AttributeName = "PTZDefaultPanTiltSpeed")]
        public string PTZDefaultPanTiltSpeed { get; set; }
        [XmlAttribute(AttributeName = "PTZDefaultZoomSpeed")]
        public string PTZDefaultZoomSpeed { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ColorCorrectionSourceEnabled")]
        public string VideoShader_ColorCorrectionSourceEnabled { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_White")]
        public string VideoShader_White { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Black")]
        public string VideoShader_Black { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Red")]
        public string VideoShader_Red { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Green")]
        public string VideoShader_Green { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Blue")]
        public string VideoShader_Blue { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Alpha")]
        public string VideoShader_Alpha { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Saturation")]
        public string VideoShader_Saturation { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ColorKey")]
        public string VideoShader_ColorKey { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Deinterlace")]
        public string VideoShader_Deinterlace { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Sharpen")]
        public string VideoShader_Sharpen { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ToleranceRed")]
        public string VideoShader_ToleranceRed { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ToleranceGreen")]
        public string VideoShader_ToleranceGreen { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ToleranceBlue")]
        public string VideoShader_ToleranceBlue { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ToleranceGreenGap")]
        public string VideoShader_ToleranceGreenGap { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_GreenFilter")]
        public string VideoShader_GreenFilter { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_GreenFilterTransparencyThreshold")]
        public string VideoShader_GreenFilterTransparencyThreshold { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_LumaKeyThreshold")]
        public string VideoShader_LumaKeyThreshold { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_AntiAliasing")]
        public string VideoShader_AntiAliasing { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_AntiAliasingFilter")]
        public string VideoShader_AntiAliasingFilter { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ClippingX1")]
        public string VideoShader_ClippingX1 { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ClippingX2")]
        public string VideoShader_ClippingX2 { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ClippingY1")]
        public string VideoShader_ClippingY1 { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_ClippingY2")]
        public string VideoShader_ClippingY2 { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_PremultipliedAlpha")]
        public string VideoShader_PremultipliedAlpha { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "MixerCollapsed")]
        public string MixerCollapsed { get; set; }
        [XmlAttribute(AttributeName = "AudioDelay")]
        public string AudioDelay { get; set; }
        [XmlAttribute(AttributeName = "AudioChannel")]
        public string AudioChannel { get; set; }
        [XmlAttribute(AttributeName = "AudioGain")]
        public string AudioGain { get; set; }
        [XmlAttribute(AttributeName = "AudioGain1")]
        public string AudioGain1 { get; set; }
        [XmlAttribute(AttributeName = "AudioGain2")]
        public string AudioGain2 { get; set; }
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
        [XmlAttribute(AttributeName = "LowLatency")]
        public string LowLatency { get; set; }
        [XmlAttribute(AttributeName = "Buffer")]
        public string Buffer { get; set; }
        [XmlAttribute(AttributeName = "StreamType")]
        public string StreamType { get; set; }
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "AudioPath")]
        public string AudioPath { get; set; }
        [XmlAttribute(AttributeName = "AudioStartChannel")]
        public string AudioStartChannel { get; set; }
        [XmlAttribute(AttributeName = "AudioChannelCount")]
        public string AudioChannelCount { get; set; }
    }
}

