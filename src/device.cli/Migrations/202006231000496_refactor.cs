namespace forte.devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        SessioId = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Permalink = c.String(maxLength: 4000),
                        VmixPreset = c.String(maxLength: 4000),
                        PrimaryIngestUrl = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.DeviceConfigs");
            DropTable("dbo.StreamingDeviceStates");
        }
        
        public override void Down()
        {
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
            
            DropTable("dbo.SessionStates");
        }
    }
}
