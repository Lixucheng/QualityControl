using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class baseproductbatchs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxBaseProductBatch",
                c => new
                {
                    Id = c.Long(false, true),
                    ProductId = c.Long(false),
                    BatchName = c.String(),
                    Count = c.Int(false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.ProductId, true)
                .Index(t => t.ProductId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxBaseProductBatch", "ProductId", "dbo.GxProduct");
            DropIndex("dbo.GxBaseProductBatch", new[] {"ProductId"});
            DropTable("dbo.GxBaseProductBatch");
        }
    }
}