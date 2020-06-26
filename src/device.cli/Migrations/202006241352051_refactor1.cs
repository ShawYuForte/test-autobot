namespace forte.devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionStates", "VmixUsed", c => c.Boolean(nullable: false));
            DropTable("dbo.StreamingDeviceCommands");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StreamingDeviceCommands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CanceledOn = c.DateTime(),
                        Command = c.Int(nullable: false),
                        Data = c.String(maxLength: 4000),
                        ExecutedOn = c.DateTime(),
                        ExecutionAttempts = c.Int(nullable: false),
                        ExecutionMessages = c.String(maxLength: 4000),
                        ExecutionSucceeded = c.Boolean(nullable: false),
                        IssuedOn = c.DateTime(nullable: false),
                        MaxAttemptsAllowed = c.Int(nullable: false),
                        StreamingDeviceId = c.Guid(nullable: false),
                        VideoStreamId = c.Guid(),
                        Created = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.SessionStates", "VmixUsed");
        }
    }
}
