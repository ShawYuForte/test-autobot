namespace forte.devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionStates", "RetryCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionStates", "RetryCount");
        }
    }
}
