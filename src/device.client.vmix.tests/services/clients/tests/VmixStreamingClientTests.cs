#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using forte.devices.extensions;
using forte.devices.models;
using forte.devices.models.presets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.XmlDiffPatch;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

#endregion

namespace forte.devices.services.clients.tests
{
    [TestClass]
    public class VmixStreamingClientTests
    {
        private List<string> _tempFiles;

        [TestInitialize]
        public void Setup()
        {
            _tempFiles = new List<string>();
        }

        [TestCleanup]
        public void TearDown()
        {
            foreach (var tempFile in _tempFiles)
            {
                if (File.Exists(tempFile))
                {
                    try
                    {
                        File.Delete(tempFile);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void TestLoadingPresetFromVideoStream()
        {
            var config = new TestStreamingDeviceConfig();
            config.Set("VmixPresetTemplateFilePath", @"C:\forte\preset\Forte Preset.vmix");
            config.Set("VmixExecutablePath", @"D:\Program Files (x86)\vMix\vMix.exe");
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var configManager = fixture.Freeze<Mock<IConfigurationManager>>();
            configManager.Setup(mgr => mgr.GetDeviceConfig())
                .Returns(config);

            var sut = fixture.Create<VmixStreamingClient>();
            var result = sut.LoadPreset(new VideoStreamModel
            {
                PrimaryIngestUrl = "primary",
                SecondaryIngestUrl = "secondary"
            });

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
            sut.StopVmix();
        }

        [TestMethod]
        [Ignore]
        public void TestLoadingPresetWithShortTimeout()
        {
            var config = new TestStreamingDeviceConfig();
            config.Set("VmixPresetTemplateFilePath", @"C:\forte\preset\Forte Preset.vmix");
            config.Set("VmixExecutablePath", @"D:\Program Files (x86)\vMix\vMix.exe");
            var timeout = 1;
            config.Set("VmixLoadTimeout", timeout);
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var configManager = fixture.Freeze<Mock<IConfigurationManager>>();
            configManager.Setup(mgr => mgr.GetDeviceConfig())
                .Returns(config);

            var sut = fixture.Create<VmixStreamingClient>();
            try
            {
                sut.LoadPreset(new VideoStreamModel
                {
                    PrimaryIngestUrl = "primary",
                    SecondaryIngestUrl = "secondary"
                });
                Assert.Fail("Expected timeout exception");
            }
            catch (Exception exception)
            {
                Assert.AreEqual($"Preset load timed out, could not load within {timeout} seconds.", exception.Message);
            }

            sut.StopVmix();
        }

        [TestMethod]
        [DeploymentItem("data", "data")]
        //[Ignore]
        public void TestPresetGeneration()
        {
            foreach (var file in Directory.GetFiles("data\\presets"))
            {
                TestPresetGeneration(file);
            }
            //TestPresetGeneration(@"D:\dev\active\forte\iot\src\device.client.vmix.tests\data\presets\Forte Preset AHPC.vmix");
        }

        private void TestPresetGeneration(string presetFile)
        {
            // TODO
            // 1. remove the Destination encoded Xml
            // 2. compare rest of the xml
            // 3. decode destination xml from #1 and compare 

            var input = Path.GetTempFileName();
            var output = Path.GetTempFileName();

            File.Copy(presetFile, input, true);
            var preset = VmixPreset.FromFile(input);
            Assert.IsNotNull(preset, "Could not load preset from file {0}", input);
            preset.ToFile(output);

            _tempFiles.Add(input);
            _tempFiles.Add(output);

            string message;
            string patchOutputFile;

            var inputDestinations = ExtractDestinations(input);
            var outputDestinations = ExtractDestinations(output);

            if (inputDestinations == null || inputDestinations.Count != outputDestinations?.Count)
                Assert.Fail("Destinations for file {0} do not match, input has {1}, output has {2}", presetFile, inputDestinations?.Count,  outputDestinations?.Count);

            var inputBuffer = new StringBuilder();
            var outputBuffer = new StringBuilder();

            inputBuffer.AppendLine("<Destinations>");
            outputBuffer.AppendLine("<Destinations>");
            for (var index = 0; index < inputDestinations.Count; index++)
            {
                inputBuffer.AppendLine("<Destination>");
                inputBuffer.AppendLine(WebUtility.HtmlDecode(inputDestinations[index]));
                inputBuffer.AppendLine("</Destination>");

                outputBuffer.AppendLine("<Destination>");
                outputBuffer.AppendLine(WebUtility.HtmlDecode(outputDestinations[index]));
                outputBuffer.AppendLine("</Destination>");
            }
            inputBuffer.AppendLine("</Destinations>");
            outputBuffer.AppendLine("</Destinations>");

            var identical = AreXmlFilesIdentical(input, output, out message, out patchOutputFile);
            Assert.IsTrue(identical, "Did not regenerate identical Xml for '{0}', message: {1}", presetFile, message);

            var inputDestinationFile = Path.GetTempFileName();
            File.WriteAllText(inputDestinationFile, inputBuffer.ToString());
            _tempFiles.Add(inputDestinationFile);

            var outputDestinationFile = Path.GetTempFileName();
            File.WriteAllText(outputDestinationFile, inputBuffer.ToString());
            _tempFiles.Add(outputDestinationFile);

            identical = AreXmlFilesIdentical(inputDestinationFile, outputDestinationFile, out message, out patchOutputFile);
            Assert.IsTrue(identical, "Did not regenerate identical Destinations Xml for '{0}', message: {1}", presetFile, message);
        }

        private List<string> ExtractDestinations(string file)
        {
            var result = new List<string>();
            const string fullTagRegEx = @"<Destination(0|1|2)[\sa-zA-Z:=\-""\/\.0-9]*>[\s\S]*?<\/?Destination(0|1|2)[\sa-zA-Z:=\-""\/\.0-9]*>";
            var fileContent = File.ReadAllText(file);
            var match = Regex.Match(fileContent, fullTagRegEx);

            while (match.Success)
            {
                result.Add(match.Value.RemoveXmlAttribute("StreamDestination"));
                fileContent = fileContent.Replace(match.Value, string.Empty);
                match = match.NextMatch();
            }

            File.WriteAllText(file, fileContent);

            return result;
        }

        private bool AreXmlFilesIdentical(string file1, string file2, out string message, out string patchOutputFile)
        {
            patchOutputFile = null;

            var xmldiff = new XmlDiff(options: XmlDiffOptions.IgnoreNamespaces | XmlDiffOptions.IgnoreDtd |
                    XmlDiffOptions.IgnoreChildOrder | XmlDiffOptions.IgnorePrefixes | XmlDiffOptions.IgnoreWhitespace);

            bool identical;

            using (var sw = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(sw))
                {
                    identical = xmldiff.Compare(file1, file2, false, xmlWriter);
                }
                message = sw.ToString();
            }

            if (identical) return true;

            var presetDoc = new XmlDocument();
            presetDoc.Load(file1);
            var xmlPatch = new XmlPatch();
            var xmlReader = new XmlTextReader(new StringReader(message));
            xmlPatch.Patch(presetDoc, xmlReader);
            patchOutputFile = Path.GetTempFileName();
            using (var xmlTextWriter = XmlWriter.Create(patchOutputFile))
            {
                presetDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
            }
            Debug.WriteLine("ERROR: Files not identical");
            Debug.WriteLine($"ERROR: Original: {file1}");
            Debug.WriteLine($"ERROR: Patched: {patchOutputFile}");

            return false;
        }

        [TestMethod]
        [Ignore]
        public void UtilityTest()
        {
            var presetFile = @"C:\dev\forte\iot\src\device.client.vmix.tests\services\clients\tests\data\preset1.vmix";
            var preset = VmixPreset.FromFile(presetFile);

            var sameProperties = new Dictionary<string, object>();
            var properties = typeof(VmixPresetInput).GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.Name == "Positions" || propertyInfo.Name == "Triggers") continue;
                var same = true;
                object previousValue = null;
                var beginning = true;

                foreach (var input in preset.Inputs)
                {
                    var currentValue = propertyInfo.GetValue(input);
                    if (!beginning && !Equals(currentValue, previousValue))
                        same = false;
                    beginning = false;
                    previousValue = currentValue;
                    if (!same) break;
                }

                if (same) sameProperties.Add(propertyInfo.Name, previousValue);
            }

            var buffer = new StringBuilder();
            foreach (var property in sameProperties)
                buffer.AppendLine(property.Value is string
                    ? $"{property.Key}=\"{property.Value}\","
                    : $"{property.Key}={property.Value},");
            var code = buffer.ToString();
            Console.WriteLine(code);
        }
    }
}