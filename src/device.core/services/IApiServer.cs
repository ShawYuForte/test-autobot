using System;

namespace forte.devices.services
{
    public interface IApiServer
    {
		IDisposable Run(int port);
	}
}