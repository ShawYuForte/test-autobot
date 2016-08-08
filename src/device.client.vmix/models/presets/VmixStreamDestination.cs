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
        //public string CloudMediaGroup.username { get; set; }
        //[XmlElement(ElementName = "CloudMediaGroup.xml")]
        //public string CloudMediaGroup.xml { get; set; }
        //[XmlElement(ElementName = "CloudMediaGroup.channel")]
        //public string CloudMediaGroup.channel { get; set; }
        //[XmlElement(ElementName = "DaCast.username")]
        //public string DaCast.username { get; set; }
        //[XmlElement(ElementName = "DaCast.xml")]
        //public string DaCast.xml { get; set; }
        //[XmlElement(ElementName = "DaCast.channel")]
        //public string DaCast.channel { get; set; }
        //[XmlElement(ElementName = "Meridix.username")]
        //public string Meridix.username { get; set; }
        //[XmlElement(ElementName = "Meridix.xml")]
        //public string Meridix.xml { get; set; }
        //[XmlElement(ElementName = "Meridix.channel")]
        //public string Meridix.channel { get; set; }
        //[XmlElement(ElementName = "ScaleEngine.username")]
        //public string ScaleEngine.username { get; set; }
        //[XmlElement(ElementName = "ScaleEngine.xml")]
        //public string ScaleEngine.xml { get; set; }
        //[XmlElement(ElementName = "ScaleEngine.channel")]
        //public string ScaleEngine.channel { get; set; }
        //[XmlElement(ElementName = "StreamingChurch.tv.username")]
        //public string StreamingChurch.tv.username { get; set; }
        //[XmlElement(ElementName = "StreamingChurch.tv.xml")]
        //public string StreamingChurch.tv.xml { get; set; }
        //[XmlElement(ElementName = "StreamingChurch.tv.channel")]
        //public string StreamingChurch.tv.channel { get; set; }
        //[XmlElement(ElementName = "StreamShark.io.username")]
        //public string StreamShark.io.username { get; set; }
        //[XmlElement(ElementName = "StreamShark.io.xml")]
        //public string StreamShark.io.xml { get; set; }
        //[XmlElement(ElementName = "StreamShark.io.channel")]
        //public string StreamShark.io.channel { get; set; }
        //[XmlElement(ElementName = "StreamSpot.username")]
        //public string StreamSpot.username { get; set; }
        //[XmlElement(ElementName = "StreamSpot.xml")]
        //public string StreamSpot.xml { get; set; }
        //[XmlElement(ElementName = "StreamSpot.channel")]
        //public string StreamSpot.channel { get; set; }
        //[XmlElement(ElementName = "Sunday_Streams.username")]
        //public string Sunday_Streams.username { get; set; }
        //[XmlElement(ElementName = "Sunday_Streams.xml")]
        //public string Sunday_Streams.xml { get; set; }
        //[XmlElement(ElementName = "Sunday_Streams.channel")]
        //public string Sunday_Streams.channel { get; set; }
        //[XmlElement(ElementName = "TikiLIVE.username")]
        //public string TikiLIVE.username { get; set; }
        //[XmlElement(ElementName = "TikiLIVE.xml")]
        //public string TikiLIVE.xml { get; set; }
        //[XmlElement(ElementName = "TikiLIVE.channel")]
        //public string TikiLIVE.channel { get; set; }
        //[XmlElement(ElementName = "Twitch.username")]
        //public string Twitch.username { get; set; }
        //[XmlElement(ElementName = "Twitch.xml")]
        //public string Twitch.xml { get; set; }
        //[XmlElement(ElementName = "Twitch.channel")]
        //public string Twitch.channel { get; set; }
        //[XmlElement(ElementName = "Ustream.username")]
        //public string Ustream.username { get; set; }
        //[XmlElement(ElementName = "Ustream.xml")]
        //public string Ustream.xml { get; set; }
        //[XmlElement(ElementName = "Ustream.channel")]
        //public string Ustream.channel { get; set; }
        //[XmlElement(ElementName = "YouTube_Live.username")]
        //public string YouTube_Live.username { get; set; }
        //[XmlElement(ElementName = "YouTube_Live.xml")]
        //public string YouTube_Live.xml { get; set; }
        //[XmlElement(ElementName = "YouTube_Live.channel")]
        //public string YouTube_Live.channel { get; set; }
        //[XmlElement(ElementName = "Wowza_Cloud.username")]
        //public string Wowza_Cloud.username { get; set; }
        //[XmlElement(ElementName = "Wowza_Cloud.xml")]
        //public string Wowza_Cloud.xml { get; set; }
        //[XmlElement(ElementName = "Wowza_Cloud.channel")]
        //public string Wowza_Cloud.channel { get; set; }
        //[XmlElement(ElementName = "Provider")]
        public string Provider { get; set; }
    }
}