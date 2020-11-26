using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutobotLauncher.Forms.Models;
using AutobotLauncher.Utils;

namespace AutobotLauncher.Forms
{
	/// <summary>
	/// Interaction logic for BaseConfigForm.xaml
	/// </summary>
	public partial class BaseConfigForm : Window
	{
		private BaseConfigModel _model = new BaseConfigModel();

		public BaseConfigForm()
		{
			DataContext = _model;
			InitializeComponent();
		}

		public async Task Init()
		{
			_model.DeviceId = await ClientApiInteractor.Setting("DeviceId");
			_model.AgoraApp = await ClientApiInteractor.Setting("agora-app-id");
			_model.ApiPath = await ClientApiInteractor.Setting("server-root-path");
		}

		private void ClickCancel(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private async void ClickSave(object sender, RoutedEventArgs e)
		{
			await ClientApiInteractor.SettingSave("DeviceId", _model.DeviceId);
			await ClientApiInteractor.SettingSave("agora-app-id", _model.AgoraApp);
			await ClientApiInteractor.SettingSave("server-root-path", _model.ApiPath);
			await ClientApiInteractor.SettingSave("server-api-path", _model.ApiPathExt);
			DialogResult = true;
			Close();
		}
	}
}
