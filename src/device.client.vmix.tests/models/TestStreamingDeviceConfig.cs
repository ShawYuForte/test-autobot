namespace forte.devices.models
{
    public class TestStreamingDeviceConfig : StreamingDeviceConfig
    {
        public void Set<T>(string setting, T value)
        {
            this[setting].Set(value);
        }
    }
}