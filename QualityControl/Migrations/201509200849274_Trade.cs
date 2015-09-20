namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trade : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxDetectionReport",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Trade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxTrade", t => t.Trade_Id)
                .Index(t => t.Trade_Id);
            
            AddColumn("dbo.GxProductBatch", "SampleCount", c => c.Int(nullable: false));
            AddColumn("dbo.GxProductBatch", "SamplaListJson", c => c.String());
            AddColumn("dbo.GxProductBatch", "Trade_Id", c => c.Long());
            AlterColumn("dbo.GxProductBatch", "Count", c => c.Int(nullable: false));
            CreateIndex("dbo.GxProductBatch", "Trade_Id");
            AddForeignKey("dbo.GxProductBatch", "Trade_Id", "dbo.GxTrade", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxDetectionReport", "Trade_Id", "dbo.GxTrade");
            DropForeignKey("dbo.GxProductBatch", "Trade_Id", "dbo.GxTrade");
            DropIndex("dbo.GxDetectionReport", new[] { "Trade_Id" });
            DropIndex("dbo.GxProductBatch", new[] { "Trade_Id" });
            AlterColumn("dbo.GxProductBatch", "Count", c => c.Long(nullable: false));
            DropColumn("dbo.GxProductBatch", "Trade_Id");
            DropColumn("dbo.GxProductBatch", "SamplaListJson");
            DropColumn("dbo.GxProductBatch", "SampleCount");
            DropTable("dbo.GxDetectionReport");
        }
    }
}
