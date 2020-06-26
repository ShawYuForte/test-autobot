using System.Data.Entity.ModelConfiguration;
using forte.devices.entities;

namespace forte.devices.data.configs
{
    public class DeviceSettingsConfig : EntityTypeConfiguration<DeviceSetting>
    {
        public DeviceSettingsConfig()
        {
            HasKey(e => e.Id);
            //Property(e => e.Version).IsRowVersion();
            Ignore(e => e.Version);
        }
    }
}