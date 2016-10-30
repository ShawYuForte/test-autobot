#region

using forte.devices.options;
using forte.devices.services;

#endregion

namespace forte.devices.commands
{
    public class SimulatorCommand
    {
        private readonly SimulatorOptions _options;

        public SimulatorCommand(SimulatorOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            var deviceSumulator = new DeviceSimulator(_options.ServerUrl, _options.DeviceId);
            deviceSumulator.Run();
        }
    }
}