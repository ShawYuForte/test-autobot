using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using AutobotLauncher.Enums;
using AutobotLauncher.Forms;
using AutobotLauncher.Utils;

namespace AutobotLauncher
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _model = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _model;
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(Check);
            t1.Start();
        }

        private void Check()
        {
            if (_model.CheckInProgress) { return; }
            _model.CheckInProgress = true;
            _model.Reset();

            RunAndWaitOffCompletion(new Task(() => { _model.IsVmixInstalled = File.Exists(Constants.VmixPath); }));
            RunAndWaitOffCompletion(new Task(() => { _model.IsVmixInstalled = File.Exists(Constants.VmixPath); }));
            RunAndWaitOffCompletion(new Task(() => { _model.IsNugetInstalled = IsNugetInstalled().Result; }));
            RunAndWaitOffCompletion(new Task(() => { UpdateNugetSources().Wait(); }));
            RunAndWaitOffCompletion(new Task(() => { CheckLatest().Wait(); }));
            RunAndWaitOffCompletion(new Task(() => { CheckClient().Wait(); }));
            RunAndWaitOffCompletion(new Task(() => { Launch().Wait(); }));
            RunAndWaitOffCompletion(new Task(() => { _model.IsApiConnected = IsApiConnected().Result; }));

            RunAndWaitOffCompletion(new Task(() => { UserSettingUtils.InitUserSettingsAfterInstallation().Wait(); }));


            _model.SetStatus();
            _model.CheckInProgress = false;
        }

        private void RunAndWaitOffCompletion(Task task)
        {
            task.Start();
            Task.WhenAll(task).Wait();
        }

        private async Task Launch()
        {
            _model.IsClientLaunched = await ClientInteractor.StartClient(_model.ClientVersion);
        }

        private async Task CheckClient()
        {
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

            if (!File.Exists(FileUtils.GetClientPath(v)))
            {
                _model.ClientVersion = v;
                // install current version
                var cm = $"install device-cli -version {_model.ClientVersion}";

                var result = await "nuget".ProcessRunAndWaitAsAdmin(cm);

                //package is installed
                var pinstalled = result.FirstOrDefault(m => m != null && m.Contains("is already installed.")); //

                var r = FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin("-version").GetAwaiter().GetResult();
                if (r != null)
                {
                    _model.ClientVersion = r.FirstOrDefault();
                }
                else
                {
                    _model.ClientVersion = v;
                }
                _model.IsClientInstalled = r != null;
            }
            else
            {
                //verify that client is ready to use
                var r = FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin("-version").GetAwaiter().GetResult();
                if (r != null)
                {
                    _model.ClientVersion = r.FirstOrDefault();
                }
                else
                {
                    _model.ClientVersion = v;

                }
                _model.IsClientInstalled = true;
            }

            // check if device-cli.exe exists for current installed version            
            if (!File.Exists(FileUtils.GetClientPath(_model.ClientVersion)))
            {
                // install current version
                var cm = $"install device-cli -version {_model.ClientVersion}";

                var result = await "nuget".ProcessRunAndWaitAsAdmin(cm);

                //package is installed
                var pinstalled = result.FirstOrDefault(m => m != null && m.Contains("is already installed.")); //

                var r = FileUtils.GetClientPath(v).ProcessRunAndWaitAsAdmin("-version").GetAwaiter().GetResult();
                _model.IsClientInstalled = r != null;
            }
        }

        private async Task<string> CheckLatest()
        {
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
            }
            //nothing installed
            if (pinstalled == null)
            {
                _model.ClientVersionLatest = null;
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

        private async Task UpdateNugetSources()
        {
            var cm1 = "sources remove -name forte-approved";
            var cm2 = "sources add -name forte-approved -Source https://pkgs.dev.azure.com/forte-fit/_packaging/forte-approved/nuget/v3/index.json -username NugetUser -password 24cmiauw26nqlzp7o5u7oinrcczwujeqx2zaghavpn2prs4w2fwa";

            await "nuget".ProcessRunAndWaitAsAdmin(cm1);
            await "nuget".ProcessRunAndWaitAsAdmin(cm2);
        }

        private async Task InstallLatest()
        {
            if (_model.CheckInProgress) { return; }
            if (_model.InstallInProgress) { return; }
            _model.InstallInProgress = true;

            _model.IsClientInstalled = null;

            Shutdown().Wait();

            var newVersion = File.ReadAllText(Constants.LatestFileName.GetAbsolutePath());
            File.WriteAllText(Constants.VersionFileName.GetAbsolutePath(), newVersion);

            Check();

            _model.InstallInProgress = false;
        }

        private async Task Shutdown()
        {
            _model.IsClientLaunched = null;

            await ClientApiInteractor.Shutdown();
        }

        private async Task<bool> IsApiConnected()
        {
            return await ClientInteractor.CheckApi(_model.ClientVersion);
        }

        private async Task<bool> IsNugetInstalled()
        {
            return await "nuget".ProcessRunAndWaitAsAdmin() != null;
        }

        private async Task CheckApi()
        {
            _model.IsApiConnected = await ClientInteractor.CheckApi(_model.ClientVersion);
        }

        #region handlers

        private void ClickCheck(object sender, RoutedEventArgs e)
        {
            if (_model.CheckInProgress) { return; }

            Check();
        }

        private void ClickInstall(object sender, RoutedEventArgs e)
        {
            RunAndWaitOffCompletion(new Task(() => { InstallLatest().Wait(); }));
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

                    try
                    {
                        //await Shutdown();
                    }
                    catch { }

                    await Launch();
                    await CheckApi();
                    //RunAndWaitOffCompletion(new Task(() => { Launch().Wait(); }));
                    //RunAndWaitOffCompletion(new Task(() => { CheckApi().Wait(); }));
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
