namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeAddSampleReceived : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxTrade", "SampleRecevied", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GxTrade", "SampleRecevied");
        }
    }
}
