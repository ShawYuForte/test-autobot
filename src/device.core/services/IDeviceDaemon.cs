using System.Threading.Tasks;

namespace forte.devices.services
{
	public interface IDeviceDaemon
    {
		void Await(int port);
		Task Start();
		void Shutdown();
    }
}