using System.Data.Entity.Migrations;

namespace QualityControl.Migrations
{
    public partial class SgsAndProductRel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxSgsProduct",
                c => new
                {
                    Id = c.Long(false, true),
                    Price = c.Single(false),
                    NeedeDay = c.Int(false),
                    Product_Id = c.Long(),
                    SGS_Id = c.Long()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.Product_Id)
                .ForeignKey("dbo.GxSGS", t => t.SGS_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.SGS_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GxSgsProduct", "SGS_Id", "dbo.GxSGS");
            DropForeignKey("dbo.GxSgsProduct", "Product_Id", "dbo.GxProduct");
            DropIndex("dbo.GxSgsProduct", new[] {"SGS_Id"});
            DropIndex("dbo.GxSgsProduct", new[] {"Product_Id"});
            DropTable("dbo.GxSgsProduct");
        }
    }
}