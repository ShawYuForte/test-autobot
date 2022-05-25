using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "Shortcut")]
    public class VmixPresetShortcut
    {
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
        [XmlElement(ElementName = "GetInputByNumber")]
        public string GetInputByNumber { get; set; }
        [XmlElement(ElementName = "Disabled")]
        public string Disabled { get; set; }
        [XmlElement(ElementName = "ControllerHidden")]
        public string ControllerHidden { get; set; }
        [XmlElement(ElementName = "Triggers")]
        public string Triggers { get; set; }
        [XmlElement(ElementName = "Title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "Description1")]
        public string Description1 { get; set; }
        [XmlElement(ElementName = "Description2")]
        public string Description2 { get; set; }
        [XmlElement(ElementName = "SelectedIndex")]
        public string SelectedIndex { get; set; }
        [XmlElement(ElementName = "ExpandedKey")]
        public string ExpandedKey { get; set; }
        [XmlElement(ElementName = "Key")]
        public string Key { get; set; }
        [XmlElement(ElementName = "Function")]
        public string Function { get; set; }
        [XmlElement(ElementName = "Duration")]
        public string Duration { get; set; }
        [XmlElement(ElementName = "Input")]
        public VmixPresetShortcutInput Input { get; set; }
        [XmlElement(ElementName = "MIDIChannel")]
        public string MIDIChannel { get; set; }
        [XmlElement(ElementName = "MIDINote")]
        public string MIDINote { get; set; }
        [XmlElement(ElementName = "MIDIValue")]
        public string MIDIValue { get; set; }
        [XmlElement(ElementName = "MIDIValueEnabled")]
        public string MIDIValueEnabled { get; set; }
        [XmlElement(ElementName = "MIDIValueType")]
        public string MIDIValueType { get; set; }
        [XmlElement(ElementName = "JSIndex")]
        public string JSIndex { get; set; }
        [XmlElement(ElementName = "JSButton")]
        public string JSButton { get; set; }
        [XmlElement(ElementName = "DisplayType")]
        public string DisplayType { get; set; }
        [XmlElement(ElementName = "Mix")]
        public string Mix { get; set; }
        [XmlElement(ElementName = "JSPressureEnabled")]
        public string JSPressureEnabled { get; set; }
    }
}