using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.Text;
using System.Xml.Linq;

namespace AutobotLauncher.InstallAsm
{
    [RunInstaller(true)]
    public partial class AutobotInstaller : System.Configuration.Install.Installer
    {
        public AutobotInstaller()
        {
            InitializeComponent();

        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            try
            {
                SaveUserData();
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message);
                base.Rollback(stateSaver);
            }
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            try
            {
                SaveUserData();
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message);
                base.Rollback(savedState);
            }
        }

        private void SaveUserData()
        {
            // string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            // MessageBox.Show(path);
        }

        private string TryGetValueFromWizardContext(string parameterName)
        {
            try
            {
                return Context.Parameters[parameterName];
            }
            catch
            {
                return null;
            }
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            var dir = Path.GetDirectoryName(Context.Parameters["assemblypath"]);
            var path = $"{dir}//userSettings.xml";

            if (File.Exists(path))
                File.Delete(path);

            var xdoc = new XDocument();
            var forteConfig = new XElement("forteConfiguration");
            var forteSettings = new XElement("forteSettings");

            var customDeviceIdAttr = new XAttribute("CustomDeviceId", Context.Parameters["DEVICEID"]);
            var studioUrlAttr = new XAttribute("StudioUrl", Context.Parameters["STUDIOURL"]);
            var videoUrlAttr = new XAttribute("VideoUrl", Context.Parameters["VIDEOURL"]);

            forteSettings.Add(customDeviceIdAttr);
            forteSettings.Add(studioUrlAttr);
            forteSettings.Add(videoUrlAttr);

            forteConfig.Add(forteSettings);
            xdoc.Add(forteConfig);
            xdoc.Save($"{dir}//userSettings.xml");
        }
    }
}
