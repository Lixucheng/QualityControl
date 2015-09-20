namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTypeAndTrade : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GxSecondProductType", name: "FirstProductType_Id", newName: "FirstType_Id");
            RenameColumn(table: "dbo.GxThirdProductType", name: "SecondProductType_Id", newName: "SecondType_Id");
            RenameIndex(table: "dbo.GxThirdProductType", name: "IX_SecondProductType_Id", newName: "IX_SecondType_Id");
            RenameIndex(table: "dbo.GxSecondProductType", name: "IX_FirstProductType_Id", newName: "IX_FirstType_Id");
            CreateTable(
                "dbo.GxTrade",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SampleType = c.Int(nullable: false),
                        CeateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        FinishTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.GxDetectionScheme", "Trade_Id", c => c.Long());
            CreateIndex("dbo.GxDetectionScheme", "Trade_Id");
            AddForeignKey("dbo.GxDetectionScheme", "Trade_Id", "dbo.GxTrade", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxDetectionScheme", "Trade_Id", "dbo.GxTrade");
            DropIndex("dbo.GxDetectionScheme", new[] { "Trade_Id" });
            DropColumn("dbo.GxDetectionScheme", "Trade_Id");
            DropTable("dbo.GxTrade");
            RenameIndex(table: "dbo.GxSecondProductType", name: "IX_FirstType_Id", newName: "IX_FirstProductType_Id");
            RenameIndex(table: "dbo.GxThirdProductType", name: "IX_SecondType_Id", newName: "IX_SecondProductType_Id");
            RenameColumn(table: "dbo.GxThirdProductType", name: "SecondType_Id", newName: "SecondProductType_Id");
            RenameColumn(table: "dbo.GxSecondProductType", name: "FirstType_Id", newName: "FirstProductType_Id");
        }
    }
}
