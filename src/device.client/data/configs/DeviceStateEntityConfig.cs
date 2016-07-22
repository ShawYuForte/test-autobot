using System.Data.Entity.ModelConfiguration;
using forte.devices.models;

namespace forte.devices.data.configs
{
    public class StreamingDeviceStateEntityConfig : EntityTypeConfiguration<StreamingDeviceState>
    {
        public StreamingDeviceStateEntityConfig()
        {
            HasKey(e => e.DeviceId);
        }
    }
}