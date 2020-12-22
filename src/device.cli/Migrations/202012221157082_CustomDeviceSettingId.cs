namespace forte.devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomDeviceSettingId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceSettings", "CustomDeviceId", c => c.Guid());
            AddColumn("dbo.DeviceSettings", "CustomDeviceIdPresent", c => c.Boolean());
        }

        public override void Down()
        {
            DropColumn("dbo.DeviceSettings", "IsCancelled");
            DropColumn("dbo.DeviceSettings", "CustomDeviceIdPresent");
        }
    }
}
