#region

using System.Xml.Serialization;

#endregion

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "StreamDestination")]
    public class VmixStreamDestination
    {
        public VmixStreamDestination()
        {
        }

        public VmixStreamDestination(string name, string url)
        {
            Url = url;
            Stream = name;
            UserAgent = "FMLE/3.0";
            Provider = "Custom_RTMP_Server";
        }

        [XmlElement(ElementName = "Stream")]
        public string Stream { get; set; }

        [XmlElement(ElementName = "URL")]
        public string Url { get; set; }

        [XmlElement(ElementName = "BackupURL")]
        public string BackupUrl { get; set; }

        [XmlElement(ElementName = "Username")]
        public string Username { get; set; }

        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }

        [XmlElement(ElementName = "BackupUsername")]
        public string BackupUsername { get; set; }

        [XmlElement(ElementName = "BackupPassword")]
        public string BackupPassword { get; set; }

        [XmlElement(ElementName = "Filename")]
        public string Filename { get; set; }

        [XmlElement(ElementName = "FilenameEnabled")]
        public string FilenameEnabled { get; set; }

        [XmlElement(ElementName = "UserAgent")]
        public string UserAgent { get; set; }

        [XmlElement(ElementName = "CloudMediaGroup.username")]
        public string CloudMediaGroupUsername { get; set; }

        [XmlElement(ElementName = "CloudMediaGroup.xml")]
        public string CloudMediaGroupXml { get; set; }
        [XmlElement(ElementName = "CloudMediaGroup.channel")]
        public string CloudMediaGroupChannel { get; set; }
        [XmlElement(ElementName = "DaCast.username")]
        public string DaCastUsername { get; set; }
        [XmlElement(ElementName = "DaCast.xml")]
        public string DaCastXml { get; set; }
        [XmlElement(ElementName = "DaCast.channel")]
        public string DaCastChannel { get; set; }
        [XmlElement(ElementName = "Meridix.username")]
        public string MeridixUsername { get; set; }
        [XmlElement(ElementName = "Meridix.xml")]
        public string MeridixXml { get; set; }
        [XmlElement(ElementName = "Meridix.channel")]
        public string MeridixChannel { get; set; }
        [XmlElement(ElementName = "ScaleEngine.username")]
        public string ScaleEngineUsername { get; set; }
        [XmlElement(ElementName = "ScaleEngine.xml")]
        public string ScaleEngineXml { get; set; }
        [XmlElement(ElementName = "ScaleEngine.channel")]
        public string ScaleEngineChannel { get; set; }
        [XmlElement(ElementName = "StreamingChurch.tv.username")]
        public string StreamingChurchTvUsername { get; set; }
        [XmlElement(ElementName = "StreamingChurch.tv.xml")]
        public string StreamingChurchTvXml { get; set; }
        [XmlElement(ElementName = "StreamingChurch.tv.channel")]
        public string StreamingChurchTvChannel { get; set; }
        [XmlElement(ElementName = "StreamShark.io.username")]
        public string StreamSharkIoUsername { get; set; }
        [XmlElement(ElementName = "StreamShark.io.xml")]
        public string StreamSharkIoXml { get; set; }
        [XmlElement(ElementName = "StreamShark.io.channel")]
        public string StreamSharkIoChannel { get; set; }
        [XmlElement(ElementName = "StreamSpot.username")]
        public string StreamSpotUsername { get; set; }
        [XmlElement(ElementName = "StreamSpot.xml")]
        public string StreamSpotXml { get; set; }
        [XmlElement(ElementName = "StreamSpot.channel")]
        public string StreamSpotChannel { get; set; }
        [XmlElement(ElementName = "Sunday_Streams.username")]
        public string Sunday_StreamsUsername { get; set; }
        [XmlElement(ElementName = "Sunday_Streams.xml")]
        public string Sunday_StreamsXml { get; set; }
        [XmlElement(ElementName = "Sunday_Streams.channel")]
        public string Sunday_StreamsChannel { get; set; }
        [XmlElement(ElementName = "TikiLIVE.username")]
        public string TikiLIVEUsername { get; set; }
        [XmlElement(ElementName = "TikiLIVE.xml")]
        public string TikiLIVEXml { get; set; }
        [XmlElement(ElementName = "TikiLIVE.channel")]
        public string TikiLIVEChannel { get; set; }
        [XmlElement(ElementName = "Twitch.username")]
        public string TwitchUsername { get; set; }
        [XmlElement(ElementName = "Twitch.xml")]
        public string TwitchXml { get; set; }
        [XmlElement(ElementName = "Twitch.channel")]
        public string TwitchChannel { get; set; }
        [XmlElement(ElementName = "Ustream.username")]
        public string UstreamUsername { get; set; }
        [XmlElement(ElementName = "Ustream.xml")]
        public string UstreamXml { get; set; }
        [XmlElement(ElementName = "Ustream.channel")]
        public string UstreamChannel { get; set; }
        [XmlElement(ElementName = "YouTube_Live.username")]
        public string YouTube_LiveUsername { get; set; }
        [XmlElement(ElementName = "YouTube_Live.xml")]
        public string YouTube_LiveXml { get; set; }
        [XmlElement(ElementName = "YouTube_Live.channel")]
        public string YouTube_LiveChannel { get; set; }
        [XmlElement(ElementName = "Wowza_Cloud.username")]
        public string Wowza_CloudUsername { get; set; }
        [XmlElement(ElementName = "Wowza_Cloud.xml")]
        public string Wowza_CloudXml { get; set; }
        [XmlElement(ElementName = "Wowza_Cloud.channel")]
        public string Wowza_CloudChannel { get; set; }
        [XmlElement(ElementName = "Provider")]
        public string Provider { get; set; }
    }
}