using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "XML")]
    public class VmixPreset
    {
        public const string PositionsMagicString =
            "&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-16&quot;?&gt;&#xD;&#xA;&lt;ArrayOfMatrixPosition xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;&lt;/ArrayOfMatrixPosition&gt;";
        public const string TriggersMagicString = "&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-16&quot;?&gt;&#xD;&#xA;&lt;ArrayOfInputTrigger xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot; /&gt;";

        public static VmixPreset FromFile(string filePath)
        {
            VmixPreset preset;
            var serializer = new XmlSerializer(typeof(VmixPreset));

            using (var reader = new StreamReader(filePath))
            {
                preset = (VmixPreset)serializer.Deserialize(reader);
                reader.Close();
            }
            return preset;
        }

        public void ToFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(VmixPreset));
            foreach (var input in Inputs)
            {
                input.Positions = "{{POSITIONS}}";
                input.Triggers = "{{TRIGGERS}}";
                if (string.IsNullOrWhiteSpace(input.Text)) continue;
                input.Text = input.Text.Replace(Environment.NewLine, string.Empty).Trim();
            }
            State.PlayLists = "";
            var tempFilePath = Path.GetTempFileName();

            using (var stream = File.Create(tempFilePath))
            using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                //NewLineHandling = NewLineHandling.Replace,
                //NewLineChars = string.Empty,
                //NewLineOnAttributes = false,
                
            }))
            {
                serializer.Serialize(xmlWriter, this);
                //stream.Close();
            }

            var fileContents = File.OpenText(tempFilePath).ReadToEnd();
            fileContents = fileContents.Replace("{{POSITIONS}}", PositionsMagicString);
            fileContents = fileContents.Replace("{{TRIGGERS}}", TriggersMagicString);
            File.WriteAllText(filePath, fileContents);
        }

        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "Input")]
        public List<VmixPresetInput> Inputs { get; set; }
        [XmlElement(ElementName = "State")]
        public VmixPresetState State { get; set; }
        [XmlElement(ElementName = "RecordingSettings")]
        public VmixPresetRecordingSettings RecordingSettings { get; set; }
        [XmlElement(ElementName = "OverlaySettings")]
        public string OverlaySettings { get; set; }
        [XmlElement(ElementName = "Shortcuts")]
        public VmixPresetShortcuts Shortcuts { get; set; }
        [XmlElement(ElementName = "AudioMaster")]
        public VmixPresetAudioMaster AudioMaster { get; set; }
        [XmlElement(ElementName = "CategorySettings")]
        public string CategorySettings { get; set; }
        [XmlElement(ElementName = "StreamingSettings")]
        public VmixPresetStreamingSettings StreamingSettings { get; set; }
        [XmlElement(ElementName = "OutputFormat")]
        public VmixPresetOutputFormat OutputFormat { get; set; }
    }
}