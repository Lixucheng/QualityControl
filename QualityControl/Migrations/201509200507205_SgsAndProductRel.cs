namespace QualityControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SgsAndProductRel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GxSgsProduct",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Price = c.Single(nullable: false),
                        NeedeDay = c.Int(nullable: false),
                        Product_Id = c.Long(),
                        SGS_Id = c.Long(),
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
            DropIndex("dbo.GxSgsProduct", new[] { "SGS_Id" });
            DropIndex("dbo.GxSgsProduct", new[] { "Product_Id" });
            DropTable("dbo.GxSgsProduct");
        }
    }
}
