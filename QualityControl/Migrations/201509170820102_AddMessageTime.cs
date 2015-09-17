namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxMessage", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxMessage", "Time");
        }
    }
}
