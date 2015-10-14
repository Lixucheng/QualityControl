namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baseproductbatchs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxBaseProductBatch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        BatchName = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GxProduct", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GxBaseProductBatch", "ProductId", "dbo.GxProduct");
            DropIndex("dbo.GxBaseProductBatch", new[] { "ProductId" });
            DropTable("dbo.GxBaseProductBatch");
        }
    }
}
