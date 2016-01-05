namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductDectectionItemChamge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GxProductDectectionItem", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxProductDectectionItem", new[] { "Product_Id" });
            RenameColumn(table: "dbo.GxProductDectectionItem", name: "Product_Id", newName: "ProductId");
            AlterColumn("dbo.GxProductDectectionItem", "ProductId", c => c.Long(nullable: false));
            CreateIndex("dbo.GxProductDectectionItem", "ProductId");
            AddForeignKey("dbo.GxProductDectectionItem", "ProductId", "dbo.GxProduct", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxProductDectectionItem", "ProductId", "dbo.GxProduct");
            DropIndex("dbo.GxProductDectectionItem", new[] { "ProductId" });
            AlterColumn("dbo.GxProductDectectionItem", "ProductId", c => c.Long());
            RenameColumn(table: "dbo.GxProductDectectionItem", name: "ProductId", newName: "Product_Id");
            CreateIndex("dbo.GxProductDectectionItem", "Product_Id");
            AddForeignKey("dbo.GxProductDectectionItem", "Product_Id", "dbo.GxProduct", "Id");
        }
    }
}
