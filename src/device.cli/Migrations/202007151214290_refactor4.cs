namespace forte.devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionStates", "IsCancelled", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionStates", "IsCancelled");
        }
    }
}
