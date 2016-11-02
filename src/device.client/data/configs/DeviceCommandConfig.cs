using System.Data.Entity.ModelConfiguration;
using forte.devices.entities;

namespace forte.devices.data.configs
{
    public class DeviceCommandConfig : EntityTypeConfiguration<StreamingDeviceCommandEntity>
    {
        public DeviceCommandConfig()
        {
            HasKey(e => e.Id);
            //Property(e => e.Version).IsRowVersion();
            Ignore(e => e.Version);
        }
    }
}