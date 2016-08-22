#region

using System.Data.Entity.Migrations;

#endregion

namespace forte.devices.Migrations
{
    public partial class Init : DbMigration
    {
        public override void Down()
        {
            DropColumn("dbo.DeviceCommands", "CanceledOn");
            DropColumn("dbo.DeviceCommands", "VideoStreamId");
        }

        public override void Up()
        {
            AddColumn("dbo.DeviceCommands", "CanceledOn", c => c.DateTime(nullable: true));
            AddColumn("dbo.DeviceCommands", "VideoStreamId", c => c.Guid(nullable: true));
        }
    }
}