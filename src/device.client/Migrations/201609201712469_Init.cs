#region

using System.Data.Entity.Migrations;

#endregion

namespace forte.devices.Migrations
{
    public partial class Init : DbMigration
    {
        public override void Down()
        {
            DropTable("dbo.DeviceSettings");
            DropTable("dbo.StreamingDeviceStates");
            DropTable("dbo.DeviceConfigs");
            DropTable("dbo.DeviceCommandEntities");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.DeviceCommandEntities",
                    c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceId = c.Guid(nullable: false),
                        Command = c.Int(nullable: false),
                        IssuedOn = c.DateTime(nullable: false),
                        ExecutedOn = c.DateTime(),
                        Status = c.Int(nullable: false),
                        PublishedOn = c.DateTime(),
                        ExecutionMessages = c.String(maxLength: 4000),
                        Data = c.String(maxLength: 4000),
                        Created = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.DeviceConfigs",
                    c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceId = c.Guid(nullable: false),
                        OperatingSystem = c.String(maxLength: 4000),
                        Processor = c.String(maxLength: 4000),
                        Memory = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.StreamingDeviceStates",
                    c => new
                    {
                        DeviceId = c.Guid(nullable: false),
                        StateCapturedOn = c.DateTime(nullable: false),
                        ActiveVideoStreamId = c.Guid(),
                        Status = c.Int(nullable: false),
                        StreamingPresetLoadHash = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.DeviceId);

            CreateTable(
                    "dbo.DeviceSettings",
                    c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 4000),
                        GuidValue = c.Guid(),
                        StringValue = c.String(maxLength: 4000),
                        IntValue = c.Int(),
                        DateTimeValue = c.DateTime(),
                        BoolValue = c.Boolean(),
                        ByteArrayValue = c.Binary(maxLength: 4000),
                        Created = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
    }
}