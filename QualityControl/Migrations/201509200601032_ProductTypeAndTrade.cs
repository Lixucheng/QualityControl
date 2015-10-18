using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class ProductTypeAndTrade : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.GxSecondProductType", "FirstProductType_Id", "FirstType_Id");
            RenameColumn("dbo.GxThirdProductType", "SecondProductType_Id", "SecondType_Id");
            RenameIndex("dbo.GxThirdProductType", "IX_SecondProductType_Id", "IX_SecondType_Id");
            RenameIndex("dbo.GxSecondProductType", "IX_FirstProductType_Id", "IX_FirstType_Id");
            CreateTable(
                "dbo.GxTrade",
                c => new
                {
                    Id = c.Long(false, true),
                    SampleType = c.Int(false),
                    CeateTime = c.DateTime(false),
                    Status = c.Int(false),
                    FinishTime = c.DateTime(false)
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.GxDetectionScheme", "Trade_Id", c => c.Long());
            CreateIndex("dbo.GxDetectionScheme", "Trade_Id");
            AddForeignKey("dbo.GxDetectionScheme", "Trade_Id", "dbo.GxTrade", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxDetectionScheme", "Trade_Id", "dbo.GxTrade");
            DropIndex("dbo.GxDetectionScheme", new[] {"Trade_Id"});
            DropColumn("dbo.GxDetectionScheme", "Trade_Id");
            DropTable("dbo.GxTrade");
            RenameIndex("dbo.GxSecondProductType", "IX_FirstType_Id", "IX_FirstProductType_Id");
            RenameIndex("dbo.GxThirdProductType", "IX_SecondType_Id", "IX_SecondProductType_Id");
            RenameColumn("dbo.GxThirdProductType", "SecondType_Id", "SecondProductType_Id");
            RenameColumn("dbo.GxSecondProductType", "FirstType_Id", "FirstProductType_Id");
        }
    }
}