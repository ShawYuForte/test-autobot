#region

using System.ServiceProcess;

#endregion

namespace device.ws
{
    public partial class DeviceService : ServiceBase
    {
        public DeviceService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}