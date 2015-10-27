namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class file : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "Files", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxTrade", "Files");
        }
    }
}
