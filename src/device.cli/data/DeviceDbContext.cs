using System.Data.Entity;
using forte.devices.data.configs;
using forte.devices.entities;

namespace forte.devices.data
{
	public class DeviceDbContext : DbContext
    {
        public DeviceDbContext() : this("local-db") { }

        public DeviceDbContext(string connectionString) : base(connectionString)
        {
            //Database.CreateIfNotExists();
        }

        public DbSet<SessionState> SessionStates { get; set; }

        public DbSet<DeviceSetting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DeviceSettingsConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}