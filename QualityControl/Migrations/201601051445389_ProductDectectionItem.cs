namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductDectectionItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxProductDectectionItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Range = c.String(nullable: false),
                        Denney = c.Boolean(nullable: false),
                        Product_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxProductDectectionItem", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxProductDectectionItem", new[] { "Product_Id" });
            DropTable("dbo.GxProductDectectionItem");
        }
    }
}
