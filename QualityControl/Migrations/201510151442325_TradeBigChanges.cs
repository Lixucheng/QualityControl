namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeBigChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxDetectionReport", "Trade_Id", "dbo.GxTrade");
            DropIndex("dbo.GxDetectionReport", new[] { "Trade_Id" });
            AddColumn("dbo.GxTrade", "CompanyUserId", c => c.String());
            AddColumn("dbo.GxTrade", "SamplingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GxTrade", "DetectingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GxTrade", "Result_Id", c => c.Long());
            AddColumn("dbo.GxDetectionReport", "CompanyName", c => c.String());
            AddColumn("dbo.GxDetectionReport", "Title", c => c.String());
            AddColumn("dbo.GxDetectionReport", "DataList", c => c.String());
            AddColumn("dbo.GxDetectionReport", "Conclusion", c => c.String());
            AddColumn("dbo.GxDetectionReport", "Comments", c => c.String());
            CreateIndex("dbo.GxTrade", "Result_Id");
            AddForeignKey("dbo.GxTrade", "Result_Id", "dbo.GxDetectionReport", "Id");
            DropColumn("dbo.GxDetectionReport", "Trade_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GxDetectionReport", "Trade_Id", c => c.Long());
            DropForeignKey("dbo.GxTrade", "Result_Id", "dbo.GxDetectionReport");
            DropIndex("dbo.GxTrade", new[] { "Result_Id" });
            DropColumn("dbo.GxDetectionReport", "Comments");
            DropColumn("dbo.GxDetectionReport", "Conclusion");
            DropColumn("dbo.GxDetectionReport", "DataList");
            DropColumn("dbo.GxDetectionReport", "Title");
            DropColumn("dbo.GxDetectionReport", "CompanyName");
            DropColumn("dbo.GxTrade", "Result_Id");
            DropColumn("dbo.GxTrade", "DetectingDate");
            DropColumn("dbo.GxTrade", "SamplingDate");
            DropColumn("dbo.GxTrade", "CompanyUserId");
            CreateIndex("dbo.GxDetectionReport", "Trade_Id");
            AddForeignKey("dbo.GxDetectionReport", "Trade_Id", "dbo.GxTrade", "Id");
        }
    }
}
