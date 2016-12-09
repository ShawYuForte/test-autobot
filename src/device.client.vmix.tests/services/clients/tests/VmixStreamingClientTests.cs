using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using forte.devices.models;
using forte.devices.models.presets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace forte.devices.services.clients.tests
{
    [TestClass]
    public class VmixStreamingClientTests
    {
        [TestMethod, Ignore]
        public void TestPresetGeneration()
        {
            var presetFile = @"C:\dev\forte\iot\src\device.client.vmix.tests\services\clients\tests\data\preset1.vmix";
            var preset = VmixPreset.FromFile(presetFile);
            Assert.IsNotNull(preset);
            var tempFile = Path.GetTempFileName();
            preset.ToFile(tempFile);

            //var expected = File.OpenText(presetFile).ReadToEnd();
            //var actual = File.OpenText(tempFile).ReadToEnd();

            //Assert.IsTrue(same);
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod, Ignore]
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

        [TestMethod, Ignore]
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

        [DllImport("user32")]
        private static extern int IsWindowEnabled(int hWnd);

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wFlag);

        [TestMethod, Ignore]
        public void TestDialogs()
        {
            var vmixPath = @"C:\Program Files (x86)\vMix\vMix64.exe";
            var vmixProcessName = "vMix64";
            var existingProcess = Process.GetProcessesByName(vmixProcessName).FirstOrDefault();

            var vMixProcess = existingProcess ?? Process.Start(new ProcessStartInfo
            {
                FileName = vmixPath,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            var handle = vMixProcess.MainWindowHandle;
            var ptr = handle.ToInt32();
            var enabled = IsWindowEnabled(ptr);
            var owner = GetWindow(handle, /*GW_OWNER*/ 4);
            var ownerEnabled = IsWindowEnabled(owner.ToInt32());
        }

        [TestMethod]
        public void Test()
        {
            var buffer = new StringBuilder();
            var process = new Process
            {
                StartInfo =
            {
                FileName = "ipconfig",
                Arguments = "/flushdns",
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
            };
            process.OutputDataReceived +=
                delegate(object o, DataReceivedEventArgs args) { buffer.AppendLine(args.Data); }; 

            process.Start();

            // Start the asynchronous read of the sort output stream.
            process.BeginOutputReadLine();

            process.WaitForExit();
            var output = buffer.ToString();
            "Successfully flushed"
            Assert.IsNotNull(output);
        }
    }
}