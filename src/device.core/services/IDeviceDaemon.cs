using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models.devices;

namespace forte.devices.services
{
	public interface IDeviceDaemon
    {
		void Await(int port);
		Task Start();
		void Shutdown();
		List<ISessionState> GetState();
	}
}