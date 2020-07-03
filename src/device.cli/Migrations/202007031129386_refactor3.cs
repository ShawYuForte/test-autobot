namespace forte.devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionStates", "SessionType", c => c.Int(nullable: false));
            DropColumn("dbo.SessionStates", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SessionStates", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.SessionStates", "SessionType");
        }
    }
}
