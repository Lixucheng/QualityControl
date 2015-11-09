namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeDectectionItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "DetectionItems", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxTrade", "DetectionItems");
        }
    }
}
