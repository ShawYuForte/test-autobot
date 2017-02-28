using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using forte.devices.infrastructure;
using RestSharp.Extensions.MonoHttp;

namespace forte.devices.models.presets
{
    [XmlRoot(ElementName = "XML")]
    public class VmixPreset
    {
        public const string PositionsMagicString =
            "&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-16&quot;?&gt;&#xD;&#xA;&lt;ArrayOfMatrixPosition xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;  &lt;MatrixPosition&gt;&#xD;&#xA;    &lt;MultiplyPosition&gt;&#xD;&#xA;      &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;      &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;      &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;      &lt;RotateOrigin&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/RotateOrigin&gt;&#xD;&#xA;      &lt;Rotate&gt;&#xD;&#xA;        &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;        &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;        &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;      &lt;/Rotate&gt;&#xD;&#xA;      &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;      &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;    &lt;/MultiplyPosition&gt;&#xD;&#xA;    &lt;Mirror&gt;false&lt;/Mirror&gt;&#xD;&#xA;    &lt;ZoomX&gt;1&lt;/ZoomX&gt;&#xD;&#xA;    &lt;ZoomY&gt;1&lt;/ZoomY&gt;&#xD;&#xA;    &lt;RotateOrigin&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/RotateOrigin&gt;&#xD;&#xA;    &lt;Rotate&gt;&#xD;&#xA;      &lt;X&gt;0&lt;/X&gt;&#xD;&#xA;      &lt;Y&gt;0&lt;/Y&gt;&#xD;&#xA;      &lt;Z&gt;0&lt;/Z&gt;&#xD;&#xA;    &lt;/Rotate&gt;&#xD;&#xA;    &lt;PanX&gt;0&lt;/PanX&gt;&#xD;&#xA;    &lt;PanY&gt;0&lt;/PanY&gt;&#xD;&#xA;  &lt;/MatrixPosition&gt;&#xD;&#xA;&lt;/ArrayOfMatrixPosition&gt;";
        public const string TriggersMagicString = "&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-16&quot;?&gt;&#xD;&#xA;&lt;ArrayOfInputTrigger xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot; /&gt;";

        private readonly XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = false,
            NamespaceHandling = NamespaceHandling.OmitDuplicates
            //NewLineHandling = NewLineHandling.Replace,
            //NewLineChars = string.Empty,
            //NewLineOnAttributes = false,

        };

        public static VmixPreset FromFile(string filePath)
        {
            VmixPreset preset;
            var serializer = new XmlSerializer(typeof(VmixPreset));

            using (var reader = new StreamReader(filePath))
            {
                preset = (VmixPreset)serializer.Deserialize(reader);
                reader.Close();
            }
            return ExtractDestinations(preset);
        }

        private static VmixPreset ExtractDestinations(VmixPreset preset)
        {
            var serializer = new XmlSerializer(typeof(VmixStreamDestination));
            preset.Outputs = new List<VmixStreamDestination>();
            ExtractDestination(preset, preset.StreamingSettings.StreamingSetting.Destination0, serializer);
            ExtractDestination(preset, preset.StreamingSettings.StreamingSetting.Destination1, serializer);
            ExtractDestination(preset, preset.StreamingSettings.StreamingSetting.Destination2, serializer);
            return preset;
        }

        private static void ExtractDestination(VmixPreset preset, string destination, XmlSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(destination)) return;
            // decode embedded xml
            destination = HttpUtility.HtmlDecode(destination);
            destination = $"<StreamDestination>{destination}</StreamDestination>";
            using (var reader = new StringReader(destination))
            {
                var vmixStreamDestination = (VmixStreamDestination)serializer.Deserialize(reader);
                preset.Outputs.Add(vmixStreamDestination);
                reader.Close();
            }
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
            ImportDestinations();
            //State.PlayLists = "";
            var tempFilePath = Path.GetTempFileName();

            using (var stream = File.Create(tempFilePath))
            {
                using (var xmlWriter = XmlWriter.Create(stream, _xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, this);
                }
            }

            var fileContents = File.OpenText(tempFilePath).ReadToEnd();
            fileContents = fileContents.Replace("{{POSITIONS}}", PositionsMagicString);
            fileContents = fileContents.Replace("{{TRIGGERS}}", TriggersMagicString);
            File.WriteAllText(filePath, fileContents);
        }

        private void ImportDestinations()
        {
            var serializer = new XmlSerializer(typeof(VmixStreamDestination));
            string destination;

            if (Outputs.Count > 0)
            {
                ImportDestination(serializer, out destination, Outputs[0]);
                StreamingSettings.StreamingSetting.Destination0 = destination;
            }
            if (Outputs.Count > 1)
            {
                ImportDestination(serializer, out destination, Outputs[1]);
                StreamingSettings.StreamingSetting.Destination1 = destination;
            }
            if (Outputs.Count > 2)
            {
                ImportDestination(serializer, out destination, Outputs[2]);
                StreamingSettings.StreamingSetting.Destination2 = destination;
            }
        }

        private void ImportDestination(XmlSerializer serializer, out string destination, VmixStreamDestination vmixStreamDestination)
        {
            var stringBuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(stringBuilder))
            {
                using (var xmlWriter = new VmixXmlWriter(XmlWriter.Create(writer, _xmlWriterSettings)))
                {
                    serializer.Serialize(xmlWriter, vmixStreamDestination);
                }
            }

            const string tagRegEx = @"<\/?StreamDestination[\sa-zA-Z:=\-""\/\.0-9]*>";
            var serialized = stringBuilder.ToString();

            var match = Regex.Match(serialized, tagRegEx);

            while (match.Success)
            {
                serialized = serialized.Replace(match.Value, string.Empty);
                match = match.NextMatch();
            }

            //serialized = serialized.Replace("<StreamDestination>", string.Empty)
            //    .Replace("</StreamDestination>", string.Empty);
            destination = serialized; // HttpUtility.HtmlEncode(serialized);
        }

        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "Input")]
        public List<VmixPresetInput> Inputs { get; set; }

        /// <summary>
        /// Stream outputs, the stream destinations (one is required, others for redundancy)
        /// </summary>
        [XmlIgnore]
        public List<VmixStreamDestination> Outputs { get; set; }

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

        [XmlElement(ElementName = "DataSources")]
        public VmixPresetDataSources DataSources { get; set; }
        [XmlElement(ElementName = "Activators")]
        public VmixPresetActivators Activators { get; set; }
    }
}