using System.Data.Entity;
using forte.devices.data.configs;
using forte.devices.entities;

namespace forte.devices.data
{
    public class DeviceDbContext : DbContext
    {
        public DeviceDbContext() : base("local-db")
        {
        }

        public DbSet<DeviceConfig> DeviceConfig { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DeviceEntityConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}