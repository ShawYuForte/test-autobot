using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using AutobotLauncher.Enums;
using AutobotLauncher.Forms;
using AutobotLauncher.Utils;
using log4net;

namespace AutobotLauncher
{
    public partial class MainWindow : Window
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MainWindowViewModel _model = new MainWindowViewModel();

        public MainWindow()
        {

            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            DataContext = _model;
            Task.Run(() => Check());
        }

        private async Task Check()
        {
            if (_model.CheckInProgress) { return; }
            _model.CheckInProgress = true;
            _model.Reset();

            var tasks = new List<Task>();
            var vmixTask = new Task(() =>
            {
                _model.IsVmixInstalled = File.Exists(Constants.VmixPath);
            });
            vmixTask.Start();
            tasks.Add(vmixTask);

            await CheckNuget();
            await UpdateNugetSources();
            await CheckLatest();
            await CheckClient();

            await Launch();
            await CheckApi();

            await Task.WhenAll(tasks.ToArray());

            _model.SetStatus();
            _model.CheckInProgress = false;
        }

        private async Task UpdateNugetSources()
        {
            _logger.Info("UpdateNugetSources - started.");
            var cm1 = "sources remove -name forte-approved";
            var cm2 = "sources add -name forte-approved -Source https://pkgs.dev.azure.com/forte-fit/_packaging/forte-approved/nuget/v3/index.json -username NugetUser -password 24cmiauw26nqlzp7o5u7oinrcczwujeqx2zaghavpn2prs4w2fwa";

            await "nuget".ProcessRunAndWaitAsAdmin(cm1);
            await "nuget".ProcessRunAndWaitAsAdmin(cm2);
        }

        private async Task CheckNuget()
        {
            _logger.Info("CheckNuget - started.");
            var r = await "nuget".ProcessRunAndWaitAsAdmin();

            _model.IsNugetInstalled = r != null;
            _logger.Info($"Nuget installed: {_model.IsNugetInstalled}");

        }

        private async Task CheckClient()
        {
            _logger.Info("CheckClient - started.");
            //check if client is installed
            var vfPath = Constants.VersionFileName.GetAbsolutePath();
            if (!File.Exists(vfPath))
            {
                var vInst = await CheckLatest();
                //got new package
                if (vInst != null)
                {
                    File.WriteAllText(vfPath, vInst);
                    await CheckClient();
                }
                return;
            }

            var v = File.ReadAllText(vfPath);
            //if no version installed
            if (v == null) { return; }

            //verify that client is ready to use
            _logger.Info("verify that client is ready to use.");
            var r = await FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin("-version");
            if (r != null)
            {
                _model.ClientVersion = r.FirstOrDefault();
            }

            _model.IsClientInstalled = r != null;
            _logger.Info($"Client installed: {_model.IsClientInstalled}");
        }

        private async Task<string> CheckLatest()
        {
            _logger.Info("CheckLatest - started.");
            var cm1 = "locals http-cache -clear";
            var cm2 = "install device-cli";

            await "nuget".ProcessRunAndWaitAsAdmin(cm1);
            var r = await "nuget".ProcessRunAndWaitAsAdmin(cm2);

            //package is installed
            var pinstalled = r.FirstOrDefault(m => m != null && m.Contains("is already installed.")); //"Package "device-cli.2.2.0" is already installed."
                                                                                                      //package was installed
            if (pinstalled == null)
            {
                pinstalled = r.FirstOrDefault(m => m != null && m.Contains("Successfully installed")); //Successfully installed 'device-cli 2.1.45'
                _logger.Info("CheckLatest - latest changes installed.");
            }
            //nothing installed
            if (pinstalled == null)
            {
                _model.ClientVersionLatest = null;
                _logger.Info("CheckLatest - latest changes not installed.");
                return null;
            }

            //get package version
            var match = Regex.Match(pinstalled, @"device-cli[ ]?[0-9\.]+");
            if (match.Success)
            {
                _model.ClientVersionLatest = Regex.Match(match.Value, @"[0-9][0-9\.]+").Value;
                var vfPath = Constants.LatestFileName.GetAbsolutePath();
                File.WriteAllText(vfPath, _model.ClientVersionLatest);
                return _model.ClientVersionLatest;
            }
            else
            {
                _model.ClientVersionLatest = null;
                return null;
            }
        }

        private async Task InstallLatest()
        {
            _logger.Info("InstallLatest - latest changes installed.");
            if (_model.CheckInProgress) { return; }
            if (_model.InstallInProgress) { return; }
            _model.InstallInProgress = true;

            _model.IsClientInstalled = null;
            await Shutdown();
            var newVersion = File.ReadAllText(Constants.LatestFileName.GetAbsolutePath());
            File.WriteAllText(Constants.VersionFileName.GetAbsolutePath(), newVersion);
            await Check();

            _model.InstallInProgress = false;
        }

        private async Task Launch()
        {
            _logger.Info("Launch - started.");
            _model.IsClientLaunched = await ClientInteractor.StartClient(_model.ClientVersion);
            if (_model.IsClientLaunched.HasValue && !_model.IsClientLaunched.Value)
            {
                _logger.Warn($"Client launched: {_model.IsClientLaunched}");
            }
        }

        private async Task CheckApi()
        {
            _logger.Info("CheckApi - started.");
            _model.IsApiConnected = await ClientInteractor.CheckApi(_model.ClientVersion);
            if (_model.IsApiConnected.HasValue && !_model.IsApiConnected.Value)
            {
                _logger.Warn($"Api Connected: {_model.IsApiConnected}");
            }
        }

        private async Task Shutdown()
        {
            _logger.Info("Shutdown - started.");
            _model.IsClientLaunched = null;

            await ClientApiInteractor.Shutdown();
        }

        #region handlers

        private async void ClickCheck(object sender, RoutedEventArgs e)
        {
            if (_model.CheckInProgress) { return; }

            await Check();
        }

        private async void ClickInstall(object sender, RoutedEventArgs e)
        {
            await InstallLatest();
        }

        private async void ClickShutdown(object sender, RoutedEventArgs e)
        {
            await Shutdown();
        }

        private async void ClickCheckPreset(object sender, RoutedEventArgs e)
        {
            if (_model.CheckInProgress) { return; }
            _model.CheckInProgress = true;
            try
            {
                await ClientInteractor.CheckPreset(_model.ClientVersion);
            }
            catch (Exception ex)
            {
                ex.Error();
            }
            _model.CheckInProgress = false;
        }

        private async void ClickEditApiPath(object sender, RoutedEventArgs e)
        {
            if (_model.CheckInProgress) { return; }
            try
            {
                var dlg = new BaseConfigForm();
                await dlg.Init();
                if (dlg.ShowDialog() == true)
                {
                    _model.IsApiConnected = null;

                    await Launch();
                    await CheckApi();
                }
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }

        private async void ClickPreset(object sender, RoutedEventArgs e)
        {
            var path = ClientInteractor.GetPresetFilePath(_model.ClientVersion);
            await "explorer.exe".ProcessRunAndWaitAsAdmin(path);
        }

        private async void ClickPresetFolder(object sender, RoutedEventArgs e)
        {
            var path = ClientInteractor.GetPresetFolderPath(_model.ClientVersion);
            await "explorer.exe".ProcessRunAndWaitAsAdmin(path);
        }

        private async void ClickAdvancedSettings(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://localhost:9000/#/dashboard"));
        }

        private async void ClickLink(object sender, RoutedEventArgs e)
        {
            var hl = (Hyperlink)sender;
            var navigateUri = hl.NavigateUri.ToString();

            Process.Start(new ProcessStartInfo(navigateUri));

            e.Handled = true;
        }

        #endregion
    }
}
