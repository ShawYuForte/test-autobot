using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "MultiViewSettings")]
    public class VmixMultiViewSettings
    {
        [XmlElement(ElementName = "TitleLocation")]
        public string TitleLocation { get; set; }
        [XmlElement(ElementName = "TitleHeadings")]
        public string TitleHeadings { get; set; }
        [XmlElement(ElementName = "InputMapping0")]
        public string InputMapping0 { get; set; }
        [XmlElement(ElementName = "InputMapping1")]
        public string InputMapping1 { get; set; }
        [XmlElement(ElementName = "InputMapping2")]
        public string InputMapping2 { get; set; }
        [XmlElement(ElementName = "InputMapping3")]
        public string InputMapping3 { get; set; }
        [XmlElement(ElementName = "InputMapping4")]
        public string InputMapping4 { get; set; }
        [XmlElement(ElementName = "InputMapping5")]
        public string InputMapping5 { get; set; }
        [XmlElement(ElementName = "InputMapping6")]
        public string InputMapping6 { get; set; }
        [XmlElement(ElementName = "InputMapping7")]
        public string InputMapping7 { get; set; }
        [XmlElement(ElementName = "InputMapping8")]
        public string InputMapping8 { get; set; }
        [XmlElement(ElementName = "InputMapping9")]
        public string InputMapping9 { get; set; }
        [XmlElement(ElementName = "InputMapping10")]
        public string InputMapping10 { get; set; }
        [XmlElement(ElementName = "InputMapping11")]
        public string InputMapping11 { get; set; }
        [XmlElement(ElementName = "InputMapping12")]
        public string InputMapping12 { get; set; }
        [XmlElement(ElementName = "InputMapping13")]
        public string InputMapping13 { get; set; }
    }
}
