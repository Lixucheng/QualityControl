using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class ProductDectectionItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxProductDectectionItem",
                c => new
                {
                    Id = c.Long(false, true),
                    Name = c.String(false),
                    Range = c.String(false),
                    Denney = c.Boolean(false),
                    Product_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.Product_Id)
                .Index(t => t.Product_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxProductDectectionItem", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxProductDectectionItem", new[] {"Product_Id"});
            DropTable("dbo.GxProductDectectionItem");
        }
    }
}