namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeRevert : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GxTrade", "CompanyUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GxTrade", "CompanyUserId", c => c.String());
        }
    }
}
