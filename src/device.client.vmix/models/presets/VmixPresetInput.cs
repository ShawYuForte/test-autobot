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
                VideoShader_PremultipliedAlpha = "False",
                FrameDelay = "0",
                PTZDefaultFocusSpeed = "0.5",
                PTZDefaultFocusEnabled = "False",
                PTZAlwaysShowThumbnail = "False",
                VideoShader_CCLiftR = "0",
                VideoShader_CCLiftG = "0",
                VideoShader_CCLiftB = "0",
                VideoShader_CCGammaR = "1",
                VideoShader_CCGammaG = "1",
                VideoShader_CCGammaB = "1",
                VideoShader_CCGainR = "1",
                VideoShader_CCGainG = "1",
                VideoShader_CCGainB = "1",
                VideoShader_Saturation2 = "0",
                VideoShader_Hue = "0",
                VideoShader_Rec601Fix = "False",
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
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }
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

        [XmlAttribute(AttributeName = "MixChannel1")]
        public string MixChannel1 { get; set; }

        [XmlAttribute(AttributeName = "MixChannel2")]
        public string MixChannel2 { get; set; }

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
        [XmlAttribute(AttributeName = "FrameDelay")]
        public string FrameDelay { get; set; }
        [XmlAttribute(AttributeName = "Positions")]
        public string Positions { get; set; }
        [XmlAttribute(AttributeName = "PositionsExtended")]
        public string PositionsExtended { get; set; }
        [XmlAttribute(AttributeName = "VirtualPTZPosition")]
        public string VirtualPTZPosition { get; set; }
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
        [XmlAttribute(AttributeName = "MasterOverlay0")]
        public string MasterOverlay0 { get; set; }
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

        [XmlAttribute(AttributeName = "PTZDefaultPositionSpeed")]
        public string PTZDefaultPositionSpeed { get; set; }

        [XmlAttribute(AttributeName = "PTZDefaultFocusSpeed")]
        public string PTZDefaultFocusSpeed { get; set; }
        [XmlAttribute(AttributeName = "PTZDefaultFocusEnabled")]
        public string PTZDefaultFocusEnabled { get; set; }
        [XmlAttribute(AttributeName = "PTZAlwaysShowThumbnail")]
        public string PTZAlwaysShowThumbnail { get; set; }
        [XmlAttribute(AttributeName = "CapturePath")]
        public string CapturePath { get; set; }
        [XmlAttribute(AttributeName = "CaptureVMR")]
        public string CaptureVMR { get; set; }
        [XmlAttribute(AttributeName = "Width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "Height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "Input")]
        public string Input { get; set; }
        [XmlAttribute(AttributeName = "CaptureAudioInput")]
        public string CaptureAudioInput { get; set; }
        [XmlAttribute(AttributeName = "FrameRate")]
        public string FrameRate { get; set; }
        [XmlAttribute(AttributeName = "FrameRateInterlaced")]
        public string FrameRateInterlaced { get; set; }
        [XmlAttribute(AttributeName = "CaptureAudioEnabled")]
        public string CaptureAudioEnabled { get; set; }
        [XmlAttribute(AttributeName = "CaptureInterlaced")]
        public string CaptureInterlaced { get; set; }
        [XmlAttribute(AttributeName = "VideoFormat")]
        public string VideoFormat { get; set; }
        [XmlAttribute(AttributeName = "VirtualInputKey")]
        public string VirtualInputKey { get; set; }
        [XmlAttribute(AttributeName = "PTZPosition")]
        public string PTZPosition { get; set; }
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
        [XmlAttribute(AttributeName = "VideoShader_CCLiftR")]
        public string VideoShader_CCLiftR { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCLiftG")]
        public string VideoShader_CCLiftG { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCLiftB")]
        public string VideoShader_CCLiftB { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCGammaR")]
        public string VideoShader_CCGammaR { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCGammaG")]
        public string VideoShader_CCGammaG { get; set; }

        [XmlAttribute(AttributeName = "VideoShader_CCGammaB")]
        public string VideoShader_CCGammaB { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCGainR")]
        public string VideoShader_CCGainR { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCGainG")]
        public string VideoShader_CCGainG { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_CCGainB")]
        public string VideoShader_CCGainB { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Saturation2")]
        public string VideoShader_Saturation2 { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Hue")]
        public string VideoShader_Hue { get; set; }
        [XmlAttribute(AttributeName = "VideoShader_Rec601Fix")]
        public string VideoShader_Rec601Fix { get; set; }

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
        [XmlAttribute(AttributeName = "AudioPath")]
        public string AudioPath { get; set; }
        [XmlAttribute(AttributeName = "AudioStartChannel")]
        public string AudioStartChannel { get; set; }
        [XmlAttribute(AttributeName = "AudioChannelCount")]
        public string AudioChannelCount { get; set; }
        [XmlAttribute(AttributeName = "AudioPad")]
        public string AudioPad { get; set; }
        [XmlAttribute(AttributeName = "AudioRackXML")]
        public string AudioRackXML { get; set; }
        [XmlAttribute(AttributeName = "DataSourcesXML")]
        public string DataSourcesXML { get; set; }
        [XmlAttribute(AttributeName = "NDISettingsXML")]
        public string NDISettingsXML { get; set; }
        [XmlAttribute(AttributeName = "NDISourceName")]
        public string NDISourceName { get; set; }
        [XmlAttribute(AttributeName = "NDISourceIP")]
        public string NDISourceIP { get; set; }
        [XmlAttribute(AttributeName = "NDILowBandwidth")]
        public string NDILowBandwidth { get; set; }
        [XmlAttribute(AttributeName = "NDIAudioOnly")]
        public string NDIAudioOnly { get; set; }
        [XmlAttribute(AttributeName = "NDIPSF")]
        public string NDIPSF { get; set; }
        [XmlAttribute(AttributeName = "NDIIncreaseBuffer")]
        public string NDIIncreaseBuffer { get; set; }
    }
}

