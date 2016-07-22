using System.Data.Entity.ModelConfiguration;
using forte.devices.entities;

namespace forte.devices.data.configs
{
    public class DeviceEntityConfig : EntityTypeConfiguration<DeviceConfig>
    {
        public DeviceEntityConfig()
        {
            HasKey(e => e.Id);
            Property(e => e.Version).IsRowVersion();
        }
    }
}