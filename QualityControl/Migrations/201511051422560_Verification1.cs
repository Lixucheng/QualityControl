namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Verification1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Verification", "TradeId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Verification", "TradeId");
        }
    }
}
