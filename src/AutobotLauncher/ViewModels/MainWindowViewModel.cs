using System;
using System.Collections.Generic;

namespace AutobotLauncher
{
	public class MainWindowViewModel : BaseViewModel
	{
		private const string _unknown = "checking...";

		private string _status;
		private string _clientVersion;
		private string _clientVersionLatest = _unknown;
		private bool? _isClientLaunched;
		private bool? _isNugetInstalled;
		private bool? _isClientInstalled;
		private bool? _isVmixInstalled;
		private bool? _isApiConnected;

		public bool? ClientVersionIsLatest
		{
			get
			{
				return ClientVersion == ClientVersionLatest || ClientVersionLatest == _unknown;
			}
		}

		public LaunchStatus StatusEnum { get; set; } = LaunchStatus.None;

		public string Status
		{
			get { return _status; }
			set { _status = value; OnPropertyChanged("Status"); }
		}

		public string ClientVersion
		{
			get { return _clientVersion; }
			set
			{
				_clientVersion = value;
				OnPropertyChanged("ClientVersion");
				OnPropertyChanged("ClientVersionIsLatest");
			}
		}

		public string ClientVersionLatest
		{
			get { return _clientVersionLatest; }
			set
			{
				_clientVersionLatest = value;
				OnPropertyChanged("ClientVersionLatest");
				OnPropertyChanged("ClientVersionIsLatest");
			}
		}

		public bool? IsClientLaunched
		{
			get { return _isClientLaunched; }
			set { _isClientLaunched = value; OnPropertyChanged("IsClientLaunched"); SetStatus(); }
		}

		public bool? IsNugetInstalled
		{
			get { return _isNugetInstalled; }
			set { _isNugetInstalled = value; OnPropertyChanged("IsNugetInstalled"); SetStatus(); }
		}

		public bool? IsClientInstalled
		{
			get { return _isClientInstalled; }
			set { _isClientInstalled = value; OnPropertyChanged("IsClientInstalled"); SetStatus(); }
		}

		public bool? IsVmixInstalled
		{
			get { return _isVmixInstalled; }
			set { _isVmixInstalled = value; OnPropertyChanged("IsVmixInstalled"); SetStatus(); }
		}

		public bool? IsApiConnected
		{
			get { return _isApiConnected; }
			set { _isApiConnected = value; OnPropertyChanged("IsApiConnected"); SetStatus(); }
		}

		public MainWindowViewModel()
		{
			Reset();
			SetStatus();
		}

		#region state management

		private bool _checkInProgress;
		private bool _installInProgress;
		private bool _vmixInProgress;

		public bool CheckInProgress
		{
			get { return _checkInProgress; }
			set { _checkInProgress = value; SetStatus(); }
		}

		public bool InstallInProgress
		{
			get { return _installInProgress; }
			set { _installInProgress = value; SetStatus(); }
		}

		public bool VmixInProgress
		{
			get { return _vmixInProgress; }
			set { _vmixInProgress = value; SetStatus(); }
		}

		public void Reset()
		{
			IsClientLaunched = null;
			IsNugetInstalled = null;
			IsClientInstalled = null;
			IsVmixInstalled = null;
			IsApiConnected = null;
		}

		public void SetStatus()
		{
			if (VmixInProgress)
			{
				Status = "Checking vmix preset...";
				return;
			}

			if (InstallInProgress)
			{
				Status = "Install is in progress...";
				return;
			}

			if (CheckInProgress)
			{
				Status = "Component check is in progress...";
				return;
			}

			if (IsNugetInstalled != true || IsClientInstalled != true || IsClientLaunched != true || IsVmixInstalled != true || IsApiConnected != true)
			{
				Status = "Required components need to be installed";
				return;
			}

			StatusEnum = LaunchStatus.None;
			Status = "Client is ready to use";
		}

		#endregion

		#region logs

		private List<string> _logs = new List<string>();

		public string Logs { get { return string.Join(Environment.NewLine, _logs); } }

		public void AddLog(string value)
		{
			_logs.Add(value);
			OnPropertyChanged("Logs");
		}

		#endregion
	}

	public enum LaunchStatus
	{
		None
	}
}
