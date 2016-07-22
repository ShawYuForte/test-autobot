using System.Data.Entity;
using forte.devices.data.configs;
using forte.devices.entities;
using forte.devices.models;

namespace forte.devices.data
{
    public class DeviceDbContext : DbContext
    {
        public DeviceDbContext() : base("local-db")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<DeviceConfig> DeviceConfig { get; set; }
        public DbSet<StreamingDeviceState> DeviceState { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DeviceEntityConfig());
            modelBuilder.Configurations.Add(new StreamingDeviceStateEntityConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}