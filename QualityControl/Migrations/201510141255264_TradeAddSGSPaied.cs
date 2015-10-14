namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeAddSGSPaied : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "SGSPaied", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxTrade", "SGSPaied");
        }
    }
}
