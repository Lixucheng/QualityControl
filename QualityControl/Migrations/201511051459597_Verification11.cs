namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Verification11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxQrCodeInfo", "TradeId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxQrCodeInfo", "TradeId");
        }
    }
}
