using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Trade : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxDetectionReport",
                c => new
                {
                    Id = c.Long(false, true),
                    Status = c.Int(false),
                    Trade_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxTrade", t => t.Trade_Id)
                .Index(t => t.Trade_Id);

            AddColumn("dbo.GxProductBatch", "SampleCount", c => c.Int(false));
            AddColumn("dbo.GxProductBatch", "SamplaListJson", c => c.String());
            AddColumn("dbo.GxProductBatch", "Trade_Id", c => c.Long());
            AlterColumn("dbo.GxProductBatch", "Count", c => c.Int(false));
            CreateIndex("dbo.GxProductBatch", "Trade_Id");
            AddForeignKey("dbo.GxProductBatch", "Trade_Id", "dbo.GxTrade", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxDetectionReport", "Trade_Id", "dbo.GxTrade");
            DropForeignKey("dbo.GxProductBatch", "Trade_Id", "dbo.GxTrade");
            DropIndex("dbo.GxDetectionReport", new[] {"Trade_Id"});
            DropIndex("dbo.GxProductBatch", new[] {"Trade_Id"});
            AlterColumn("dbo.GxProductBatch", "Count", c => c.Long(false));
            DropColumn("dbo.GxProductBatch", "Trade_Id");
            DropColumn("dbo.GxProductBatch", "SamplaListJson");
            DropColumn("dbo.GxProductBatch", "SampleCount");
            DropTable("dbo.GxDetectionReport");
        }
    }
}