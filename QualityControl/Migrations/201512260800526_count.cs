namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class count : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "Count", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxTrade", "Count");
        }
    }
}
