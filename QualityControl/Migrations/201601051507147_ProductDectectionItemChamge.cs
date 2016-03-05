using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class ProductDectectionItemChamge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxProductDectectionItem", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxProductDectectionItem", new[] {"Product_Id"});
            RenameColumn("dbo.GxProductDectectionItem", "Product_Id", "ProductId");
            AlterColumn("dbo.GxProductDectectionItem", "ProductId", c => c.Long(false));
            CreateIndex("dbo.GxProductDectectionItem", "ProductId");
            AddForeignKey("dbo.GxProductDectectionItem", "ProductId", "dbo.GxProduct", "Id", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxProductDectectionItem", "ProductId", "dbo.GxProduct");
            DropIndex("dbo.GxProductDectectionItem", new[] {"ProductId"});
            AlterColumn("dbo.GxProductDectectionItem", "ProductId", c => c.Long());
            RenameColumn("dbo.GxProductDectectionItem", "ProductId", "Product_Id");
            CreateIndex("dbo.GxProductDectectionItem", "Product_Id");
            AddForeignKey("dbo.GxProductDectectionItem", "Product_Id", "dbo.GxProduct", "Id");
        }
    }
}