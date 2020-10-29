namespace AutobotLauncher
{
	public class MainWindowViewModel : BaseViewModel
	{
		private string _status;
		private bool? _isNugetInstalled;
		private bool? _isChocoInstalled;
		private bool? _isClientInstalled;
		private bool? _isVmixInstalled;
		private bool? _isApiConnected;

		public LaunchStatus StatusEnum { get; set; } = LaunchStatus.None;

		public string Status
		{
			get { return _status; }
			set { _status = value; OnPropertyChanged("Status"); }
		}

		public bool? IsNugetInstalled
		{
			get { return _isNugetInstalled; }
			set { _isNugetInstalled = value; OnPropertyChanged("IsNugetInstalled"); }
		}

		public bool? IsChocoInstalled
		{
			get { return _isChocoInstalled; }
			set { _isChocoInstalled = value; OnPropertyChanged("IsChocoInstalled"); }
		}

		public bool? IsClientInstalled
		{
			get { return _isClientInstalled; }
			set { _isClientInstalled = value; OnPropertyChanged("IsClientInstalled"); }
		}

		public bool? IsVmixInstalled
		{
			get { return _isVmixInstalled; }
			set { _isVmixInstalled = value; OnPropertyChanged("IsVmixInstalled"); }
		}

		public bool? IsApiConnected
		{
			get { return _isApiConnected; }
			set { _isApiConnected = value; OnPropertyChanged("IsApiConnected"); }
		}

		public MainWindowViewModel()
		{
			SetStatus();

			//#if DEBUG
			//_isNugetInstalled = false;
			//_isChocoInstalled = false;
			//_isClientInstalled = false;
			//_isVmixInstalled = false;
			//_isApiConnected = false;
			//#endif
		}

		public void SetStatus()
		{
			if (IsNugetInstalled == false || IsChocoInstalled == false || IsClientInstalled == false)
			{
				StatusEnum = LaunchStatus.AutoInstall;
				Status = "Click to install required components";
				return;
			}

			if (IsVmixInstalled == false || IsApiConnected == false)
			{
				StatusEnum = LaunchStatus.ManualInstall;
				Status = "Install missing components manually";
				return;
			}

			StatusEnum = LaunchStatus.None;
			Status = "Launch";
		}

	}

	public enum LaunchStatus
	{
		None, AutoInstall, ManualInstall
	}
}
