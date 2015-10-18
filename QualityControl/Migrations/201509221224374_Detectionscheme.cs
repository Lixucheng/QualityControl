using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class Detectionscheme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GxContractModification", "DetectionScheme_Id", c => c.Long());
            AddColumn("dbo.GxContract", "DetectionScheme_Id", c => c.Long());
            AddColumn("dbo.GxContract", "Product_Id", c => c.Long());
            AddColumn("dbo.GxDetectionScheme", "Product_Id", c => c.Long());
            CreateIndex("dbo.GxCompact", "Id");
            CreateIndex("dbo.GxDetectionScheme", "Product_Id");
            CreateIndex("dbo.GxContract", "DetectionScheme_Id");
            CreateIndex("dbo.GxContract", "Product_Id");
            CreateIndex("dbo.GxContractModification", "DetectionScheme_Id");
            AddForeignKey("dbo.GxContract", "DetectionScheme_Id", "dbo.GxDetectionScheme", "Id");
            AddForeignKey("dbo.GxContract", "Product_Id", "dbo.GxProduct", "Id");
            AddForeignKey("dbo.GxContractModification", "DetectionScheme_Id", "dbo.GxDetectionScheme", "Id");
            AddForeignKey("dbo.GxDetectionScheme", "Product_Id", "dbo.GxProduct", "Id");
            AddForeignKey("dbo.GxCompact", "Id", "dbo.GxTrade", "Id");
            DropColumn("dbo.GxCompact", "CheckNum");
            DropColumn("dbo.GxContractModification", "DetectionSchemeId");
            DropColumn("dbo.GxContract", "CheckNum");
            DropColumn("dbo.GxContract", "ProductId");
            DropColumn("dbo.GxDetectionScheme", "CheckNum");
            DropColumn("dbo.GxDetectionScheme", "ProductId");
            DropColumn("dbo.GxProductBatch", "CheckNum");
        }

        public override void Down()
        {
            AddColumn("dbo.GxProductBatch", "CheckNum", c => c.String());
            AddColumn("dbo.GxDetectionScheme", "ProductId", c => c.Long(false));
            AddColumn("dbo.GxDetectionScheme", "CheckNum", c => c.String());
            AddColumn("dbo.GxContract", "ProductId", c => c.Long(false));
            AddColumn("dbo.GxContract", "CheckNum", c => c.String());
            AddColumn("dbo.GxContractModification", "DetectionSchemeId", c => c.Long(false));
            AddColumn("dbo.GxCompact", "CheckNum", c => c.String());
            DropForeignKey("dbo.GxCompact", "Id", "dbo.GxTrade");
            DropForeignKey("dbo.GxDetectionScheme", "Product_Id", "dbo.GxProduct");
            DropForeignKey("dbo.GxContractModification", "DetectionScheme_Id", "dbo.GxDetectionScheme");
            DropForeignKey("dbo.GxContract", "Product_Id", "dbo.GxProduct");
            DropForeignKey("dbo.GxContract", "DetectionScheme_Id", "dbo.GxDetectionScheme");
            DropIndex("dbo.GxContractModification", new[] {"DetectionScheme_Id"});
            DropIndex("dbo.GxContract", new[] {"Product_Id"});
            DropIndex("dbo.GxContract", new[] {"DetectionScheme_Id"});
            DropIndex("dbo.GxDetectionScheme", new[] {"Product_Id"});
            DropIndex("dbo.GxCompact", new[] {"Id"});
            DropColumn("dbo.GxDetectionScheme", "Product_Id");
            DropColumn("dbo.GxContract", "Product_Id");
            DropColumn("dbo.GxContract", "DetectionScheme_Id");
            DropColumn("dbo.GxContractModification", "DetectionScheme_Id");
        }
    }
}