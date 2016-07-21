using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using forte.devices.models;
using forte.devices.models.presets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace forte.devices.services.clients.tests
{
    [TestClass]
    public class VmixStreamingClientTests
    {
        [TestMethod, Ignore]
        public void TestPresetGeneration()
        {
            var presetFile = @"C:\dev\forte\iot\src\device.client.vmix.tests\services\clients\tests\data\preset1.vmix";
            var preset =VmixPreset.FromFile(presetFile);
            Assert.IsNotNull(preset);
            var tempFile = Path.GetTempFileName();
            preset.ToFile(tempFile);

            //var expected = File.OpenText(presetFile).ReadToEnd();
            //var actual = File.OpenText(tempFile).ReadToEnd();

            //Assert.IsTrue(same);
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod, Ignore]
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
                    if (!beginning && !Object.Equals(currentValue, previousValue))
                    {
                        same = false;
                    }
                    beginning = false;
                    previousValue = currentValue;
                    if (!same) break;
                }

                if (same) sameProperties.Add(propertyInfo.Name, previousValue);
            }

            var buffer = new StringBuilder();
            foreach (var property in sameProperties)
            {
                if ((property.Value as string) != null)
                    buffer.AppendLine($"{property.Key}=\"{property.Value}\",");
                else
                    buffer.AppendLine($"{property.Key}={property.Value},");
            }
            var code = buffer.ToString();
        }
    }
}