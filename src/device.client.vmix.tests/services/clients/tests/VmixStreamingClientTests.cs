#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
            //foreach (var file in Directory.GetFiles("data\\presets"))
            //{
            //    TestPresetGeneration(file);
            //}
            TestPresetGeneration(@"D:\dev\active\forte\iot\src\device.client.vmix.tests\data\presets\Forte Preset AHPC.vmix");
        }

        private void TestPresetGeneration(string presetFile)
        {
            // TODO
            // 1. remove the Destination encoded Xml
            // 2. compare rest of the xml
            // 3. decode destination xml from #1 and compare 

            var preset = VmixPreset.FromFile(presetFile);
            Assert.IsNotNull(preset, "Could not load preset from file {0}", presetFile);
            var tempFile = Path.GetTempFileName();
            preset.ToFile(tempFile);

            var xmldiff = new XmlDiff(options: XmlDiffOptions.IgnoreNamespaces | XmlDiffOptions.IgnoreDtd |
                XmlDiffOptions.IgnoreChildOrder | XmlDiffOptions.IgnorePrefixes | XmlDiffOptions.IgnoreWhitespace);

            string message;
            bool identical;

            using (var sw = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(sw))
                {
                    identical = xmldiff.Compare(presetFile, tempFile, false, xmlWriter);
                }
                message = sw.ToString();
            }

            if (!identical)
            {
                var presetDoc = new XmlDocument();
                presetDoc.Load(presetFile);
                var xmlPatch = new XmlPatch();
                var xmlReader = new XmlTextReader(new StringReader(message));
                xmlPatch.Patch(presetDoc, xmlReader);
                var tempOutputFile = Path.GetTempFileName();
                using (var xmlTextWriter = XmlWriter.Create(tempOutputFile))
                {
                    presetDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    //patched = stringWriter.GetStringBuilder().ToString();
                }
                Debug.WriteLine("ERROR: Files not identical");
                Debug.WriteLine($"ERROR: Original: {presetFile}");
                Debug.WriteLine($"ERROR: Patched: {tempOutputFile}");
            }

            Assert.IsTrue(identical, "Did not regenerate identical Xml for '{0}', message: {1}", presetFile, message);
            File.Delete(tempFile);
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