using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models.devices;

namespace forte.devices.services
{
	public interface IDeviceDaemon
	{
		Task CheckApi(int port);
		void Await(int port);
		Task Start();
		Task RunPresetTest();
		Task RunApiTest();
		void Shutdown();
		List<ISessionState> GetState();
	}
}